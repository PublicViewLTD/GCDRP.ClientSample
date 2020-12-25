using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GCDRP.Client.Dto;
using Nekonya;

namespace GCDRP.Client
{
    class Program
    {
        string mBaseUrl = "https://localhost:5001";
        string mAppSecret = "i4PVGZopvyPiyK7v";
        string mAppSecretHash = "fmadz8kQlpqXX4NpSCgweeJMLt8gkISJp3ovTAnmhpQ=";
        string mAppId = "08d8a89a-d0e0-4745-8676-0277526cf1e6";

        Dictionary<int, DateTime> mDict_NonceCache = new Dictionary<int, DateTime>();
        Random mRandom = new Random();
        

        static void Main(string[] args)
        {
            var p = new Program();
            p.DoMain().Wait();
        }

        public async Task DoMain()
        {
            Console.WriteLine("Hello World!");
            HttpClient client = new HttpClient();
            var c2s_dto = this.GetC2SDto(new GrabageDto
            {
                DeviceCode = "100",
                Weight = 101,
                TrashCode = "102",
                TypeCode = "103",
                ScanningTime = this.GetUtcNowTimeStamp()
            }, mAppId, mAppSecret, mAppSecretHash);

            var c2s_json = JsonSerializer.Serialize(c2s_dto);
            Console.WriteLine("c2s json:\n{0}", c2s_json);
            var resp = await client.PostAsync($"{mBaseUrl}/api/Datas/Grabage", new StringContent(c2s_json, Encoding.UTF8, "application/json"));

            Console.WriteLine($"\n数据发送结果:{(resp.IsSuccessStatusCode ? "成功" : "失败")}\n状态码:{resp.StatusCode} | {(int)resp.StatusCode}");
            if (!resp.IsSuccessStatusCode)
            {
                var result = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(result ?? "无返回内容");
            }

        }


        /// <summary>
        /// 生成客户端发往服务器的数据原型
        /// </summary>
        /// <returns></returns>
        private PushDto<GrabageDto> GetC2SDto(GrabageDto data, 
            string appId, 
            string appSecret, 
            string appSecretHash)
        {
            return new PushDto<GrabageDto>
            {
                Data = data,
                AppId = appId,
                AppSecretHash = appSecretHash,
                //Nonce = 12,
                Nonce = this.GetNonce(),
                TimeStamp = this.GetUtcNowTimeStamp(),
                Signature = this.GetSign(appSecret, 
                    data.DeviceCode,
                    data.Weight.ToString(),
                    data.TrashCode,
                    data.TypeCode,
                    data.ScanningTime.ToString())
            };
        }



        /// <summary>
        /// 获取此刻的时间戳
        /// </summary>
        /// <returns></returns>
        private long GetUtcNowTimeStamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取一个有效的流水号
        /// </summary>
        /// <returns></returns>
        private int GetNonce()
        {
            var i = mRandom.Next(1,1000);
            while (!IsValidNonce(i))
            {
                i = mRandom.Next();
            }
            mDict_NonceCache.AddOrOverride(i, DateTime.UtcNow);
            return i;
        }

        /// <summary>
        /// 是否是有效的流水号（两分钟内不重复）
        /// </summary>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private bool IsValidNonce(int nonce)
        {
            if (mDict_NonceCache.TryGetValue(nonce, out var saveTime))
                return (DateTime.UtcNow - saveTime).TotalMinutes < 2;
            else
                return true;
        }

        /// <summary>
        /// 获取App签名
        /// </summary>
        /// <param name="appSecret"></param>
        /// <param name="dataParams"></param>
        /// <returns></returns>
        private string GetSign(string appSecret, params string[] dataParams)
        {
            string paramsStr = "";
            foreach (var item in dataParams)
                paramsStr += item;
            string secretParams = paramsStr + appSecret;
            return secretParams.GetMD5(true, true);
        }

    }
}

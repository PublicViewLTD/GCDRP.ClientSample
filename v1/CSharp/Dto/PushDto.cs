using System;
using System.Text.Json.Serialization;

namespace GCDRP.Client.Dto
{
    public class PushDto
    {
        /// <summary>
        /// AppID
        /// </summary>
        [JsonPropertyName("app")]
        public string AppId { get; set; }

        /// <summary>
        /// App Secret Hash 登录开发者面板获取
        /// </summary>
        [JsonPropertyName("secret")]
        public string AppSecretHash { get; set; }

        /// <summary>
        /// 临时流水号（请确保该值在2分钟内不重复）
        /// </summary>
        [JsonPropertyName("nonce")]
        public int Nonce { get; set; }

        /// <summary>
        /// 签名字段（生成规则请参考文档）
        /// </summary>
        [JsonPropertyName("sign")]
        public string Signature { get; set; }

        /// <summary>
        /// 时间戳 （Unix时间戳）
        /// </summary>
        //[JsonConverter(typeof(DateTimeTimeStampConverter))]
        [JsonPropertyName("time")]
        public long TimeStamp { get; set; }
    }

    public class PushDto<T> : PushDto
    {
        /// <summary>
        /// 推送数据主体
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

}

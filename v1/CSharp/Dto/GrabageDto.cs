using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GCDRP.Client.Dto
{
    public class GrabageDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [JsonPropertyName("device")]
        public string DeviceCode { get; set; }

        /// <summary>
        /// 称重数据
        /// </summary>
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }


        /// <summary>
        /// 垃圾桶编码
        /// </summary>
        [JsonPropertyName("trash")]
        public string TrashCode { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonPropertyName("type")]
        public string TypeCode { get; set; }


        /// <summary>
        /// 数据生成时间 Unix时间戳
        /// </summary>
        [JsonPropertyName("scanningTime")]
        public long ScanningTime { get; set; }
    }
}

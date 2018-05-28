using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentYoutuYun.SDK.Csharp.Model
{
    /// <summary>
    /// 人脸图片信息
    /// </summary>
    public class FaceImage
    {
        /// <summary>
        /// 包含人脸的图片数据
        /// </summary>
        public string image_path { get; set; }
        /// <summary>
        /// 是否为网络图片
        /// </summary>
        public bool isWebImage { get; set; }
    }
}

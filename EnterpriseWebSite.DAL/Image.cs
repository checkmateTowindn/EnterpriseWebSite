using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [Serializable]
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alt { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public ImgType Type { get; set; }
    }
    /// <summary>
    /// 图片格式
    /// </summary>
    public enum ImgType
    {
        jpeg=0,
        jpg =1,
        png=2,
        gif=3,
        bmp=4,
    }
}

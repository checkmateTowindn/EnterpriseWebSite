using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace EnterpriseWebSite.Common
{
    public class Com
    {
        /// <summary>
        /// 验证电子邮件
        /// </summary>
        /// <param name="str_mail"></param>
        /// <returns></returns>
        public static bool isMail(string str_mail)
        {
            return (System.Text.RegularExpressions.Regex.IsMatch(str_mail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")) ? true : false;
        }
        /// <summary>
        /// 检查输入是否是字母、数字、下划线或-
        /// </summary>
        /// <param name="strTemp"></param>
        /// <returns></returns>
        public static bool CheckAccount(string strTemp)
        {
            Regex myreg = new Regex(@"^[A-Za-z_0-9\-]+$");
            return (myreg.IsMatch(strTemp)) ? true : false;
        }
        /// <summary>
        /// 只能输入非零的正整数
        /// </summary>
        /// <param name="strTemp"></param>
        /// <returns></returns>
        public static bool CheckNumber(string strTemp)
        {
            Regex myreg = new Regex(@"^\+?[1-9][0-9]*$");
            return (myreg.IsMatch(strTemp)) ? true : false;
        }
        /// <summary>
        /// 输入字符过滤
        /// </summary>
        /// <param name="NoHTML">包括HTML，脚本，数据库关键字，特殊字符的源码 </param>
        /// <returns>已经去除标记后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return "";
            }
            else
            {
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&*.out", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&*.exe", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&*.bat", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop database", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "'", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"""", "", RegexOptions.IgnoreCase);
                return Htmlstring;
            }
        }
        /// <summary>
        /// 验证电话号码或手机号码
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool isTelephone(string str_telephone)
        {
            if (string.IsNullOrEmpty(str_telephone))
            {
                return false;
            }
            //电信手机号码正则        
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //联通手机号正则        
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex tReg = new Regex(liantong);
            //移动手机号正则        
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);
            //座机   
            /* 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
                也可以不用，区号与本地号间可以用-连字号或空格间隔，
                也可以没有间隔           
            */
            string zuoji = @"^\(010|02[012345789]\)[- ]\d{8}$";
            string zuoji1 = @"^010|02[012345789][- ]\d{8}$";
            string zuoji2 = @"^\(0[345789]\d{2}\)[- ]\d{7}$";
            string zuoji3 = @"^0[345789]\d{2}[- ]\d{7}$";
            string zuoji4 = @"^\(010|02[012345789]\)\d{8}$";
            string zuoji5 = @"^010|02[012345789]\d{8}$";
            string zuoji6 = @"^\(0[345789]\d{2}\)\d{7}$";
            string zuoji7 = @"^0[345789]\d{2}\d{7}$";
            Regex zReg = new Regex(zuoji);
            Regex zReg1 = new Regex(zuoji1);
            Regex zReg2 = new Regex(zuoji2);
            Regex zReg3 = new Regex(zuoji3);
            Regex zReg4 = new Regex(zuoji4);
            Regex zReg5 = new Regex(zuoji5);
            Regex zReg6 = new Regex(zuoji6);
            Regex zReg7 = new Regex(zuoji7);
            return (dReg.IsMatch(str_telephone) ||
                tReg.IsMatch(str_telephone) ||
                yReg.IsMatch(str_telephone) ||
                zReg.IsMatch(str_telephone) ||
                zReg1.IsMatch(str_telephone) ||
                zReg2.IsMatch(str_telephone) ||
                zReg3.IsMatch(str_telephone) ||
                zReg4.IsMatch(str_telephone) ||
                zReg5.IsMatch(str_telephone) ||
                zReg6.IsMatch(str_telephone) ||
                zReg7.IsMatch(str_telephone)) ? true : false;

        }
        /// <summary>
        ///  验证邮编
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }

        //取得当前访问的IP
        public static string GetCurrentLoginIP()
        {
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            string ipstr = string.Empty;
            string hostName = Dns.GetHostName();//获得本地计算机名称
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);//IP主机
            for (int i = 0; i < addressList.Length; i++)
            {
                //if (hostEntry.AddressList[i].AddressFamily.ToString() == "InterNetwork" ||
                //    hostEntry.AddressList[i].AddressFamily.ToString() == "InterNetworkV6")
                //{
                //    ipstr = addressList[0].ToString();
                //    break;
                //}
                if (hostEntry.AddressList[i].AddressFamily.ToString() == "InterNetwork")
                {
                    ipstr = addressList[i].ToString();
                    break;
                }
                //IPv4 可以使用 hostEntry.AddressList[i].AddressFamily.ToString() == "InterNetwork"
                //IPv6可以使用hostEntry.AddressList[i].AddressFamily.ToString() == "InterNetworkV6"                           
            }
            return ipstr;
        }

        //加密
        #region 不可逆 散列算法
        ///// <summary>
        ///// MD5对字符串加密
        ///// </summary>
        ///// <param name="str">要加密的字符串</param>
        ///// <param name="i">16或32</param>
        ///// <returns>加密后的字符串，失败返源串</returns>
        //public static string MD5Encrypt(string str, int i)
        //{
        //    //获取要加密的字段，并转化为Byte[]数组
        //    byte[] data = System.Text.Encoding.Unicode.GetBytes(str.ToCharArray());
        //    //建立加密服务
        //    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    //加密Byte[]数组
        //    byte[] result = md5.ComputeHash(data);
        //    //将加密后的数组转化为字段
        //    if (i == 16 && str != string.Empty)
        //    {
        //        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
        //    }
        //    else if (i == 32 && str != string.Empty)
        //    {
        //        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        //    }
        //    else
        //    {
        //        switch (i)
        //        {
        //            case 16: return "000000000000000";
        //            case 32: return "000000000000000000000000000000";
        //            default: return str;
        //        }
        //    }
        //}
        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static string MD5Encrypt(Stream stream)
        {
            MD5 md5serv = MD5CryptoServiceProvider.Create();
            byte[] buffer = md5serv.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            foreach (byte var in buffer)
                sb.Append(var.ToString("x2"));
            return sb.ToString();
        }
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Default.GetBytes(str));
            s256.Clear();
            return Convert.ToBase64String(byte1);
        }
        /// <summary>
        /// SHA384加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA384Encrypt(string str)
        {
            System.Security.Cryptography.SHA384 s384 = new System.Security.Cryptography.SHA384Managed();
            byte[] byte1;
            byte1 = s384.ComputeHash(Encoding.Default.GetBytes(str));
            s384.Clear();
            return Convert.ToBase64String(byte1);
        }
        /// <summary>
        /// SHA512加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA512Encrypt(string str)
        {
            System.Security.Cryptography.SHA512 s512 = new System.Security.Cryptography.SHA512Managed();
            byte[] byte1;
            byte1 = s512.ComputeHash(Encoding.Default.GetBytes(str));
            s512.Clear();
            return Convert.ToBase64String(byte1);
        }
        #endregion
    }
}
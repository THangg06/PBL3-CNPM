//<<<<<<< HEAD
//﻿using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace Demo.Extension
//{
//    public static class  HashMD5
//    {
//        public static string ToMD5(this string str)
//    {
//            // bảo mật password
//            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
//            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
//            StringBuilder sbHash = new StringBuilder();
//            foreach (byte b in bHash)
//            {
//                sbHash.Append(String.Format("{0:x2}", b));
//            }
//            return sbHash.ToString() ;
//    }
//}
//}
=======
﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Extension
{
    public static class  HashMD5
    {
        public static string ToMD5(this string str)
    {
            // bảo mật password
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString() ;
    }
}
}
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228

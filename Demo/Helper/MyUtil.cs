//<<<<<<< HEAD
//﻿using System.Text;

//namespace Demo.Helper
//{
//    public class MyUtil
//    {
//        public static bool IsValidEmail(string email)
//        {
//            if(email.Trim().EndsWith("."))
//            {
//                return false;
//            }    
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch { return false; }
//        }
//        public static string UploadHinh(IFormFile file, string folder)
//        {
//            try {
//                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, file.FileName);
//                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
//                {
//                    file.CopyTo(myfile);
//                }
//            return file.FileName;
//            } catch(Exception e) {
//                return string.Empty;
//            }
//        }
//        public static string GenerateRandomKey(int lenght = 5)
//        {
//            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!!";
//            var sb= new StringBuilder();
//            var rd = new Random(lenght);
//            for(int i=0; i < lenght; i++)
//            {
//                sb.Append(pattern[rd.Next(0, pattern.Length)]);
//            } 
//            return sb.ToString();
//        }
//    }
//}
=======
﻿using System.Text;
using System.Text.RegularExpressions;

namespace Demo.Helper
{
    public class MyUtil
    {

        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if(!string.IsNullOrEmpty(result))
            {
                var words = str.Split(' ');
                for(int i = 0; i < words.Length; i++)
                {
                    var s = words[i];
                    if (s.Length>0)
                    {
                        words[i] = s[0].ToString().ToUpper() + s.Substring(1);

                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            try
            {
                if(String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if(!regex.IsMatch(str))
                {
                    return false;   
                }
            }
            catch {  }
            return true;
        }
        public static bool IsValidEmail(string email)
        {
            if(email.Trim().EndsWith("."))
            {
                return false;
            }    
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }
        public static string UploadHinh(IFormFile file, string folder)
        {
            try {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MenuCss", folder, file.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    file.CopyTo(myfile);
                }
            return file.FileName;
            } catch(Exception e) {
                return string.Empty;
            }
        }

        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"áàạảãấầậẩẫắằặẵẳ", "a");
            url = Regex.Replace(url, @"éèẹẽẻếềệểễ", "e");
            url = Regex.Replace(url, @"óòọỏõớờợỡởốồộỗổ", "o");
            url = Regex.Replace(url, @"úùụủũứựừữử", "u");
            url = Regex.Replace(url, @"íìịỉĩ", "i");
            url = Regex.Replace(url, @"ýỳỵỷỹ", "y");
            url = Regex.Replace(url, @"đ", "d");
            //chi cho phep nhan
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            url = Regex.Replace(url, @"\s", "-");
            while(true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else break;
            }
            return url;
        }

        public static async Task<string> UploadFile(IFormFile file, string directory, string newName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menucss", directory, newName);

            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "jfif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower().TrimStart('.');

            if (!supportedTypes.Contains(fileExtension))
            {
                return null;
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return newName;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GenerateRandomKey(int lenght = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!!";
            var sb= new StringBuilder();
            var rd = new Random(lenght);
            for(int i=0; i < lenght; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            } 
            return sb.ToString();
        }
    }
}
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228

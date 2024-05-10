using System.Text;

namespace Demo.Helper
{
    public class MyUtil
    {
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
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, file.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    file.CopyTo(myfile);
                }
            return file.FileName;
            } catch(Exception e) {
                return string.Empty;
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

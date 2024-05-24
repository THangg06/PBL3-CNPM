namespace Demo.Extension
{
    public static class Extension
    {
        public static string ToVnd(this double donGia)
        {
            return donGia.ToString("#,##0") + " d";
        }
        //public static string ToTitleCase(string str)
        //{
        //    string resuit = str;
        //    if(!string.IsNullOrEmpty(str)) {
        //    var words = str.Split(' ');
        //        for(int index = 0; index < words.Length; index++)
        //        {
        //            vả
        //        }
        //    }
        //}
    }
}

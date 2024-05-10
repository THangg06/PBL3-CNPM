//<<<<<<< HEAD
//﻿using Newtonsoft.Json;

//namespace Demo.Extension
//{
//    public static class SectionExtentions
//    {
//        public static void Set<T>(this ISession session, string key, T value)
//        {
//            session.SetString(key, JsonConvert.SerializeObject(value));
//        }  
        
//        public static T Get<T>(this ISession session, string key)
//        {
//            var value = session.GetString(key);
//            return value == null ? default(T) :
//                JsonConvert.DeserializeObject<T>(value);
//        }

//    }
//}
//=======
﻿using Newtonsoft.Json;

namespace Demo.Extension
{
    public static class SectionExtentions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }  
        
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

    }
}
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228

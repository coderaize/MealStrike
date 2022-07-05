using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace MealApp
{
    public static class Json
    {
        public static string AsJson(this object ToJsonObj)
        {
            return JsonConvert.SerializeObject(ToJsonObj);
        }

        public static T FromJson<T>(this string FromJsonStr)
        {
            return JsonConvert.DeserializeObject<T>(FromJsonStr);
        }
    }
}

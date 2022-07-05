using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MealApp
{
    public static class RuntimeCache
    {
        public static string UserName { get; set; } //= "devTime";

        public static DateTime TestDT { get; set; } = new DateTime();

        public static Cart Cart { get; set; } = new Cart();

    }
}

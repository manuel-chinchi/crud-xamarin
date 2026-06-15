using System;
using System.Collections.Generic;
using System.Text;

namespace crud_xamarin.Shared
{
    static class GlobalHelper
    {
        static Random s_rand = new Random();

        public static int NewId()
        {
            int id = s_rand.Next(1, 4000);
            return id;
        }
    }
}

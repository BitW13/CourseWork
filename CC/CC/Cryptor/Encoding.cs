using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CC.Cryptor
{
    public class Encoding
    {
        public static string GetCrypt(string value)
        {
            var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(valueBytes);

            //return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
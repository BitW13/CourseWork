using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CC.Cryptor
{
    public class Decoding
    {
        public static string GetDecrypt(string encrValue)
        {
            var value = Convert.FromBase64String(encrValue);

            return System.Text.Encoding.UTF8.GetString(value);
        }
    }
}
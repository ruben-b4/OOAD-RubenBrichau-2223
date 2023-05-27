using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class StringUtils
    {
        public static string ToSha256(string cleartext)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputByes = Encoding.UTF8.GetBytes(cleartext);
            byte[] hashedPasswordBytes = sha256.ComputeHash(inputByes);
            string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "");
            return hashedPassword;
        }
    }
}

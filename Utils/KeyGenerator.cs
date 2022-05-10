using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TinyUrl.Models;

namespace TinyUrl.Utils
{
    public class KeyGenerator
    {
        const string KeyCharacterSet = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";   //62^6 unique keys possible.
        private readonly Random random;
        private static readonly Lazy<KeyGenerator> lazy = new(() => new KeyGenerator());

        public static KeyGenerator Instance { get { return lazy.Value; } }

        private KeyGenerator()
        {
            random = new Random();
        }
       
        public string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}

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
            // calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // convert byte array to base64 string
            return Convert.ToBase64String(hashBytes);
        }
    }
}

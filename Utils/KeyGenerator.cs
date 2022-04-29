using System;
using System.Linq;
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
       
        public string Generate()
        {
            return new string(Enumerable.Repeat(KeyCharacterSet, Constants.SHORT_URL_LENGTH)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

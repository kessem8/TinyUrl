using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public interface IUrlServices
    {
        Url CreateShortUrl(string originalUrl);
        string GetFullUrl(string shortUrl);
    }
}


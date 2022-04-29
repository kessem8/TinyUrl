using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public interface IUrlCache
    {
        void Add(Url url);
        Url GetValueBykey(string key);
    }
}

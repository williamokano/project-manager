using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Katapoka.BLL.Utilitarios
{
    public class Criptografia
    {
        public static string GetHash(string str)
        {
            SHA512 alg = SHA512.Create();
            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(str));
            return string.Concat(result.Select(p => p.ToString("X2")));
        }
    }
}

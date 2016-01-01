using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Utils.Sequrity
{
    public static class Encrypter
    {
        public static string HashText(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();
            byte[] byteHash = cryptoService.ComputeHash(bytes);

            return new Guid(byteHash).ToString();
        }
    }
}

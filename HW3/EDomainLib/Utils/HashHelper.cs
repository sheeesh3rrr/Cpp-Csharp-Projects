using System.Security.Cryptography;
using System.Text;

namespace EDomainLib.Utils
{
    public static class HashHelper
    {
        public static string Sha256Hex(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Sha256Hex(bytes);
        }

        public static string Sha256Hex(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            using SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(bytes);
            StringBuilder sb = new(hash.Length * 2);
            foreach (byte b in hash)
            {
                _ = sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}

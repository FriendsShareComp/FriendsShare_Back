
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Security
{
    public class Encrypt
    {
        public static string encryption(string str)
        {
            SHA256 sha256= SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] stream = null;
            stream=sha256.ComputeHash(encoding.GetBytes(str));
            for(int i = 0; i < stream.Length; i++) stringBuilder.AppendFormat("{0:x2}", stream[i]);
            return stringBuilder.ToString();
        }
    }
}

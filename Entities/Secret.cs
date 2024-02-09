using System.Text;

namespace SecureSoftware.Entities
{
    public class Secret
    {

        public byte[] EncryptKey { get; set; }

        public byte[] EncryptIV { get; set; }

        public Secret()
        { 
            EncryptKey = Encoding.UTF8.GetBytes("nqKA02KCk0wgDZhvcLrHbDXTmhleNZHb");
            EncryptIV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");
        }

    }
}

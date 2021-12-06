using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace ProjectBank.Infrastructure
{
    public class Signature
    {
        public int Id {get; set;}
        public IReadOnlyCollection<string> Hashes;

        public Signature(IReadOnlyCollection<Tag> tags)
        {
            if(tags.Count() == 0) {throw new ArgumentException("Cannot initialize a signature with an empty list of Tag.");}
            Hashes = GetHashedTags(tags);
        }

        public Signature() {}

        private IReadOnlyCollection<string> GetHashedTags(IReadOnlyCollection<Tag> tags)
        {
            string minSHA1 = "å";
            string minSHA256 = "å";
            string minMD5 = "å";
            string minSHA384 = "å";
            string minSHA512 = "å";
            string minNoHash = "å";
            var builder = new HashBuilder();

            foreach (Tag tag in tags)
            {
                var SHA1Value = builder.HashString(tag.Name, SHA1.Create());
                if (minSHA1 == "å" || minSHA1.CompareTo(SHA1Value) > 0) minSHA1 = SHA1Value;
                
                var SHA256Value = builder.HashString(tag.Name, SHA256.Create());
                if (minSHA256 == "å" || minSHA256.CompareTo(SHA256Value) > 0) minSHA256 = SHA256Value;

                var MD5Value = builder.HashString(tag.Name, MD5.Create());
                if (minMD5 == "å" || minMD5.CompareTo(MD5Value) > 0) minMD5 = MD5Value;

                var SHA384Value = builder.HashString(tag.Name, SHA384.Create());
                if (minSHA384 == "å" || minSHA384.CompareTo(SHA384Value) > 0) minSHA384 = SHA384Value;

                var SHA512Value = builder.HashString(tag.Name, SHA512.Create());
                if (minSHA512 == "å" || minSHA512.CompareTo(SHA512Value) > 0) minSHA512 = SHA512Value;

                var NoHash = tag.Name;
                if (minNoHash == "å" || minNoHash.CompareTo(NoHash) > 0) minNoHash = NoHash;
            }

            return new ReadOnlyCollection<string>(new List<string> { minSHA1, minSHA256, minMD5, minSHA384, minSHA512, minNoHash});

        }
/*
        private string ToString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private string ComputeSHA256(string str)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var code = ToString(bytes);
                return code;
            }
        }

        private string ComputeSHA1(string str)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var code = ToString(bytes);
                return code;
            }
        }

        private string ComputeSHA384(string str)
        {
            using (SHA384 sha384Hash = SHA384.Create())
            {
                byte[] bytes = sha384Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var code = ToString(bytes);
                return code;
            }
        }
        
        private string ComputeSHA512(string str)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var code = ToString(bytes);
                return code;
            }
        }

        private string ComputeMD5(string str)
        {
            using (MD5 MD5Hash = MD5.Create())
            {
                byte[] bytes = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var code = ToString(bytes);
                return code;
            }
        }
         */
    }
}

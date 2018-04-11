using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestConsole.cookie
{
    public class HashEncrypt
    {
        public static string SHA1Encrypt(string data)
        {
            SHA1 sHA = new SHA1Managed();
            byte[] value = sHA.ComputeHash(Encoding.ASCII.GetBytes(data));
            string text = BitConverter.ToString(value);
            text = text.Replace("-", "");
            sHA.Clear();
            return text;
        }

        public static string SHA256Encrypt(string data)
        {
            SHA256 sHA = new SHA256Managed();
            byte[] value = sHA.ComputeHash(Encoding.ASCII.GetBytes(data));
            string text = BitConverter.ToString(value);
            text = text.Replace("-", "");
            sHA.Clear();
            return text;
        }

        public static string SHA512Encrypt(string data)
        {
            SHA512 sHA = new SHA512Managed();
            byte[] value = sHA.ComputeHash(Encoding.ASCII.GetBytes(data));
            string text = BitConverter.ToString(value);
            text = text.Replace("-", "");
            sHA.Clear();
            return text;
        }

        public static string DESEncrypt(string originalValue, string key, string IV)
        {
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            ICryptoTransform transform = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(IV)
            }.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(originalValue);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string DESEncrypt(string originalValue, string key)
        {
            return HashEncrypt.DESEncrypt(originalValue, key, key);
        }

        public static string DESDecrypt(string encryptedValue, string key, string IV)
        {
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            ICryptoTransform transform = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(IV)
            }.CreateDecryptor();
            byte[] array = Convert.FromBase64String(encryptedValue);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static string DESDecrypt(string encryptedValue, string key)
        {
            return HashEncrypt.DESDecrypt(encryptedValue, key, key);
        }
    }
}



using System;
using System.Text;

namespace TestConsole.cookie
{
    public class RC4Encrypt
    {
        public enum EncoderMode
        {
            Base64Encoder,
            HexEncoder
        }

        private static Encoding Encode = Encoding.Default;

        public static string Encrypt(string data, string pass, RC4Encrypt.EncoderMode em)
        {
            bool flag = data == null || pass == null;
            string result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = em == RC4Encrypt.EncoderMode.Base64Encoder;
                if (flag2)
                {
                    result = Convert.ToBase64String(RC4Encrypt.EncryptEx(RC4Encrypt.Encode.GetBytes(data), pass));
                }
                else
                {
                    result = RC4Encrypt.ByteToHex(RC4Encrypt.EncryptEx(RC4Encrypt.Encode.GetBytes(data), pass));
                }
            }
            return result;
        }

        public static string Decrypt(string data, string pass, RC4Encrypt.EncoderMode em)
        {
            bool flag = data == null || pass == null;
            string result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = em == RC4Encrypt.EncoderMode.Base64Encoder;
                if (flag2)
                {
                    result = RC4Encrypt.Encode.GetString(RC4Encrypt.DecryptEx(Convert.FromBase64String(data), pass));
                }
                else
                {
                    result = RC4Encrypt.Encode.GetString(RC4Encrypt.DecryptEx(RC4Encrypt.HexToByte(data), pass));
                }
            }
            return result;
        }

        public static string Encrypt(string data, string pass)
        {
            return RC4Encrypt.Encrypt(data, pass, RC4Encrypt.EncoderMode.Base64Encoder);
        }

        public static string Decrypt(string data, string pass)
        {
            return RC4Encrypt.Decrypt(data, pass, RC4Encrypt.EncoderMode.Base64Encoder);
        }

        private static byte[] EncryptEx(byte[] data, string pass)
        {
            bool flag = data == null || pass == null;
            byte[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                byte[] array = new byte[data.Length];
                long num = 0L;
                long num2 = 0L;
                byte[] key = RC4Encrypt.GetKey(RC4Encrypt.Encode.GetBytes(pass), 256);
                for (long num3 = 0L; num3 < (long)data.Length; num3 += 1L)
                {
                    num = (num + 1L) % (long)key.Length;
                    num2 = (num2 + (long)((ulong)key[(int)(checked((IntPtr)num))])) % (long)key.Length;
                    checked
                    {
                        byte b = key[(int)((IntPtr)num)];
                        key[(int)((IntPtr)num)] = key[(int)((IntPtr)num2)];
                        key[(int)((IntPtr)num2)] = b;
                        byte b2 = data[(int)((IntPtr)num3)];
                        byte b3 = key[(int)(unchecked(key[(int)(checked((IntPtr)num))] + key[(int)(checked((IntPtr)num2))])) % key.Length];
                        array[(int)((IntPtr)num3)] = (byte)(b2^b3);
                    }
                }
                result = array;
            }
            return result;
        }

        private static byte[] DecryptEx(byte[] data, string pass)
        {
            return RC4Encrypt.EncryptEx(data, pass);
        }

        public static byte[] HexToByte(string szHex)
        {
            int length = szHex.Length;
            bool flag = length <= 0 || length % 2 != 0;
            byte[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                int num = length / 2;
                byte[] array = new byte[num];
                for (int i = 0; i < num; i++)
                {
                    uint num2 = (uint)(szHex[i * 2] - ((szHex[i * 2] >= 'A') ? '7' : '0'));
                    bool flag2 = num2 >= 16u;
                    if (flag2)
                    {
                        result = null;
                        return result;
                    }
                    uint num3 = (uint)(szHex[i * 2 + 1] - ((szHex[i * 2 + 1] >= 'A') ? '7' : '0'));
                    bool flag3 = num3 >= 16u;
                    if (flag3)
                    {
                        result = null;
                        return result;
                    }
                    array[i] = (byte)(num2 * 16u + num3);
                }
                result = array;
            }
            return result;
        }

        public static string ByteToHex(byte[] vByte)
        {
            bool flag = vByte == null || vByte.Length < 1;
            string result;
            if (flag)
            {
                result = null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(vByte.Length * 2);
                for (int i = 0; i < vByte.Length; i++)
                {
                    bool flag2 = vByte[i] < 0;
                    if (flag2)
                    {
                        result = null;
                        return result;
                    }
                    uint num = (uint)(vByte[i] / 16);
                    stringBuilder.Append((char)((ulong)num + (ulong)((num > 9u) ? 55L : 48L)));
                    num = (uint)(vByte[i] % 16);
                    stringBuilder.Append((char)((ulong)num + (ulong)((num > 9u) ? 55L : 48L)));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        private static byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] array = new byte[kLen];
            for (long num = 0L; num < (long)kLen; num += 1L)
            {
                array[(int)(checked((IntPtr)num))] = (byte)num;
            }
            long num2 = 0L;
            for (long num3 = 0L; num3 < (long)kLen; num3 += 1L)
            {
                num2 = (num2 + (long)((ulong)array[(int)(checked((IntPtr)num3))]) + (long)((ulong)pass[(int)(checked((IntPtr)(num3 % unchecked((long)pass.Length))))])) % (long)kLen;
                checked
                {
                    byte b = array[(int)((IntPtr)num3)];
                    array[(int)((IntPtr)num3)] = array[(int)((IntPtr)num2)];
                    array[(int)((IntPtr)num2)] = b;
                }
            }
            return array;
        }
    }
}


using System.Collections.Generic;

namespace TestConsole.cookie
{
    public class SecurityManagerT
    {
        public static string Encrypt<T>(T obj, Dictionary<string,string> parameters)
        {
            var str = ServiceStack.Text.JsonSerializer.SerializeToString<T>(obj);

            var rc4Str = RC4Encrypt.Encrypt(str, parameters["rc4key"], RC4Encrypt.EncoderMode.HexEncoder).Trim();
            var hasStr = HashEncrypt.SHA1Encrypt(rc4Str + parameters["hashkey"]);

            return hasStr + rc4Str;
        }

        public static T DeEncrypt<T>(string mess, Dictionary<string, string>parameters)
        {
            var hashStr = mess.Substring(0, 40);
            var rc4Str = mess.Substring(40);

            //这个用加密的方式去比较相等不
            var temphashStr =  HashEncrypt.SHA1Encrypt(rc4Str + parameters["hashkey"]);
            if (!temphashStr.Equals(hashStr))
                return default(T);

            //解密放到客户端的数据  
            var rc4deEncryptStr = RC4Encrypt.Decrypt(rc4Str, parameters["rc4key"], RC4Encrypt.EncoderMode.HexEncoder);
            if (string.IsNullOrWhiteSpace(rc4deEncryptStr))
                return default(T);

            return
                ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(rc4deEncryptStr);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.cookie
{
    public class User
    {
        public int Id { get; set; }

        public DateTime BeginTime { get; set; }
    }
}


var encryptStr = 
            SecurityManagerT.Encrypt<User>(new User
            {
                Id = 6,
                BeginTime = DateTime.Now.AddDays(1)
            }, new Dictionary<string, string> {
                    {"rc4key", "123456"},
                    {"hashkey", "asdfgh"}
            });

            Console.WriteLine(encryptStr);

            var result = 
                SecurityManagerT.DeEncrypt<User>(
                    encryptStr, 
                    new Dictionary<string, string> {
                        {"rc4key", "123456"},
                        {"hashkey", "asdfgh"}
                });



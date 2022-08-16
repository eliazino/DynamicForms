using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Shared {
    public class Cryptography {

        public class Hash {
            public static string getHash(string rawData) {
                using (SHA256 sha256Hash = SHA256.Create()) {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++) {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }

        public class CharGenerator {
            private static readonly List<string> templates = new List<string> { "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890", "1234567890abcdef", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz" };
            public static string genID(int counts, characterSet type = 0) {
                char[] generated = new char[counts];
                char[] characters = templates[(int)type].ToCharArray();
                var random = new Random();
                int sampleLength = characters.Length -1;
                for (int i = 0; i < counts; i++) {
                    int index = random.Next(0, sampleLength);
                    generated[i] = characters[index];
                }                
                return new string(generated);
            }

            public static string genID() {
                Guid id = Guid.NewGuid();
                return id.ToString();
            }
            
            public enum characterSet {
                NUMERIC = 2,
                ALPHA_NUMERIC_NON_CASE = 0,
                ALPHA_NUMERIC_CASE = 1,
                HEX_STRING = 3,
                GUID = 6,
                UPPER_ALPHABETS_ONLY = 4,
                LOWER_ALPHABETS_ONLY = 5
            }
        }

        public class AES {
            private CipherMode mode = CipherMode.CBC;
            private PaddingMode paddingMode = PaddingMode.PKCS7;
            private string IV = string.Empty;
            private string key = string.Empty;
            public int blockSize = 128, keySize = 128;            
            public AES(CipherMode cipher, PaddingMode padding, string key, string IV = null) {
                this.mode = cipher;
                this.paddingMode = padding;
                this.key = key;
                this.IV = string.IsNullOrEmpty(IV)? string.Empty : IV;
            }
            public AES() {

            }
            private RijndaelManaged getRijndaelManaged(string secretKey) {
                var keyBytes = new byte[16];
                var IVBytes = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
                Array.Copy(keyBytes, IVBytes, Math.Min(keyBytes.Length, IVBytes.Length));
                if (this.IV.Length > 0) {
                    var IVB = Encoding.UTF8.GetBytes(this.IV);
                    Array.Copy(IVB, IVBytes, Math.Min(IVBytes.Length, IVB.Length));
                }
                return new RijndaelManaged {
                    Mode = this.mode,
                    Padding = this.paddingMode,
                    KeySize = keySize,
                    BlockSize = blockSize,
                    Key = keyBytes,
                    IV = IVBytes
                };
            }
            private AesManaged getAesManaged(string secretKey) {
                byte[] secretKeyBytesRaw = new byte[16];
                byte[] IVBytesRaw = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                Array.Copy(secretKeyBytes, secretKeyBytesRaw, Math.Min(secretKeyBytesRaw.Length, secretKeyBytes.Length));
                IVBytesRaw = secretKeyBytesRaw;
                if (this.IV.Length > 0) {
                    byte[] IVBytes = Encoding.UTF8.GetBytes(this.IV);
                    Array.Copy(IVBytes, IVBytesRaw, Math.Min(IVBytes.Length, IVBytesRaw.Length));
                }
                return new AesManaged {
                    Mode = this.mode,
                    Padding = this.paddingMode,
                    Key = secretKeyBytesRaw,
                    IV = IVBytesRaw
                };
            }

            private byte[] encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged) {
                return rijndaelManaged.CreateEncryptor()
                    .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
            private byte[] decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged) {
                return rijndaelManaged.CreateDecryptor()
                    .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
            private byte[] encrypt(byte[] plainBytes, AesManaged aesManaged) {
                return aesManaged.CreateEncryptor()
                    .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
            private byte[] decrypt(byte[] encryptedData, AesManaged aesManaged) {
                return aesManaged.CreateDecryptor()
                    .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }

            public string encrypt(string plainText, string key) {
                try {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(encrypt(plainBytes, getRijndaelManaged(key)));
                } catch {
                    return null;
                }
            }
            public string decrypt(string encryptedText, string key) {
                try {
                    var encryptedBytes = Convert.FromBase64String(encryptedText);
                    var t = decrypt(encryptedBytes, getRijndaelManaged(key));
                    return Encoding.UTF8.GetString(t);
                } catch {
                    return null;
                }
            }
            public string encryptAesManaged(string plainText, string key) {
                try {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(encrypt(plainBytes, getAesManaged(key)));
                } catch {
                    return null;
                }
            }
            public string decryptAesManaged(string encryptedText, string key) {
                try {
                    var encryptedBytes = Convert.FromBase64String(encryptedText);
                    return Encoding.UTF8.GetString(decrypt(encryptedBytes, getAesManaged(key)));
                } catch {
                    return null;
                }
            }

            public string encrypt(string plainText) {
                try {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(encrypt(plainBytes, getRijndaelManaged(key)));
                } catch {
                    return null;
                }
            }
            public string decrypt(string encryptedText) {
                try {
                    var encryptedBytes = Convert.FromBase64String(encryptedText);
                    var t = decrypt(encryptedBytes, getRijndaelManaged(key));
                    return Encoding.UTF8.GetString(t);
                } catch {
                    return null;
                }
            }
            public string encryptAesManaged(string plainText, bool returnHexStr = false) {
                try {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    var byteArr = encrypt(plainBytes, getAesManaged(key));
                    if (returnHexStr) {
                        return Utilities.byteArrToHexStr(byteArr);
                    }
                    return Convert.ToBase64String(byteArr);
                } catch {
                    return null;
                }
            }
            public string decryptAesManaged(string encryptedText, bool inputIsHexStr = false) {
                try {
                    byte[] encryptedBytes;
                    if (inputIsHexStr) {
                        encryptedBytes = Utilities.hexStringToByteArray(encryptedText);
                    } else {
                        encryptedBytes = Convert.FromBase64String(encryptedText);
                    }
                    return Encoding.UTF8.GetString(decrypt(encryptedBytes, getAesManaged(key)));
                } catch {
                    return null;
                }
            }

            public string encrypt(object obj, string key) {
                try {
                    string plainText = JObject.FromObject(obj).ToString();
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(encrypt(plainBytes, getRijndaelManaged(key)));
                } catch {
                    return null;
                }
            }
            public string encrypt(object obj) {
                try {
                    string plainText = JObject.FromObject(obj).ToString();
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(encrypt(plainBytes, getRijndaelManaged(key)));
                } catch {
                    throw;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using UHWID;

namespace Core
{
    /// <summary>
    /// Encryption/Decryption class based on local machine HWID 
    /// using the follwing lib: https://github.com/davcs86/csharp-uhwid 
    /// </summary>
    public class Encryption
    {
        //Declare variables for HWID strings
        private static string SimpleUID = UHWIDEngine.SimpleUid;
        private static string AdvancedUID = UHWIDEngine.AdvancedUid;
        private static string _KEY = SimpleUID+AdvancedUID;
        //--------------------------------------

        /// <summary>
        /// decryption function
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string _decryptData(string data)
        {
            string DecryptedData=string.Empty;
            try
            {
                string InputText = data;
                string Password = _KEY;
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = System.Text.Encoding.ASCII.GetBytes(Password.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = System.Text.Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch(Exception e)
            {
                //throw new Exception ("Error: Something went wrong on decryption!");
                CLog.LogWriteError("Core - Decryption error: " + e.ToString());
            }
            return DecryptedData;

        }

        /// <summary>
        /// Encryption function
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string _encryptData(string data)
        {
            string EncryptedData = string.Empty;
            try
            {
                string Password = _KEY;
                string InputText = data;
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
                byte[] Salt = System.Text.Encoding.ASCII.GetBytes(Password.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();
                byte[] CipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
               EncryptedData = Convert.ToBase64String(CipherBytes);
              
            }
            catch(Exception e)
            {
                CLog.LogWriteError("Core - Encryption error: " + e.ToString());
            }
            return EncryptedData;

        }
       
    }
}

using System;
using System.Security.Cryptography;
using System.Text;

namespace Notes.Models
{
    class Aes
    {
        private static RijndaelManaged rijndael = new RijndaelManaged();
        private static System.Text.UnicodeEncoding unicodeEncoding = new UnicodeEncoding();

        public static string SAMPLE_KEY = "gCjK+DZ/GCYbKIGiAt1qCA==";
        public static string SAMPLE_IV = "47l5QsSe1POo31adQ/u7nQ==";

        private const int CHUNK_SIZE = 128;

        private void InitializeRijndael()
        {
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;
        }

        //для случайных вхдных данныъ
        public Aes()
        {
            InitializeRijndael();

            rijndael.KeySize = CHUNK_SIZE;
            rijndael.BlockSize = CHUNK_SIZE;

            rijndael.GenerateKey();
            rijndael.GenerateIV();
        }

        public Aes(String base64key, String base64iv)
        {
            InitializeRijndael();

            rijndael.Key = Convert.FromBase64String(base64key);
            rijndael.IV = Convert.FromBase64String(base64iv);
        }

        public Aes(byte[] key, byte[] iv)
        {
            InitializeRijndael();

            rijndael.Key = key;
            rijndael.IV = iv;
        }

        public string Decrypt(byte[] cipher)
        {
            ICryptoTransform transform = rijndael.CreateDecryptor();
            byte[] decryptedValue = transform.TransformFinalBlock(cipher, 0, cipher.Length);
            return unicodeEncoding.GetString(decryptedValue);
        }

        public string DecryptFromBase64String(string base64cipher)
        {
            return Decrypt(Convert.FromBase64String(base64cipher));
        }

        public byte[] EncryptToByte(string plain)
        {
            ICryptoTransform encryptor = rijndael.CreateEncryptor();
            byte[] cipher = unicodeEncoding.GetBytes(plain);
            byte[] encryptedValue = encryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            return encryptedValue;
        }

        public string EncryptToBase64String(string plain)
        {
            return Convert.ToBase64String(EncryptToByte(plain));
        }

        public string GetKey()
        {
            return Convert.ToBase64String(rijndael.Key);
        }

        public string GetIV()
        {
            return Convert.ToBase64String(rijndael.IV);
        }

        public override string ToString()
        {
            return "KEY:" + GetKey() + Environment.NewLine + "IV:" + GetIV();
        }
    }

    //Пример использования
    /*
     private const string ORIGINAL = "this is some data to encrypt";//текст которых шифруеться
        private const string SAMPLE_KEY = "gCjK+DZ/GCYbKIGiAt1qCA==";
        private const string SAMPLE_IV = "47l5QsSe1POo31adQ/u7nQ==";

        static void Main(string[] args)
        {

            Aes aes = new Aes(SAMPLE_KEY, SAMPLE_IV);

            Console.WriteLine("ORIGINAL:" + ORIGINAL);
            Console.WriteLine("KEY:" + aes.GetKey());
            Console.WriteLine("IV:" + aes.GetIV());

            //string->byte->string
            Console.WriteLine("Example for: string->byte->string");
            byte[] encryptedBlock = aes.EncryptToByte(ORIGINAL);
            string decryptedString = aes.Decrypt(encryptedBlock);
            Console.WriteLine(decryptedString);

            //string->base64->string
            Console.WriteLine("Example for: string->base64->string");
            string encryptedBase64String = aes.EncryptToBase64String(ORIGINAL);
            decryptedString = aes.DecryptFromBase64String(encryptedBase64String);   

            Console.WriteLine(encryptedBase64String);
            Console.WriteLine(decryptedString);


            Console.ReadLine();
        }
    */
}
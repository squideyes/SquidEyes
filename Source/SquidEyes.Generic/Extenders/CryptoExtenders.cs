using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class CryptoExtenders
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        
        public const int MinSaltSize = 2;
        public const int MaxSaltSize = 4096;

        public static byte[] ToSHA512(this string plainText, byte[] saltBytes)
        {
            return Encoding.UTF8.GetBytes(plainText).ToSHA512(saltBytes);
        }

        public static byte[] ToSHA512(this byte[] plainTextBytes, byte[] saltBytes)
        {
            byte[] plainTextWithSaltBytes =
                new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            var hash = new SHA512Managed();

            return hash.ComputeHash(plainTextWithSaltBytes);
        }

        public static byte[] ToMD5(this string plainText)
        {
            return Encoding.UTF8.GetBytes(plainText).ToMD5();
        }

        public static byte[] ToMD5(this byte[] plainTextBytes)
        {
            return new MD5CryptoServiceProvider().ComputeHash(plainTextBytes);
        }

        public static byte[] GetSalt(int minSaltSize, int maxSaltSize)
        {
            Debug.Assert(minSaltSize >= MinSaltSize);
            Debug.Assert(maxSaltSize > minSaltSize);
            Debug.Assert(maxSaltSize <= MaxSaltSize);

            int saltSize = GetNextInt32(minSaltSize, maxSaltSize);

            byte[] bytes = new byte[saltSize];

            new RNGCryptoServiceProvider().GetNonZeroBytes(bytes);

            return bytes;
        }

        public static int GetNextInt32()
        {
            return GetNextInt32(int.MinValue, int.MaxValue);
        }

        public static int GetNextInt32(int minValue, int maxValue)
        {
            return (int)GetNextInt64(minValue, maxValue);
        }

        public static long GetNextInt64()
        {
            return GetNextInt64(long.MinValue, long.MaxValue);
        }

        public static long GetNextInt64(long minValue, long maxValue)
        {
            Contract.Requires(maxValue.InRange(minValue + 1, long.MaxValue));

            byte[] buffer = new byte[8];

            double number;

            rng.GetBytes(buffer);

            number = Math.Abs(BitConverter.ToDouble(buffer, 0));

            number = number - Math.Truncate(number);

            return (long)(number * ((double)maxValue - (double)minValue) + minValue);
        }

        public static double GetNextDouble()
        {
            return GetNextDouble(double.MinValue, double.MaxValue);
        }

        public static double GetNextDouble(double minValue, double maxValue)
        {
            Contract.Requires(maxValue.InRange(minValue + 1, long.MaxValue));

            byte[] buffer = new byte[8];

            double number;

            rng.GetBytes(buffer);

            number = Math.Abs(BitConverter.ToDouble(buffer, 0));

            number = number - Math.Truncate(number);

            return number * (maxValue - minValue) + minValue;
        }

        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            byte[] encryptedData;

            using (var stream = new MemoryStream())
            {
                var algorithm = Rijndael.Create();

                algorithm.Key = Key;

                algorithm.IV = IV;

                using (var cipherStream = new CryptoStream(
                    stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cipherStream.Write(clearData, 0, clearData.Length);
                }

                encryptedData = stream.ToArray();
            }

            return encryptedData;
        }

        public static string Encrypt(this string clearText, string password)
        {
            var clearBytes = Encoding.Unicode.GetBytes(clearText);

            var pdb = GetPasswordDerivedBytes(password);

            var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static byte[] Encrypt(this byte[] clearData, string password)
        {
            var pdb = GetPasswordDerivedBytes(password);

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            byte[] decryptedData;

            using (var stream = new MemoryStream())
            {
                var algorithm = Rijndael.Create();

                algorithm.Key = Key;

                algorithm.IV = IV;

                using (var cipherStream = new CryptoStream(
                    stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cipherStream.Write(cipherData, 0, cipherData.Length);
                }

                decryptedData = stream.ToArray();
            }

            return decryptedData;
        }

        public static string Decrypt(this string cipherText, string password)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);

            var pdb = GetPasswordDerivedBytes(password);

            var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Encoding.Unicode.GetString(decryptedData);
        }

        public static byte[] Decrypt(this byte[] cipherData, string password)
        {
            var pdb = GetPasswordDerivedBytes(password);

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        private static PasswordDeriveBytes GetPasswordDerivedBytes(string password)
        {
            return new PasswordDeriveBytes(password,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        }

        public static byte[] ToFNV1A(this string plainText)
        {
            return Encoding.UTF8.GetBytes(plainText).ToFNV1A();
        }

        public static byte[] ToFNV1A(this byte[] bytes)
        {
            ulong result = 0;

            if ((bytes != null) || (bytes.Length != 0))
            {
                unchecked
                {
                    result = 0xcbf29ce484222325u;

                    for (int i = 0; i < bytes.Length; i++)
                        result = ((result ^ bytes[i]) * 0x100000001b3);
                }
            }

            return BitConverter.GetBytes(result);
        }

        public static string ToBase32String(this byte[] bytes)
        {
            const string ValidChars = "QAZ2WSX3EDC4RFV5TGB6YHN7UJM8K9LP";

            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();

            byte index;
            int hi = 5;
            int currentByte = 0;

            while (currentByte < bytes.Length)
            {
                if (hi > 8)
                {
                    index = (byte)(bytes[currentByte++] >> (hi - 5));

                    if (currentByte != bytes.Length)
                        index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);

                    hi -= 3;
                }
                else if (hi == 8)
                {
                    index = (byte)(bytes[currentByte++] >> 3);
                    hi -= 3;
                }
                else
                {

                    index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);

                    hi += 5;
                }

                sb.Append(ValidChars[index]);
            }

            return sb.ToString();
        }

        public static byte[] ToCRC32(this string plainText)
        {
            return Encoding.UTF8.GetBytes(plainText).ToCRC32();
        }

        private static byte[] ToCRC32(this byte[] bytes)
        {
            return new CRC32Managed().ComputeHash(bytes);
        }
    }
}

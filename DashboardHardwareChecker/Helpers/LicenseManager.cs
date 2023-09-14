using DashboardHardwareChecker.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DashboardHardwareChecker.Helpers
{
    public static class LicenseManager
    {
        public static string LegacyLicenseFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClearDashboard_Projects",
                "license.txt");

        public static string LicenseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "UserSecrets", "License");
        public static string LicenseFileName = "license.txt";
        public static string LicenseFilePath = Path.Combine(LicenseFolderPath, LicenseFileName);

        private static Aes CreateCryptoProvider()
        {
            var cryptProvider = Aes.Create();
            cryptProvider.BlockSize = 128;
            cryptProvider.KeySize = 128;

            byte[] key =
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            };
            cryptProvider.Key = key;

            cryptProvider.IV = key;

            cryptProvider.Mode = CipherMode.CBC;
            cryptProvider.Padding = PaddingMode.PKCS7;
            return cryptProvider;
        }

       
        public static User DecryptLicenseFromFileToUser(string filePath)
        {
            try
            {
                var json = DecryptLicenseFromFile(filePath);

                return DecryptedJsonToUser(json);

            }
            catch (Exception)
            {
                return new User();
            }
        }

        public static string DecryptLicenseFromFile(string filePath)
        {
            try
            {
                var str = GetLicenseFromFile(filePath);

                return DecryptLicenseFromString(str);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetLicenseFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(LicenseFilePath) && filePath == LicenseFilePath)
                {
                    filePath = LegacyLicenseFilePath;
                }

                var str = File.ReadAllText(filePath);
                return str;
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static User DecryptedJsonToUser(string decryptedLicenseKey, bool isGenerator = false)
        {
            try
            {
                var licenseUser = JsonSerializer.Deserialize<User>(decryptedLicenseKey);
                return licenseUser ?? new User();
            }
            catch (Exception ex)
            {
                return new User();
            }
        }


        public static string DecryptLicenseFromString(string str, bool isGenerator = false)
        {
            try
            {
                var cryptProvider = CreateCryptoProvider();

                var transform = cryptProvider.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64String(str);
                var decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                var serialized = Encoding.ASCII.GetString(decryptedBytes);

                return serialized;

            }
            catch (Exception ex)
            {
                return "";
            }
        }


    }
}

using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace ms_evva_core.Security
{
    public class Encryption
    {
        private static readonly string Key = "EvvaCoreSecretKey123"; // Pode ser menor ou maior, será ajustado
        private static readonly string IV = "EvvaIV12"; // Pode ser menor ou maior, será ajustado

        private static byte[] GetKeyBytes(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length > 24)
                return keyBytes.Take(24).ToArray();
            if (keyBytes.Length < 24)
                return keyBytes.Concat(new byte[24 - keyBytes.Length]).ToArray();
            return keyBytes;
        }

        private static byte[] GetIVBytes(string iv)
        {
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            if (ivBytes.Length > 8)
                return ivBytes.Take(8).ToArray();
            if (ivBytes.Length < 8)
                return ivBytes.Concat(new byte[8 - ivBytes.Length]).ToArray();
            return ivBytes;
        }

        private static bool IsValidBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentException("Texto não pode ser nulo ou vazio");

                byte[] keyBytes = GetKeyBytes(Key);
                byte[] ivBytes = GetIVBytes(IV);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                using (var des = TripleDES.Create())
                {
                    des.Key = keyBytes;
                    des.IV = ivBytes;
                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;

                    using (var encryptor = des.CreateEncryptor())
                    {
                        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criptografar mensagem: {ex.Message}", ex);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    throw new ArgumentException("Texto criptografado não pode ser nulo ou vazio");

                if (!IsValidBase64(encryptedText))
                    throw new ArgumentException("Texto criptografado não é um Base64 válido");

                byte[] keyBytes = GetKeyBytes(Key);
                byte[] ivBytes = GetIVBytes(IV);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                using (var des = TripleDES.Create())
                {
                    des.Key = keyBytes;
                    des.IV = ivBytes;
                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;

                    using (var decryptor = des.CreateDecryptor())
                    {
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao descriptografar mensagem: {ex.Message}", ex);
            }
        }

        public static string GenerateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool ValidateHash(string input, string hash)
        {
            string computedHash = GenerateHash(input);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
} 
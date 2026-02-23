#nullable enable
using System;
using System.IO;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using MessagePack;
using Persistence.Common;
using Persistence.Interfaces;
using UnityEngine;

namespace Persistence.Repository
{
    public abstract class EncryptedOnPremiseRepository<T> :
        IReadRepository<T>,
        ICreateRepository<T> where T : new()
    {
        protected abstract string FilePath { get; }
        protected virtual string FileType => ".dat";
        
        private ISecureKeyGenerator _secureKeyGenerator;
        private string _path;
        private readonly byte[] _secureKey;
        private const int IV_SIZE = 16;

        protected EncryptedOnPremiseRepository()
        {
            _path = Path.Combine(Application.persistentDataPath, FilePath + FileType);
            _secureKeyGenerator = new SecureKeyGenerator();
            _secureKey = _secureKeyGenerator.GetSecureKey();
        }
        
        
        public async UniTask<T?> ReadAsync()
        {
            if (!File.Exists(_path))
            {
                return default;
            }

            await UniTask.SwitchToThreadPool();
            var data = await File.ReadAllBytesAsync(_path);
            var decrypted = await DecryptAsync(data, _secureKey);
            //var test = BitConverter.ToString(decrypted);
            return MessagePackSerializer.Deserialize<T>(decrypted);
        }

        public async UniTask CreateAsync(T data)
        {
            var tempPath = _path + ".tmp";
            var bytes = MessagePackSerializer.Serialize(data);

            await UniTask.SwitchToThreadPool();
            bytes = await EncryptAsync(bytes, _secureKey);

            try 
            {
                await File.WriteAllBytesAsync(tempPath, bytes);

                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                File.Move(tempPath, _path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Save Failed: {e.Message}");
            }
        }

        private async UniTask<byte[]> EncryptAsync(byte[] data, byte[] privateKey)
        {
            using var aes = Aes.Create();
            aes.Key = privateKey;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var msEncrypt = new MemoryStream();
            await msEncrypt.WriteAsync(aes.IV, 0, IV_SIZE);

            using (var csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                await csEncrypt.WriteAsync(data, 0, data.Length);
            }
                
            return msEncrypt.ToArray();
        }

        private static async UniTask<byte[]> DecryptAsync(byte[] cipherData, byte[] privateKey)
        {
            if (cipherData.Length < IV_SIZE)
                throw new ArgumentException("Invalid cipher data");

            using var aes = Aes.Create();
            var iv = new byte[IV_SIZE];
            Buffer.BlockCopy(cipherData, 0, iv, 0, IV_SIZE);
            
            aes.Key = privateKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(
                       ms, 
                       aes.CreateDecryptor(), 
                       CryptoStreamMode.Write))
            {
                await cs.WriteAsync(cipherData, IV_SIZE, cipherData.Length - IV_SIZE);
            }
                
            return ms.ToArray();
        }
    }
}
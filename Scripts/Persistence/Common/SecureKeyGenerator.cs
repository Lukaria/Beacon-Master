using System.Security.Cryptography;
using System.Text;
using Persistence.Interfaces;
using UnityEngine.Device;

namespace Persistence.Common
{
    public class SecureKeyGenerator : ISecureKeyGenerator
    {
        private byte[] _secureKey;
        
        public byte[] GetSecureKey()
        {
            if(_secureKey != null) return _secureKey;
            
            var layer1 = GetDeviceKey();
            var salt = GetObfuscatedSalt();
            _secureKey = new byte[32];
            for (var i = 0; i < 32; i++)
            {
                _secureKey[i] = (byte)(layer1[i] ^ salt[i % salt.Length]);
            }
            
            return _secureKey;
        }

        private static byte[] GetDeviceKey()
        {
            using var sha = SHA256.Create();
            var deviceId = SystemInfo.deviceUniqueIdentifier;
            var appId = Application.identifier;
            var gpu = SystemInfo.graphicsDeviceName;
        
            var combined = $"{deviceId}_{appId}_{gpu}";

            return sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
        }
        
        private static byte[] GetObfuscatedSalt()
        {
            var salt = new byte[16];
            var seed = 0x5F3759DF;
        
            for (var i = 0; i < 16; i++)
            {
                seed = seed * 1103515245 + 12345;
                salt[i] = (byte)((seed >> 16) & 0xFF);
            }
        
            return salt;
        }
    }
}
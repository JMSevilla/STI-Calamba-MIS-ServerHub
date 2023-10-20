using System.Security.Cryptography;

namespace sti_sys_backend.Utilization;

public class DeviceKeyGenerator
{
    public static string GenerateDeviceKey()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            string deviceKey = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            return deviceKey;
        }
    }
}
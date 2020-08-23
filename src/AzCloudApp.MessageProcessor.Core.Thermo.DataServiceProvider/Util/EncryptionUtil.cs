
namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public class EncryptionUtil
    {
        public static string Encrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool IsPasswordAMatch(string password, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedPassword);
        }
    }
}


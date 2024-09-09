namespace BaseApp.Core.Utils
{
    public class SecurityUtil
    {
        public static string Encrypt(string pro)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            return BCrypt.Net.BCrypt.HashPassword(pro, salt);
        }

        public static bool Verify(string? password, string? encryptStr)
        {
            if (password == null || encryptStr == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, encryptStr);
        }
    }
}

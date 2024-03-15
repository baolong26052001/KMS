using System.Security.Cryptography;
using System.Text;

namespace KMS.Tools
{
    public class Password
    {
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Hash the input password
            string hashedInputPassword = hashPassword(password);

            // Compare the hashed input password with the stored hashed password
            return hashedInputPassword == hashedPassword;
        }
    }
}

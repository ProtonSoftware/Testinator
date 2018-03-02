using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Testinator.Core
{
    /// <summary>
    /// Handles data hashing as well as saving it into binary file in app data folder
    /// </summary>
    public static class FileDataHasher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">The password to hash and save</param>
        public static void HashAndSaveString(string password)
        {
            // Data to hash
            var textToHash = Encoding.UTF8.GetBytes(password);

            // Generate additional entropy (will be used as the Initialization vector)
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            // Get hashed result
            var hashedText = ProtectedData.Protect(textToHash, entropy, DataProtectionScope.CurrentUser);

            // Create directory in appdata
            /*var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);*/

            // NOTE:
            // We presume that hashed text and entropy can be easily seen, as it requires to use ProtectedData class to decrypt that
            // So its not that easy to guess that PIN and we can allow that, as clients can't decrypt that in reasonable time

            // Write both data to the files
            File.WriteAllBytes("pass.dat", hashedText);
            File.WriteAllBytes("entropy.dat", entropy);
        }

        public static string ReadAndUnhashString()
        {
            // Get directory in appdata
            /*var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);*/

            // Get pass and entropy from files
            var hashedText = File.ReadAllBytes("pass.dat");
            var entropy = File.ReadAllBytes("entropy.dat");

            // Decrypt the password
            var password = ProtectedData.Unprotect(hashedText, entropy, DataProtectionScope.CurrentUser);

            // Return it as string
            return Encoding.UTF8.GetString(password);
        }        
    }
}

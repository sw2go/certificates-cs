using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (var cert = LoadCertificate(StoreLocation.CurrentUser, "3645b5343d2597053d724ce7daef67f920f72b94"))
            {
                // usecase "send data, only receiver can decrypt"

                var encrypted = Encrypt(cert, "hello world");           // with publicKey

                var decrypted = Decrypt(cert, encrypted);               // with privateKey

                // usecase "identity verification"

                var signature = Sign(cert, "hello world");              // with privateKey

                var valid = Verify(cert, "hello world", signature);     // with publicKey
            }
        }


        public static X509Certificate2 LoadCertificate(StoreLocation storeLocation, string thumbprint)
        {
            using(X509Store store = new X509Store(storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                // returns certificate from the "My" folder only
                return store.Certificates.FirstOrDefault(x => String.Compare(x.Thumbprint, thumbprint, true) == 0);
            }
        }

        private static string Encrypt(X509Certificate2 x509, string stringToEncrypt)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);

            var encryptedBytes = x509.GetRSAPublicKey().Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);

        }

        private static string Decrypt(X509Certificate2 x509, string stringToDecrypt)
        {
            byte[] encryptedBytes = Convert.FromBase64String(stringToDecrypt);

            var decryptedBytes = x509.GetRSAPrivateKey().Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return ASCIIEncoding.ASCII.GetString(decryptedBytes);
        }

        private static string Sign(X509Certificate2 x509, string data)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(data);

            var signatureBytes = x509.GetRSAPrivateKey().SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signatureBytes);
        }

        private static bool Verify(X509Certificate2 x509, string data, string signature)
        {
            byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(data);
            byte[] signatureBytes = Convert.FromBase64String(signature);

            return x509.GetRSAPublicKey().VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);          
        }
    }
}
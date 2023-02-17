using System.Security.Cryptography.X509Certificates;

namespace JWTAuthentication.Helper
{
    public interface ICertificateHelper
    {
        public bool ValidateCertificate(X509Certificate2 clientCertificate, string requestorHost);

        public bool IsCorrectCert(byte[] clientCertificate, string thumbprint, string cerPassw);
    }
}

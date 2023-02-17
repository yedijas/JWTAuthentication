using JWTAuthentication.Databases.Certificates;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

namespace JWTAuthentication.Helper
{
    public class CertificateHelper : ICertificateHelper
    {
        private ICertificateService _certificateService;
        private IOptions<CertificateOptions> _cerOptions;
        private List<CertificateInfo> _certificates;

        #region constructor
        public CertificateHelper(IOptions<CertificateOptions> cerOptions, ICertificateService cerService)
        {
            _cerOptions = cerOptions;
            _certificateService = cerService;
        }
        #endregion

        #region properties
        public IOptions<CertificateOptions> CerOptions
        {
            set
            {
                _cerOptions = value;
            }
        }
        #endregion

        public bool IsCorrectCert(byte[] clientCertificate, string thumbprint, string cerPassw)
        {
            bool retval = false;

            X509Certificate2 cer = new X509Certificate2(clientCertificate, cerPassw);
            if (cer.Thumbprint.Equals(thumbprint))
            {
                retval = true;
            }

            return retval;
        }

        public bool IsCertRegistered(X509Certificate2 cert)
        {
            if (_certificateService.GetByThumbprint(cert.Thumbprint) != null)
                return false;
            else
                return true;
        }

        public bool ValidateCertificate(X509Certificate2 clientCertificate, string requestorHost)
        {
            if (clientCertificate == null)
                return false;

            if (!clientCertificate.Verify()) // basic verification
                return false;

            if (DateTime.Compare(DateTime.Now, clientCertificate.NotBefore) < 0 
                || DateTime.Compare(DateTime.Now, clientCertificate.NotAfter) > 0) // certificate date is valid
                return false;

            if (clientCertificate.GetNameInfo(X509NameType.SimpleName, false).Equals(requestorHost)) // verify subject
                return false;

            if (!IsCertRegistered(clientCertificate)) // certificate thumbprint never registered to system
                return false;

            return true;
        }
    }
}

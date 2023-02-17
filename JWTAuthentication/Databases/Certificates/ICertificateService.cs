using JWTAuthentication.Models;

namespace JWTAuthentication.Databases.Certificates
{
    public interface ICertificateService
    {
        public int Insert(CertificateInfo entity);
        public bool Update(CertificateInfo entity);
        public bool DeleteByID(int certID);
        public int DeleteByAppID(string appID);
        public int DeleteByThumbprint(string thumbprint);
        public IEnumerable<CertificateInfo> GetAll();
        public CertificateInfo GetByID(int certID);
        public CertificateInfo GetByAppID(string appID);
        public CertificateInfo GetByThumbprint(string thumbprint);
    }
}

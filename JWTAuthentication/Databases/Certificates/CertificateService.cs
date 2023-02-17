using JWTAuthentication.Models;
using LiteDB;

namespace JWTAuthentication.Databases.Certificates
{
    public class CertificateService : ICertificateService, IDisposable
    {
        private LiteDatabase _myLiteDB;
        private bool disposedValue;

        public CertificateService(ILiteDbContext liteDbContext)
        {
            _myLiteDB = liteDbContext.Database;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _myLiteDB = null;
                    GC.Collect();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int Insert(CertificateInfo entity)
        {
            return _myLiteDB.GetCollection<CertificateInfo>
                ("CertificateInfo").Insert(entity);
        }

        public bool Update(CertificateInfo entity)
        {
            return _myLiteDB.GetCollection<CertificateInfo>
                ("CertificateInfo").Update(entity);
        }

        public bool DeleteByID(int certID)
        {
            return _myLiteDB.GetCollection
                <CertificateInfo>("CertificateInfo")
                .Delete(certID);
        }

        public int DeleteByAppID(string appID)
        {
            return _myLiteDB.GetCollection
                <CertificateInfo>("CertificateInfo")
                .DeleteMany(o => o.AppID.Equals(appID));
        }

        public int DeleteByThumbprint(string thumbprint)
        {
            return _myLiteDB.GetCollection
                <CertificateInfo>("CertificateInfo")
                .DeleteMany(o => o.Thumbprint.Equals(thumbprint));
        }

        public IEnumerable<CertificateInfo> GetAll()
        {
            var result = _myLiteDB.GetCollection<CertificateInfo>
                            ("CertificateInfo").FindAll();
            return result;
        }

        public CertificateInfo GetByID(int certID)
        {
            var result = _myLiteDB.GetCollection<CertificateInfo>
                ("CertificateInfo").Find(o =>
                o.ID == certID)
                .FirstOrDefault();
            return result;
        }

        public CertificateInfo GetByAppID(string appID)
        {
            var result = _myLiteDB.GetCollection<CertificateInfo>
                ("CertificateInfo").Find(o =>
                o.AppID.Equals(appID))
                .FirstOrDefault();
            return result;
        }

        public CertificateInfo GetByThumbprint(string thumbprint)
        {
            var result = _myLiteDB.GetCollection<CertificateInfo>
                ("CertificateInfo").Find(o =>
                o.Thumbprint.Equals(thumbprint))
                .FirstOrDefault();
            return result;
        }
    }
}

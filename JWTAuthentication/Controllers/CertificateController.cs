using JWTAuthentication.Databases.Audiences;
using JWTAuthentication.Databases.Certificates;
using JWTAuthentication.Databases.Tokens;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Helper;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<CertificateOptions> _options;
        private readonly ICertificateService _certService;
        private readonly ICertificateHelper _certHelper;

        public CertificateController(ILogger<UserController> logger, IOptions<CertificateOptions> options,
            ICertificateService certService, ICertificateHelper certHelper)
        {
            _logger = logger;
            _certService = certService;
            _certHelper = certHelper;
            _options = options;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCertificate([FromForm] string appID,
            [FromForm] string thumbprint, [FromForm] IFormFile cerFile, [FromForm] string cerPassw)
        {
            bool cerOK = false;
            using (var stream = new MemoryStream())
            {
                cerFile.CopyTo(stream);
                if (_certHelper.IsCorrectCert(stream.ToArray(), thumbprint, cerPassw))
                    cerOK = true;
            }

            if (cerOK)
            {
                string fullCerLocation = _options.Value.CertLocation + "/" + cerFile.FileName;
                var cerInfoToAdd = new CertificateInfo()
                {
                    CertLocation = fullCerLocation,
                    AppID = appID,
                    Key = cerPassw,
                    Thumbprint = thumbprint
                };
                if (_certService.Insert(cerInfoToAdd) != 0)
                {
                    using (var stream = System.IO.File.Create(fullCerLocation))
                    {
                        cerFile.CopyTo(stream);
                    }
                    return Ok("Certificate added to app " + appID + "!");
                }
                else
                {
                    return BadRequest();

                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

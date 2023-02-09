using JWTAuthentication.Databases.Audiences;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudienceController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<JWTOptions> _options;
        private readonly IAudienceService _audienceservice;

        public AudienceController(ILogger<UserController> logger, IOptions<JWTOptions> options, IAudienceService audienceservice)
        {
            _logger = logger;
            _options = options;
            _audienceservice = audienceservice;
        }

        [HttpGet]
        public IEnumerable<TokenAudience> Get()
        {
            return _audienceservice.GetAll();
        }

        [HttpGet("{audienceId}")]
        public ActionResult<TokenAudience> Get(int audienceId)
        {
            var result = _audienceservice.GetById(audienceId);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("{hostName}")]
        public ActionResult<TokenAudience> Get(string hostName)
        {
            var result = _audienceservice.GetByHostname(hostName);
            if (result != default)
                return Ok(result);
            else
                return NotFound();

        }

        [HttpGet("{applicationName}")]
        public ActionResult<TokenAudience> GetByAppName(string applicationName)
        {
            var result = _audienceservice.GetBySystemName(applicationName);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<TokenAudience> Insert([FromBody] TokenAudience entity)
        {
            var id = _audienceservice.Insert(entity);
            if (id != default)
                return CreatedAtAction("Get", _audienceservice.GetById(id));
            else
                return BadRequest();
        }

        [HttpPut]
        public ActionResult Update(TokenAudience entity)
        {
            var result = _audienceservice.Update(entity);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result = _audienceservice.Delete(id);
            if (result)
                return Ok();
            else
                return NotFound();
        }
    }
}

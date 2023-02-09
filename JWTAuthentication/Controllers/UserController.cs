using JWTAuthentication.Databases.Users;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var result = _userService.GetById(id);
            if (result != default)
                return Ok(_userService.GetById(id));
            else
                return NotFound();
        }

        [HttpGet("{userEmail}")]
        public ActionResult<User> Get(string userEmail)
        {
            var result = _userService.GetByEmail(userEmail);
            if (result != default)
                return Ok(_userService.GetByEmail(userEmail));
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<User> Insert([FromBody] User newUser)
        {
            newUser.CreatedAt = DateTime.Now;
            var id = _userService.Insert(newUser);
            if (id != default)
                return CreatedAtAction("Get", _userService.GetById(id));
            else
                return BadRequest();
        }

        [HttpPut]
        public ActionResult<User> Update(User newUser)
        {
            var result = _userService.Update(newUser);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int userId)
        {
            var result = _userService.Delete(userId);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{userEmail}")]
        public ActionResult<User> Delete(string userEmail)
        {
            var result = _userService.DeleteByEmail(userEmail);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost("{userName,password}")]
        public ActionResult Login(string userName, string password)
        {
            var singleUser = _userService.GetByUsername(userName);
            if (singleUser != null && singleUser.UserPassword.Equals(password)) 
                return Ok(); // should return token here
            else
                return BadRequest();
        }

    }
}

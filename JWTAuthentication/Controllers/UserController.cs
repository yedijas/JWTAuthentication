﻿using JWTAuthentication.Databases.Tokens;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Helper;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTAuthentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<JWTOptions> _options;
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenService _tokenService;

        public UserController(ILogger<UserController> logger, IOptions<JWTOptions> options, IUserService userService
            , ITokenService tokenService, ITokenHelper tokenHelper)
        {
            _logger = logger;
            _options = options;
            _userService = userService;
            _tokenHelper = tokenHelper;
            _tokenService = tokenService;
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
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("{userEmail}")]
        public ActionResult<User> Get(string userEmail)
        {
            var result = _userService.GetByEmail(userEmail);
            if (result != default)
                return Ok(result);
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
        public ActionResult Update(User newUser)
        {
            var result = _userService.Update(newUser);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int userId)
        {
            var result = _userService.Delete(userId);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete("{userEmail}")]
        public ActionResult<User> Delete(string userEmail)
        {
            var result = _userService.DeleteByEmail(userEmail);
            if (result > 0)
                return Ok();
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("{userName,password}")]
        public ActionResult Login(string userName, string password)
        {
            var singleUser = _userService.GetByUsername(userName);
            StringValues requestorHostName = "";
            Request.Headers.TryGetValue("Host", out requestorHostName);
            var tokenInfo = new TokenInfo(_options.Value.Issuer, _options.Value.Subject, requestorHostName, _options.Value.TokenLife, _options.Value.SecretKey);
            if (singleUser != null && singleUser.UserPassword.Equals(password))
            {
                string resultedToken = _tokenHelper.GenerateJSONWebToken(tokenInfo, singleUser);
                _tokenService.Insert(new Token { 
                    ActualToken= resultedToken,
                    CreatedDate = DateTime.Now,
                    IssuedFor = singleUser.UserName,
                    RequestorURL = requestorHostName,
                    IsValid = true
                });
                return Ok(resultedToken); // return token here
            }
            else
                return BadRequest();
        }

        [HttpPost("{userName}")]
        public ActionResult LogOut(string userName)
        {
            if (_tokenService.DeleteByUsername(userName) != default) // delete the preexisting token
            {
                // should update to other sites to delete their token storage too.
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}

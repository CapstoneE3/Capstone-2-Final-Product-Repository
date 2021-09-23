using System.Threading.Tasks;
using System;
using System.Text;
using System.Security.Cryptography;
using PantryBackEnd.Repositories;
using PantryBackEnd.HashingPassword;
using PantryBackEnd.JwtGenerator;
using Microsoft.AspNetCore.Http;
namespace PantryBackEnd.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using PantryBackEnd.Models;
    public class UserController : ControllerBase
    {
        private Guid guid = Guid.NewGuid();
        JwtService service;
        private IUserRepo userRepo;
        public UserController(IUserRepo context, JwtService service)
        {
            this.service = service;
            this.userRepo = context;
        }
        [Route("api/Users/Login")]
        [HttpGet]
        public IActionResult login([FromBody]LoginDt log)
        {
            Hash getPassword = new Hash();
            Account user = userRepo.GetByEmail(log.email);

            if(user == null||!getPassword.Verify(log.password,user.Password))
            {
                return BadRequest(new {message = "Invalid credentials"});
            }
            var jwt = service.Generator(user.AccId);
            Response.Cookies.Append("jwt",jwt, new CookieOptions{
                HttpOnly =true
            });
            return Ok( new {message = "Success"});
        }
        [Route("api/Users/Register")]
        [HttpPost]
        public ActionResult<Account> Register([FromBody] RegisterDt reg)
        {
            Hash getPassword = new Hash();
            string hashPassword = getPassword.HashPass(reg.password);
            using(SHA256 hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(reg.password);
                byte[] hashBytes = hash.ComputeHash(sourceBytes);
                hashPassword = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            Account user = new Account
            {
                AccId = guid,
                Email = reg.email,
                Password =hashPassword,
                Name = reg.name
            };
            //need fixing from database
            Account newlyReg =userRepo.Register(user);
            return Ok(newlyReg);
            /*
            Account user = new Account
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password, workFactor)
            }*/
        }
        [Route("api/GetUser")]
        [HttpGet]
        public ActionResult<Account> GetUser()
        {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                return Ok(user);
        }
    }
}
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
        [HttpPost]
        public IActionResult login([FromBody] LoginDt log)
        {
            Hash getPassword = new Hash();
            Account user = userRepo.GetByEmail(log.email);
            var jwt = service.Generator(user.AccId);
            var loggedin = service.Generator(guid = new Guid());
            if (Request.Cookies["jwt"] == null)
            {


                if (user == null || !getPassword.Verify(log.password, user.Password))
                {
                    return BadRequest(new { message = "Invalid credentials" });
                }
                Response.Cookies.Append("LoggedIn", "SuckOnMY", new CookieOptions
                {
                    Domain = "handypantry.azurewebsites.net",
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Today.AddMonths(1)
                });
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    Domain = "pantties.azurewebsites.net",
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Today.AddMonths(1)


                });
                return Ok(new { message = "Success" });
            }
            else
            {
                try
                {

                    var check = Request.Cookies["jwt"];
                    var token = service.Verification(check);
                    return Unauthorized(new { message = "Already logged in" });
                }
                catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
                {
                    Response.Cookies.Append("LoggedIn", "SuckOnMY", new CookieOptions
                    {
                        Domain = "handypantry.azurewebsites.net",
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTime.Today.AddMonths(1)
                    });
                    Response.Cookies.Append("jwt", jwt, new CookieOptions
                    {
                        Domain = "pantties.azurewebsites.net",
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTime.Today.AddMonths(1)
                    });
                    return Ok(new { message = "Success" });
                }
            }

        }
        [Route("api/Users/Register")]
        [HttpPost]
        public ActionResult<Account> Register([FromBody] RegisterDt reg)
        {
            try
            {
                Hash getPassword = new Hash();
                string hashPassword = getPassword.HashPass(reg.password);
                using (SHA256 hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(reg.password);
                    byte[] hashBytes = hash.ComputeHash(sourceBytes);
                    hashPassword = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }
                Account user = new Account
                {
                    AccId = guid,
                    Email = reg.email,
                    Password = hashPassword,
                    Firstname = reg.FirstName,
                    Lastname = reg.LastName
                };
                Account newReg = userRepo.Register(user);
                if (newReg != null)
                {
                    return Ok(new { message = "You have successfully registered" });
                }
                return Ok(new { message = "Cannot Register" });
            }
            catch (Exception)
            {
                return Ok(new { message = "Account already exist" });
            }
        }
        [Route("api/isLoggedIn")]
        [HttpGet]
        public ActionResult<bool> IsLoggedIn()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }
        [Route("api/GetUser")]
        [HttpGet]
        public ActionResult<Guid> GetUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                User ok = new User(userId);
                return Ok(ok);

            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("api/Users/Logout")]
        [HttpGet]
        public ActionResult logout()
        {
            try
            {
                Response.Cookies.Append("LoggedIn", "", new CookieOptions
                {
                    Domain = "handypantry.azurewebsites.net",
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Today.AddMonths(-1)
                });
                Response.Cookies.Append("jwt", "", new CookieOptions
                {
                    Domain = "pantties.azurewebsites.net",
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Today.AddMonths(-1)
                });
                return Ok(new { message = "Success" });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Failed" });
            }

        }

        [Route("api/Users/removeUser")]
        [HttpDelete]
        public ActionResult removeUser()
        {

            var jwt = Request.Cookies["jwt"];
            var token = service.Verification(jwt);
            Guid userId = Guid.Parse(token.Issuer);
            Account user = userRepo.GetByID(userId);

            string str = userRepo.removeUser(user);
            return Ok(new { message = "Success" });

        }
    }
}
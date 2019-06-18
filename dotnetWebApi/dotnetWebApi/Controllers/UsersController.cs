using dotnetWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotnetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private AppDataBaseContext _ctx;
        public UsersController(AppDataBaseContext ctx)
        {
            _ctx = ctx;
        }

        // GET /api/users    -info
        [Authorize]
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public User Get()
        {
            return _ctx.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
        }

        // POST api/users - Register user
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task Post(User user)
        {
            //user.Username is not null
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must send a username");
                return;
            }

            //user.Password is not null
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must send a password");
                return;
            }

            //user.Email is not null
            if (string.IsNullOrWhiteSpace(user.Email))
            { 
                Response.StatusCode = 400;
                await Response.WriteAsync("You must send a valid email address");
                return;
            }
            //user.Email is not valid
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var match = regex.Match(user.Email);
            if (!(match.Success && match.Length == user.Email.Length))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must send a valid email address");
                return;
            }

            //user.Email is already exist in db
            var anyUser = _ctx.Users.Any(p => string.Compare(p.Email, user.Email) == 0);
            if (anyUser)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("User with that email already exist");
                return;
            }

            user.Password = CalculateMD5Hash(user.Password);

            var email = user.Email;
            var password = user.Password;

            user.Balance = 500;

            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password. - " + password);
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(
                        AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256)
                    );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                id_token = encodedJwt,
                username = identity.Name
            };

            Response.ContentType = "application/json";
            await Response.WriteAsync(
                JsonConvert.SerializeObject(response, 
                new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = _ctx.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // if user is not found
            return null;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using dotnetWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dotnetWebApi.Controllers
{
    [Route("api/users/list")]
    [ApiController]
    
    public class UsersListController : ControllerBase
    {
        private AppDataBaseContext _ctx;
        public UsersListController(AppDataBaseContext ctx)
        {
            _ctx = ctx;
        }

        // POST /api/users/list 
        [Authorize]
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public List<User> Post(User searchUser)
        {
            return _ctx.Users.Where(x => searchUser.Username.Any(stringToCheck => x.Username.Contains(searchUser.Username))).ToList();
        }
    }
}
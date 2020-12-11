using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Database;

namespace WebApplication.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private UserDbContext userDb;

        public AccountController(UserDbContext userDb)
        {
            this.userDb = userDb;
        }
        
        [Route("sign-in")]
        public async Task<string> Login(string userName)
        {
            await Authenticate(userName);

            return userName;
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims,"ApplicationCookie",ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Route("Her")]
        [Authorize]
        public async Task<string> Her()
        {
            return "sasi";
        }
    }
}
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    [Route("api/v1/authenticate")]
    public class AuthenticateController : ControllerBase
    {
        private readonly Db db;

        public AuthenticateController(Db db)
        {
            this.db = db;
        }

        [HttpPost("sign-up")]
        public async Task SignUpUser(string email, string accountName, string password)
        {
            var user = await db.GetUserAsync(email);

            if (user == null)
            {
                await db.AddUserAsync(email, accountName, password);

                await db.SaveChangesAsync();

                user = await db.GetUserAsync(email);

                var account = db.GetAccount(user, accountName, password);

                await Authenticate(account.Id.ToString(), Role.User.ToString());
            }
        }

        [HttpGet("sign-in")]
        public async Task SignIn(string email, string accountName, string password)
        {
            var user = await db.GetUserAsync(email);

            if (user != null)
            {
                var account = db.GetAccount(user, accountName, password);

                if (account != null)
                {
                    await Authenticate(account.Id.ToString(), user.Role.ToString());
                }
            }
        }

        [Authorize]
        [HttpGet("sign-out")]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task Authenticate(string idAccount, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, idAccount),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
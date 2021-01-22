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
        private readonly IDb db;

        public AuthenticateController(IDb db)
        {
            this.db = db;
        }

        [HttpPost("sign-up")]
        public async Task SignUpAsync(string email, string accountName, string password)
        {
            var user = await db.GetUserAsync(email);

            if (user == null)
            {
                await db.AddUserAsync(email, accountName, password);

                user = await db.GetUserAsync(email);

                var account = db.GetAccount(user, accountName, password);

                await AuthenticateAsync(account.Id.ToString(), Role.User.ToString());
            }
        }

        [HttpGet("sign-in")]
        public async Task<bool> SignInAsync(string email, string accountName, string password)
        {
            var user = await db.GetUserAsync(email);

            if (user == null) return false;
            var account = db.GetAccount(user, accountName, password);

            if (account == null) return false;
            await AuthenticateAsync(account.Id.ToString(), user.Role.ToString());

            return true;
        }

        [Authorize]
        [HttpGet("sign-out")]
        public async Task SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task AuthenticateAsync(string idAccount, string role)
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
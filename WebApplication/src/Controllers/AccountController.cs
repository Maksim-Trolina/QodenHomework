using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly Db db;
        public AccountController(Db db)
        {
            this.db = db;
        }

        [HttpPost("sign-up/user")]
        public async Task SignUpUser(string email,string accountName,string password)
        {
            var user = await db.GetUserAsync(email);

            if (user == null)
            {
                await db.AddUserAsync(email,accountName,password);

                await db.SaveChangesAsync();

                user = await db.GetUserAsync(email);

                var account = db.GetAccount(user, accountName, password);

                await Authenticate(account.Id.ToString(),Role.User.ToString());
            }
        }

        [Authorize]
        [HttpPost("sign-up/account")]
        public async Task SignUpAccount(string accountName, string password)
        {
            var account = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            var user = account.User;

            var duplicate = db.GetAccount(user, accountName, password);

            if (duplicate == null)
            {
                await db.AddAccountAsync(user, accountName, password);

                await db.SaveChangesAsync();
            }
        }

        [Authorize]
        [HttpDelete("delete/account")]
        public async Task DeleteAccount(string accountName)
        {
            var currentAccount = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            await db.RemoveAccountAsync(accountName, currentAccount.UserId);

            await db.SaveChangesAsync();

            if (currentAccount.Name == accountName)
            {
                await SignOut();
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

        private async Task Authenticate(string idAccount,string role)
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
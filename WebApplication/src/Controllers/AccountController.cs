using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database;
using WebApplication.Database.Models;

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
        
        [Route("login")]
        public async Task Login(string userName,string password)
        {
            User user = await userDb.Users.FirstOrDefaultAsync(x => x.UserName == userName && 
                                                                    x.Password == password);

            if (user != null)
            {
                await Authenticate(user.Id);
                
            }
        }

        [Authorize]
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Route("register")]
        public async Task Register(string userName,string password)
        {
            User user = await userDb.Users.FirstOrDefaultAsync(x => x.UserName == userName && 
                                                                    x.Password == password);

            if (user == null)
            {
                user = new User{Id = Guid.NewGuid(), UserName = userName, Password = password, Role = "User"};
                
                userDb.Users.Add(user);

                await userDb.SaveChangesAsync();

                await Authenticate(user.Id);
            }
        }

        private async Task Authenticate(Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims,"ApplicationCookie",ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
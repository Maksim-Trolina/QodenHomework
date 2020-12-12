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
        public async Task<string> Login(string userName,string password)
        {
            User user = await userDb.Users.FirstOrDefaultAsync(x => x.UserName == userName && 
                                                                    x.Password == password);

            if (user != null)
            {
                await Authenticate(user.Id.ToString());

                return "Вы вошли в систему";
            }

            return "Вы не вошли в систему";

        }

        [Authorize]
        [Route("logout")]
        public async Task<string> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return "Вы вышли из системы";
        }
        
        [Route("register")]
        public async Task<string> Register(string userName,string password)
        {
            User user = await userDb.Users.FirstOrDefaultAsync(x => x.UserName == userName && 
                                                                    x.Password == password);

            if (user == null)
            {
                int id = await userDb.Users.MaxAsync(x => x.Id);
                user = new User{Id = ++id, UserName = userName, Password = password, Role = "User"};
                
                userDb.Users.Add(user);

                await userDb.SaveChangesAsync();

                await Authenticate(user.Id.ToString());

                return "Вы зарегистрировались в системе";
            }

            return "Вы не зарегистрировались в системе";
        }

        private async Task Authenticate(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims,"ApplicationCookie",ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
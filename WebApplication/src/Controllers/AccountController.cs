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
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api")]
    public class AccountController : ControllerBase
    {
        
        
        /*
        /*[Route("login")]
        public async Task<string> Login(string userName,string password)
        {
            User user = await userDb.GetUser(userName, password);

            if (user != null)
            {
                await Authenticate(user.Id.ToString());

                return "Вы вошли в систему";
                
            }

            return "Вы не вошли в систему";

        }#1#

        [Authorize]
        [Route("logout")]
        public async Task<string> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return "Вы вышли из системы";
        }
        
        /*[Route("register")]
        public async Task<string> Register(string userName,string password)
        {
            User user = await userDb.GetUser(userName, password);

            if (user == null)
            {
                /*int id = await userDb.GetLastId();#2#

                user = await userDb.AddUser(userName, password);

                await Authenticate(user.Id.ToString());

                return "Вы зарегистрировались в системе";
            }

            return "Вы не зарегистрировались в системе";
        }#1#

        [Authorize]
        [Route("register/account")]
        public async Task<string> RegisterAccount()
        {
            var account = await userDb.AddAccount(User.Identity.Name);

            return account.UserId.ToString();
        }

        [Authorize]
        [Route("account/put")]
        public async Task AddMoney(string currency, decimal value,string accountId)
        {
            await userDb.AddMoney(currency, value, accountId,User.Identity.Name);
        }

        [Authorize]
        [Route("currency/delete")]
        public async Task DeleteCurrency(string currency)
        {
            await userDb.DeleteCurrency(currency, User.Identity.Name);
        }

        [Authorize]
        [Route("currency/add")]
        public async Task AddCurrency(string currency, decimal coast)
        {
            await userDb.AddCurrency(currency, User.Identity.Name, coast);
        }

        [Authorize]
        [Route("money/transfer")]
        public async Task TransferMoney(string currency, decimal value, string accountId)
        {
            await userDb.TransferMoney(currency, value, accountId, User.Identity.Name);
        }

        [Authorize]
        [Route("set/commission/transfer/all")]
        public async Task SetCommissionTransfer(string currency,decimal commission)
        {
            await userDb.SetCommissionTransferAll(currency, commission, User.Identity.Name);
        }

        [Authorize]
        [Route("set/commission/transfer")]
        public async Task SetCommissionTransfer(string currency, decimal commission, string userId)
        {
            await userDb.SetCommissionTransferUser(currency, commission, userId, User.Identity.Name);
        }

        [Authorize]
        [Route("set/limit/transfer/all")]
        public async Task SetLimitTransfer(string currency, decimal limit)
        {
            await userDb.SetLimitTransferAll(currency, limit, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/limit/transfer")]
        public async Task SetLimitTransfer(string currency, decimal commission, string userId)
        {
            await userDb.SetLimitTransfer(currency, commission, userId, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/limit/input/all")]
        public async Task SetLimitInput(string currency, decimal limit)
        {
            await userDb.SetLimitInputAll(currency, limit, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/limit/input")]
        public async Task SetLimitInput(string currency, decimal commission, string userId)
        {
            await userDb.SetLimitInput(currency, commission, userId, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/limit/output/all")]
        public async Task SetLimitOutput(string currency, decimal limit)
        {
            await userDb.SetLimitOutputAll(currency, limit, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/limit/output")]
        public async Task SetLimitOutput(string currency, decimal commission, string userId)
        {
            await userDb.SetLimitOutput(currency, commission, userId, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/commission/input/all")]
        public async Task SetCommissionInput(string currency, decimal limit)
        {
            await userDb.SetCommissionInputAll(currency, limit, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/commission/input")]
        public async Task SetCommissionInput(string currency, decimal commission, string userId)
        {
            await userDb.SetCommissionInput(currency, commission, userId, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/commission/output/all")]
        public async Task SetCommissionOutput(string currency, decimal limit)
        {
            await userDb.SetCommissionOutputAll(currency, limit, User.Identity.Name);
        }
        
        [Authorize]
        [Route("set/commission/output")]
        public async Task SetCommissionOutput(string currency, decimal commission, string userId)
        {
            await userDb.SetCommissionOutput(currency, commission, userId, User.Identity.Name);
        }
        
        
        
        private async Task Authenticate(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims,"ApplicationCookie",ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }*/
    }
}
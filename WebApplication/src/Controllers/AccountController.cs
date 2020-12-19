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
using WebApplication.Helpers;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api")]
    public class AccountController : ControllerBase
    {

        private DatabaseService db;

        private FinancialService finService;

        public AccountController(DatabaseService db,FinancialService financialService)
        {
            this.db = db;

            finService = financialService;
        }
        
        [Route("login")]
        public async Task Login(string mail, string name, string password)
        {
            Account account = await db.GetAccount(mail, name, password);

            if (account != null)
            {
                await Authenticate(name);
            }
        }

        [Authorize]
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Route("register/user")]
        public async Task Register(string mail, string name, string password)
        {
            Account account = await db.GetAccount(name);

            if (account == null)
            {
                await db.AddUser(mail, name, password);

                await db.Save();

                await Authenticate(name);
            }
        }

        [Authorize]
        [Route("register/account")]
        public async Task Register(string name, string password)
        {
            Account account = await db.GetAccount(name);

            if (account == null)
            {
                string mail = await db.GetMail(User.Identity.Name);

                await db.AddAccount(mail, name, password);

                await db.Save();

                await Logout();
                
                await Authenticate(name);
            }
        }

        [Authorize]
        [Route("delete/account")]
        public async Task Delete(string name)
        {
            Role role = (Role)(await db.GetRole(User.Identity.Name));

            if (role == Role.Admin && User.Identity.Name!=name)
            {
                await db.DeleteAccount(name);

                await db.Save();
            }
            else
            {
                string mailCurrent = await db.GetMail(User.Identity.Name);

                string mailDelete = await db.GetMail(name);

                if (mailCurrent == mailDelete && User.Identity.Name!=name)
                {
                    await db.DeleteAccount(name);

                    await db.Save();
                }
            }
        }

        [Authorize]
        [Route("delete/user")]
        public async Task Delete()
        {
            string mail = await db.GetMail(User.Identity.Name);

            await db.DeleteUser(mail);

            await db.Save();

            await Logout();
        }
        
        [Authorize]
        [Route("input/money")]
        public async Task InputMoney(string name,string currencyName,decimal value)
        {
            CurrencyAccount currencyAccount = await db.GetCurrencyAccount(name, currencyName);

            if (currencyAccount != null)
            {
                string mail = await db.GetMail(name);
                
                CurrencyAll currencyAll = await db.GetCurrencyAll(currencyName);

                await finService.InputMoney(mail, value, currencyAccount,User.Identity.Name,currencyAll);

                await db.Save();
            }
        }

        [Authorize]
        [Route("output/money")]
        public async Task OutputMoney(string name, string currencyName, decimal value)
        {
            CurrencyAccount currencyAccount = await db.GetCurrencyAccount(name, currencyName);

            if (currencyAccount != null)
            {
                string mail = await db.GetMail(name);
                
                CurrencyAll currencyAll = await db.GetCurrencyAll(currencyName);

                await finService.OutputMoney(mail, value, currencyAccount, User.Identity.Name,currencyAll);

                await db.Save();
            }
        }

        [Authorize]
        [Route("transfer/money")]
        public async Task TransferMoney(string name, string currencyName, decimal value)
        {
            CurrencyAccount currencyAccount = await db.GetCurrencyAccount(name, currencyName);

            if (currencyAccount != null)
            {
                string mail = await db.GetMail(name);
                
                CurrencyAll currencyAll = await db.GetCurrencyAll(currencyName);

                await finService.TransferMoney(mail, value, currencyAccount, User.Identity.Name, currencyAll);

                await db.Save();
            }
        }

        [Authorize]
        [Route("add/currency")]
        public async Task AddCurrency(string currency)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.AddCurrencyAll(currency);

                await db.Save();
            }
        }

        [Authorize]
        [Route("delete/currency")]
        public async Task DeleteCurrency(string currency)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte)Role.Admin)
            {
                await db.DeleteCurrency(currency);

                await db.Save();
            }
        }

        [Authorize]
        [Route("change/input/commission")]
        public async Task ChangeInputCommission(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.InputCommission);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/input/limit")]
        public async Task ChangeInputLimit(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.InputLimit);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/input/min")]
        public async Task ChangeInputMin(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.MinInput);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/output/commission")]
        public async Task ChangeOutputCommission(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.OutputCommission);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/output/limit")]
        public async Task ChangeOutputLimit(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.OutputLimit);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/output/min")]
        public async Task ChangeOutputMin(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.MinOutput);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/transfer/commission")]
        public async Task ChangeTransferCommission(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.TransferCommission);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/transfer/limit")]
        public async Task ChangeTransferLimit(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.TransferLimit);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/transfer/min")]
        public async Task ChangeTransferMin(string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(currency, commission,CurrencyAllOption.MinTransfer);

                await db.Save();
            }
        }

        [Authorize]
        [Route("change/user/input/commission")]
        public async Task ChangeInputCommission(string mail, string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(mail, currency, commission, CurrencyAllOption.InputCommissionUser);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/user/output/commission")]
        public async Task ChangeOutputCommission(string mail, string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(mail, currency, commission, CurrencyAllOption.OutputCommissionUser);

                await db.Save();
            }
        }
        
        [Authorize]
        [Route("change/user/transfer/commission")]
        public async Task ChangeTransferCommission(string mail, string currency, decimal commission)
        {
            byte role = await db.GetRole(User.Identity.Name);

            if (role == (byte) Role.Admin)
            {
                await db.ChangeCurrencyAllOption(mail, currency, commission, CurrencyAllOption.TransferCommissionUser);

                await db.Save();
            }
        }
        

        /*
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
        */
        
        
        
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
using System.Threading.Tasks;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Services
{

    public class FinancialService
    {
        private DatabaseService db;

        public FinancialService(DatabaseService db)
        {
            this.db = db;
        }

        public async Task InputMoney(string mail, decimal value,CurrencyAccount currencyAccount,string name,CurrencyAll currencyAll)
        {
            byte role = await db.GetRole(name);
            if (role == (byte) Roles.User && currencyAll.MinInput<=value)
            {
                CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                currencyAccount.Count += value * (1 - currencyUser.InputCommision);
            }
            else
            {
                currencyAccount.Count += value;
            }
        }

        public async Task OutputMoney(string mail,decimal value,CurrencyAccount currencyAccount,string name,CurrencyAll currencyAll)
        {
            byte role = await db.GetRole(name);

            string mailCurrent = await db.GetMail(name);

            string mailAccount = await db.GetMail(currencyAccount.AccountName);

            if (role == (byte) Roles.User && currencyAccount.Count>=value && currencyAll.MinOutput<=value && mailAccount == mailCurrent)
            {
                CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                currencyAccount.Count -= value * (1 - currencyUser.OutputCommision);
            }
            else
            {
                currencyAccount.Count -= value;
            }
        }

        public async Task TransferMoney(string mail, decimal value, CurrencyAccount currencyAccount, string name,
            CurrencyAll currencyAll)
        {
            byte role = await db.GetRole(name);

            CurrencyAccount account = await db.GetCurrencyAccount(name, currencyAll.CurrencyName);

            if (role == (byte) Roles.User && account.Count >= value && currencyAll.MinTransfer <= value)
            {
                CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                currencyAccount.Count += value * (1 - currencyUser.TransferCommision);

                account.Count -= value;
            }
        }
    }
}
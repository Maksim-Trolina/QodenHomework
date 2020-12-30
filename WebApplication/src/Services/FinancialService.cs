/*using System;
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
            if (role == (byte) Role.User && currencyAll.MinInput<=value)
            {
                if (currencyAll.InputLimit >= value)
                {
                    CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                    currencyAccount.Count += value * (1 - currencyUser.InputCommision);
                }
                else
                {
                    await db.AddBigOperation(currencyAccount.AccountName, value, TypeOperation.Input,currencyAll.CurrencyName);

                    await db.Save();
                }
            }
            else
            {
                currencyAccount.Count += value;
            }
        }
        

        public async Task BigMoneyOperation(Guid id)
        {
            BigOperation bigOperation = await db.GetBigOperation(id);

            string mail = await db.GetMail(bigOperation.ToAccountName);

            CurrencyAccount currencyAccount =
                await db.GetCurrencyAccount(bigOperation.ToAccountName, bigOperation.Currency);

            CurrencyUser currencyUser = await db.GetCurrencyUser(mail, bigOperation.Currency);

            switch (bigOperation.TypeOperation)
            {
                case (byte)TypeOperation.Input:
                    currencyAccount.Count += bigOperation.Value * (1 - currencyUser.InputCommision);
                    break;
                case (byte)TypeOperation.Output:
                    currencyAccount.Count -= bigOperation.Value * (1 - currencyUser.OutputCommision);
                    break;
                case (byte)TypeOperation.Transfer:
                    CurrencyAccount currencyAccountFrom =
                        await db.GetCurrencyAccount(bigOperation.FromAccountName, bigOperation.Currency);
                    currencyAccountFrom.Count -= bigOperation.Value;
                    currencyAccount.Count += bigOperation.Value * (1 - currencyUser.TransferCommision);
                    break;
            }
            
            db.DeleteBigOperation(bigOperation);

            await db.Save();
        }

        public async Task OutputMoney(string mail,decimal value,CurrencyAccount currencyAccount,string name,CurrencyAll currencyAll)
        {
            byte role = await db.GetRole(name);

            string mailCurrent = await db.GetMail(name);

            string mailAccount = await db.GetMail(currencyAccount.AccountName);

            if (role == (byte) Role.User && currencyAccount.Count>=value && currencyAll.MinOutput<=value && mailAccount == mailCurrent)
            {
                if (value <= currencyAll.OutputLimit)
                {
                    CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                    currencyAccount.Count -= value * (1 - currencyUser.OutputCommision);
                }
                else
                {
                    await db.AddBigOperation(currencyAccount.AccountName, value, TypeOperation.Output,currencyAll.CurrencyName);

                    await db.Save();
                }
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

            if (role == (byte) Role.User && account.Count >= value && currencyAll.MinTransfer <= value)
            {
                if (currencyAll.TransferLimit >= value)
                {
                    CurrencyUser currencyUser = await db.GetCurrencyUser(mail, currencyAccount.CurrencyName);

                    currencyAccount.Count += value * (1 - currencyUser.TransferCommision);

                    account.Count -= value;
                }
                else
                {
                    await db.AddBigOperation(currencyAccount.AccountName, value, TypeOperation.Transfer,currencyAll.CurrencyName,account.AccountName);

                    await db.Save();
                }
            }
        }
    }
}*/
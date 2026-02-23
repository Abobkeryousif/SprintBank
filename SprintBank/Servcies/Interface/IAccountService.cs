using SprintBank.DTOs;
using SprintBank.Models;

namespace SprintBank.Servcies.Interface
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber , string Pin);
        IEnumerable<Account> GetAccounts();
        Account Create(Account accountDto, string Pin, string ConfirmPin);
        void Update(UpdateAccountDto accountDto , string Pin = null);
        void Delete(int id);    
        Account GetAccount(int id);
        Account GetByAccountNumber(string AccountNumber);
    }
}

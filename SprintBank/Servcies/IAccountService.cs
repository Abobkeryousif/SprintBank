using SprintBank.Models;

namespace SprintBank.Servcies
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber , string Pin);
        IEnumerable<Account> GetAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account , string Pin = null);
    }
}

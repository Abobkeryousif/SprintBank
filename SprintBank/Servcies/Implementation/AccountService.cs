using AutoMapper;
using SprintBank.Data;
using SprintBank.DTOs;
using SprintBank.Models;
using SprintBank.Servcies.Interface;
using System.Text;

namespace SprintBank.Servcies.Implementation
{
    public class AccountService : IAccountService   
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AccountService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Account Authenticate(string AccountNumber, string Pin)
        {
            var account = _dbContext.Accounts.Where(a => a.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;

            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;

            return account;

        }

        public Account Create(Account accountDto, string Pin, string ConfirmPin)
        {
            if (_dbContext.Accounts.Any(e => e.Email == accountDto.Email))
                throw new ArgumentException("this account already added!");


            if (!Pin.Equals(ConfirmPin))
                throw new ArgumentException("Pin does't match");

            byte[] pinHash, pinSalt;

            CreatePinHash(Pin, out pinHash, out pinSalt);

            accountDto.PinHash = pinHash;
            accountDto.PinSalt = pinSalt;

            var accountData = _mapper.Map<Account>(accountDto);
            _dbContext.Accounts.Add(accountData);
            _dbContext.SaveChanges();

            return accountData;

        }

        public void Delete(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
                throw new ArgumentException($"not found with id: {id}");
            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
        }

        public Account GetAccount(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(i=> i.Id == id);
            if (account == null)
                throw new ArgumentException($"not found with id: {id}");
            return account;
        }

        public IEnumerable<Account> GetAccounts()
        {
           return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.Accounts.Where(a => a.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                throw new ArgumentException("not found with this account number");
            return account;
        }

        public void Update(UpdateAccountDto accountDto, string Pin = null)
        {
            var updatedAccount = _dbContext.Accounts.FirstOrDefault(i=> i.Id == accountDto.id);
            if (updatedAccount == null)
                throw new ArgumentException("account does't exist");
            
            if(!string.IsNullOrWhiteSpace(accountDto.Email))
            {
                if (_dbContext.Accounts.Any(e => e.Email == accountDto.Email))
                    throw new ArgumentException("this email already exist");

                updatedAccount.Email = accountDto.Email;    
            }


            if (!string.IsNullOrWhiteSpace(accountDto.PhoneNumber))
            {
                if (_dbContext.Accounts.Any(p => p.PhoneNumber == accountDto.PhoneNumber))
                    throw new ArgumentException("this email already exist");

                updatedAccount.PhoneNumber= accountDto.PhoneNumber;
            }
                

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                updatedAccount.PinHash = pinHash;
                updatedAccount.PinSalt = pinSalt;
                
            }

            updatedAccount.DateLastUpdate = DateTime.Now;

            _dbContext.Accounts.Update(updatedAccount);
            _dbContext.SaveChanges();
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA3_512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin))
                throw new ArgumentNullException("Pin can't be empty");

            using (var hmac = new System.Security.Cryptography.HMACSHA3_512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != pinHash[i])
                        return false;
                }

                return true;

            }
        }
    }
}

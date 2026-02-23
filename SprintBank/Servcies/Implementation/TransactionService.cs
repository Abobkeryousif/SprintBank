using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SprintBank.Data;
using SprintBank.Models;
using SprintBank.Servcies.Interface;
using SprintBank.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SprintBank.Servcies.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private readonly AppSetting _setting;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService; 
        public TransactionService(ApplicationDbContext context, ILogger<TransactionService> logger, IOptions<AppSetting> setting, IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _setting = setting.Value;
            _ourBankSettlementAccount = _setting.OurBankSettlementAccount;
            _accountService = accountService;
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "transaction created successfly!";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _context.Transactions.Where(d=> d.TransactionDate == date).ToList();

            response.ResponseCode = "00";
            response.ResponseMessage = "transaction created successfly!";
            response.Data = transaction;

            return response;

        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();
            //we need to chack if user authenticate

            var authUser = _accountService.Authenticate(AccountNumber,TransactionPin);
            if (authUser == null)
                throw new ArgumentException("invalid Cerdentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == EntityState.Modified) && (_context.Entry(destinationAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = Enums.TransStatues.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction completed successfly";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = Enums.TransStatues.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction Failed";
                    response.Data = null;
                }

            }

            catch (Exception ex) 
            {
                _logger.LogError($"An ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = Enums.TransType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount; 
            transaction.TransactionDate = DateTime.Now;


            transaction.TransactionParticulars = $"NEW Transaction From Source Account => {JsonSerializer.Serialize(transaction.TransactionSourceAccount)} TO " +
                $"Destination Account => {JsonSerializer.Serialize(transaction.TransactionDestinationAccount)}" +
                $"ON Date => {transaction.TransactionDate}" +
                $"For Amount => {JsonSerializer.Serialize(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonSerializer.Serialize(transaction.TransactionType)}" +
                $"Transaction Status => {JsonSerializer.Serialize(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);   
            _context.SaveChanges();

            return response;
        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();
            //we need to chack if user authenticate

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null)
                throw new ArgumentException("invalid Cerdentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == EntityState.Modified) && (_context.Entry(destinationAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = Enums.TransStatues.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction completed successfly";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = Enums.TransStatues.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction Failed";
                    response.Data = null;
                }

            }

            catch (Exception ex)
            {
                _logger.LogError($"An ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = Enums.TransType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;


            transaction.TransactionParticulars = $"NEW Transaction From Source Account => {JsonSerializer.Serialize(transaction.TransactionSourceAccount)} TO " +
                $"Destination Account => {JsonSerializer.Serialize(transaction.TransactionDestinationAccount)}" +
                $"ON Date => {transaction.TransactionDate}" +
                $"For Amount => {JsonSerializer.Serialize(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonSerializer.Serialize(transaction.TransactionType)}" +
                $"Transaction Status => {JsonSerializer.Serialize(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;

            Transaction transaction = new Transaction();
            //we need to chack if user authenticate

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
                throw new ArgumentException("invalid Cerdentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_context.Entry(sourceAccount).State == EntityState.Modified) && (_context.Entry(destinationAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = Enums.TransStatues.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction completed successfly";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = Enums.TransStatues.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "transaction Failed";
                    response.Data = null;
                }

            }

            catch (Exception ex)
            {
                _logger.LogError($"An ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = Enums.TransType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;


            transaction.TransactionParticulars = $"NEW Transaction From Source Account => {JsonSerializer.Serialize(transaction.TransactionSourceAccount)} TO " +
                $"Destination Account => {JsonSerializer.Serialize(transaction.TransactionDestinationAccount)}" +
                $"ON Date => {transaction.TransactionDate}" +
                $"For Amount => {JsonSerializer.Serialize(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonSerializer.Serialize(transaction.TransactionType)}" +
                $"Transaction Status => {JsonSerializer.Serialize(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }
    }
}

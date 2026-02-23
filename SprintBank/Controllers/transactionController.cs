using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SprintBank.DTOs;
using SprintBank.Models;
using SprintBank.Servcies.Interface;
using System.Text.RegularExpressions;

namespace SprintBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;    
        
        public transactionController(ITransactionService transactionService , IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateTransaction(TransactionRequestDto transactionDto)
        {
           if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = _mapper.Map<Transaction>(transactionDto);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }

        [HttpPost("make-deposit")]
        public IActionResult MakeDeposit(string accountNumber , decimal Amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9}]\d{9}$"))
                return BadRequest("account number must be 10 digit");

            return Ok(_transactionService.MakeDeposit(accountNumber,Amount,transactionPin));
        }

        [HttpPost("make-withdrawal")]
        public IActionResult MakeWithdrawal(string accountNumber, decimal Amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9}]\d{9}$"))
                return BadRequest("account number must be 10 digit");

            return Ok(_transactionService.MakeWithdrawal(accountNumber, Amount, transactionPin));
        }

        [HttpPost("make-fund-transfer")]
        public IActionResult MakeFundTransfer(string fromAccount, string toAccount, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(fromAccount, @"^[0][1-9]\d{9}$|^[1-9}]\d{9}$") || !Regex.IsMatch(toAccount, @"^[0][1-9]\d{9}$|^[1-9}]\d{9}$"))
                return BadRequest("account number must be 10 digit");

            return Ok(_transactionService.MakeFundsTransfer(fromAccount,toAccount,amount,transactionPin));
        }
    }
}

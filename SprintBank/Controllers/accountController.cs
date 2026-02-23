using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SprintBank.DTOs;
using SprintBank.Models;
using SprintBank.Servcies.Interface;
using System.Text.RegularExpressions;
namespace SprintBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class accountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public accountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public IActionResult RegisterAccount(AccountDto accountDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = _mapper.Map<Account>(accountDto);
            return Ok(_accountService.Create(account,accountDto.Pin,accountDto.ConfirmPin));
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            var account = _accountService.GetAccounts();
            var mappingAccountList = _mapper.Map<IList<GetAccountDto>>(account);
            return Ok(mappingAccountList);
        }

        [HttpPost("authentication")]
        public IActionResult Authenticate(AuthenticationModel model)
        {
            if(!ModelState.IsValid) 
                return BadRequest("account number or pin does't match");

            return Ok(_accountService.Authenticate(model.AccountNumber , model.Pin));
        }

        [HttpGet("get-account-by-accountnumber")]
        public IActionResult GetByAccountNumber(string accountNumber)
        {
            if(!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9}]\d{9}$")) 
                return BadRequest("account number must be 10 digit");

            var account = _accountService.GetByAccountNumber(accountNumber);
            var cleanedAccount = _mapper.Map<GetAccountDto>(account);
            return Ok(cleanedAccount);
        }

        [HttpGet("get-account-by-id")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accountService.GetAccount(id);
            var cleanedAccount = _mapper.Map<GetAccountDto>(account);
            return Ok(cleanedAccount);
        }

        [HttpPut("update-account")]
        public IActionResult UpdateAccount(UpdateAccountDto update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = _mapper.Map<Account>(update);
             _accountService.Update(update, update.Pin);
            return Ok($"Data updated Successfly {account.AccountName}");

        }
    }
}

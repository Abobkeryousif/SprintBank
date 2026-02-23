using AutoMapper;
using SprintBank.DTOs;
using SprintBank.Models;

namespace SprintBank.Mapping
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();
            CreateMap<Account, GetAccountDto>().ReverseMap();
        }
    }
}

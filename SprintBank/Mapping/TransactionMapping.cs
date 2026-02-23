using AutoMapper;
using SprintBank.DTOs;
using SprintBank.Models;

namespace SprintBank.Mapping
{
    public class TransactionMapping : Profile
    {
        public TransactionMapping()
        {
            CreateMap<Transaction, TransactionRequestDto>().ReverseMap();
        }
    }
}

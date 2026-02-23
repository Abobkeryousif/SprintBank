using SprintBank.Enums;
using System.ComponentModel.DataAnnotations;

namespace SprintBank.DTOs
{
    public class AccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName => $"{FirstName} {LastName}";
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }

        //public string AccountNumberGenerated { get; set; } 

        //public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateLastUpdate { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$",ErrorMessage = "Pin must be equles 4 digit")]
        //to generate number from 0 to 9 and just 4 digit
        public string Pin {  get; set; }

        [Required]
        [Compare("Pin",ErrorMessage = "Confirm Pin Does't match Pin")]
        public string ConfirmPin { get; set; }
    }

    public record UpdateAccountDto
    {
        public int id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateLastUpdate { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be equles 4 digit")]
        //to generate number from 0 to 9 and just 4 digit
        public string Pin { get; set; }

        [Required]
        [Compare("Pin", ErrorMessage = "Confirm Pin Does't match Pin")]
        public string ConfirmPin { get; set; }

    }

    public record GetAccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName => $"{FirstName} {LastName}";
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }

        public string AccountNumberGenerated { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }

}

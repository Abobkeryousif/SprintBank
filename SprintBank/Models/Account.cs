using SprintBank.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintBank.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName => $"{FirstName} {LastName}";
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; } // we will generated number soon

        // we will store hash and slat of account transaction pin
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateLastUpdate { get; set; }

        Random random = new Random();

        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long) random.NextDouble() * 9_000_000_000L + 1_000_000_000L); // here we will get 10-digit random number
        }
    }
}

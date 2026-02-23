using SprintBank.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintBank.DTOs
{
    public class TransactionRequestDto
    {
        public int Id { get; set; }
        public decimal TransactionAmount { get; set; }

        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }

        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}

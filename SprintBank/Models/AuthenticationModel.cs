using System.ComponentModel.DataAnnotations;

namespace SprintBank.Models
{
    public class AuthenticationModel
    {
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9}]\d{9}$")]
        public required string AccountNumber { get; set; }
        public required string Pin { get; set; }
    }
}

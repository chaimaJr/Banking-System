using AccountService.Models;
using System.ComponentModel.DataAnnotations;

namespace AccountService.DTOs
{
    public class CreateAccountDto
    {
        [Required] public AccountType AccountType { get; set; }


    }
}

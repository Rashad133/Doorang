
using System.ComponentModel.DataAnnotations;

namespace Doorang.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }


        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Surname { get; set; }


        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Username { get; set; }


        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        [MinLength(3)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}

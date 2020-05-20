
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDTO
    {
         [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength =4 ,ErrorMessage="You must Specify Password between 4 and 8") ]
        public string Password { get; set; }

    }
}
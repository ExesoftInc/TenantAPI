using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenantAPI.Models
{
    public class RegisterModel : LoginModel
    {
        public int TitleId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name can't be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name can't be longer than 50 characters.")]
        public string LastName { get; set; }


        [Phone]
        //[StringLength(50, MinimumLength = 10, ErrorMessage = "Phone length needs to be at least 10 characters.")]
        public string PhoneNumber { get; set; }
    }
}

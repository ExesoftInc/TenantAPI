using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenantAPI.Models
{
    public class RegisterByProviderModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string TenantName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int? TitleId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name can't be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name can't be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Phone length needs to be at least 10 characters.")]
        public string PhoneNumber { get; set; }
    }
}

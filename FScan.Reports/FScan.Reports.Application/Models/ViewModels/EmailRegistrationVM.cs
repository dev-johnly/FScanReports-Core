using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.ViewModels
{
    public class EmailRegistrationVM
    {
        public string Usercode { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [Compare(nameof(Email))]
        public string ConfirmEmail { get; set; }
    }
}

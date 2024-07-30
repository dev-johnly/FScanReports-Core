using FScan.Reports.Application.Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.ViewModels
{
    public class ChangePasswordVM
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Old password is required.")]
        [RegularExpression(@"^(?!.*[ ])(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[\W]).{8,20}$",
            ErrorMessage = "Passwords must be at least 8 - 20 characters and contain of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?!.*[ ])(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[\W]).{8,20}$",
           ErrorMessage = "Passwords must be at least 8 - 20 characters and contain of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(nameof(NewPassword))]
        [RegularExpression(@"^(?!.*[ ])(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[\W]).{8,20}$",
            ErrorMessage = "Passwords must be at least 8 - 20 characters and contain of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string ConfirmPassword { get; set;} = string.Empty;
    }
}

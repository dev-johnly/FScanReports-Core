using FScan.Reports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public int RequiredLength { get; set; } = 8;
        public int RequiredUniqueChars { get; set; } = 4;
        public bool RequireDigit { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string password = value.ToString();
                if (password.Length < RequiredLength)
                {
                    return new ValidationResult($"Password must be at least {RequiredLength} characters long.");
                }

                // Check for required unique characters
                if (password.Distinct().Count() < RequiredUniqueChars)
                {
                    return new ValidationResult($"Password must contain at least {RequiredUniqueChars} unique characters.");
                }

                // Check for required digit
                if (RequireDigit && !password.Any(char.IsDigit))
                {
                    return new ValidationResult("Password must contain at least one digit.");
                }

                // Check for required lowercase
                if (RequireLowercase && !password.Any(char.IsLower))
                {
                    return new ValidationResult("Password must contain at least one lowercase letter.");
                }

                // Check for required non-alphanumeric
                if (RequireNonAlphanumeric && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    return new ValidationResult("Password must contain at least one non-alphanumeric character.");
                }

                // Check for required uppercase
                if (RequireUppercase && !password.Any(char.IsUpper))
                {
                    return new ValidationResult("Password must contain at least one uppercase letter.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.ViewModels;

public class ForgotPasswordVM
{
    [Required(ErrorMessage = "UserID is required.")]
    public string Usercode { get; set; }
}

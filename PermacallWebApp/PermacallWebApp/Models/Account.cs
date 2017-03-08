using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Please insert Username")]
        [StringLength(50, ErrorMessage = "Username should be between 3 and 50 characters", MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please insert Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string CheckPassword { get; set; }

        public string ErrorMessage { get; set; }
        public string RedirectPage { get; set; }

    }
}
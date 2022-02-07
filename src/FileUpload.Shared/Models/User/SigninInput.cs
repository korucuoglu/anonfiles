﻿using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Models.User
{
    public class SigninInput
    {
        [Required]
        [Display(Name = "Email adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

    }
}
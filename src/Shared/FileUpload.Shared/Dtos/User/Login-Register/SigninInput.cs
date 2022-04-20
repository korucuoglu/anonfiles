﻿using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Dtos.User
{
    public class SigninInput
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}

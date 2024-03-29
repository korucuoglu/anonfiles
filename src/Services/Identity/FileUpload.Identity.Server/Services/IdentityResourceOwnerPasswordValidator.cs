﻿using FileUpload.Data.Entity;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        private readonly UserManager<User> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);

            if (user == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, context.Password);

            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }

            if (!user.EmailConfirmed)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Lüften mail adresinizi onaylayın." });
                context.Result.CustomResponse = errors;
                return;
            }



            context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}

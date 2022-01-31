﻿using AnonFilesUpload.Shared.Models.User;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SigninInput signinInput);
        Task<bool> SignUp(SignupInput signupInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}

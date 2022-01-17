// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AnonFilesUpload.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources = new ApiResource[]
        {

                new ApiResource("resource_api"){Scopes = {"api"}}

                // [2] Daha sonra Resource oluşturarak buna ait scope tanımladık. 
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
         {

         };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api", "AnonFilesUpload.Api'ya full erişim iznidir."),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),

                // [1] İlk olarak upload-api adında bir scope tanımlaması yaptık. 
              
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "VueJsClient",
                    ClientName = "VueJS Client Credentials Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api", IdentityServerConstants.LocalApi.ScopeName }
                },
            };
    }
}
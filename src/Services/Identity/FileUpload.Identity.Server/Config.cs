﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FileUpload.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
                new ApiScope("upload_fullpermission", "Upload Api için full erişim iznidir."),
                new ApiScope("gateway_fullpermission", "Gateway Api için full erişim iznidir."),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),

                // [1] İlk olarak upload-api adında bir scope tanımlaması yaptık. 
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
                new ApiResource("resource_upload"){Scopes = {"upload_fullpermission"}},
                new ApiResource("resource_gateway"){Scopes = {"gateway_fullpermission"}}

                // [2] Daha sonra Resource oluşturarak buna ait scope tanımladık. 
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
        };

        public static IEnumerable<Client> Clients => new Client[]
        {
                new Client
                {
                    ClientId = "MVCClient",
                    ClientName = "MVC Client Credentials Client",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },

                 new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={ "gateway_fullpermission", "upload_fullpermission", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles" },
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime= (int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds,
                    RefreshTokenUsage= TokenUsage.OneTimeOnly
                },
                 new Client
                {
                    ClientId = "vue",
                    ClientName = "Vue Client",
                    RequireClientSecret = false,
                    // AllowAccessTokensViaBrowser =true,
                    AllowedScopes = {
                       "gateway_fullpermission",
                       "upload_fullpermission",
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "Roles"
                    },
                    RedirectUris = {"http://localhost:8080"},
                    AllowedCorsOrigins = {"http://localhost:8080"},
                    PostLogoutRedirectUris = {"http://localhost:8080"},
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                }

        };
    }
}
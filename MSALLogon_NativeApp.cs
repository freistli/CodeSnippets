 private MsalAuthenticationProvider CreatePubliclAuthorizationProvider(List<string> scopes)
        {
            var clientId = ViewModelLocator.SettingsPageInstance.EnterpriseConfig.clientId;
            //var clientSecret = ViewModelLocator.SettingsPageInstance.EnterpriseConfig.clientSecret;
            var redirectUri = ViewModelLocator.SettingsPageInstance.EnterpriseConfig.redirectUri;
            var authority = $"https://login.microsoftonline.com/{ViewModelLocator.SettingsPageInstance.EnterpriseConfig.tenantId}/v2.0";

            var cca = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(authority)
                                                    .WithRedirectUri(redirectUri)
                                                    .Build();
            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
        private GraphServiceClient GetPublicAuthenticatedGraphClient(List<string> scopes)
        {
             authenticationProvider = CreatePubliclAuthorizationProvider(scopes);
            ((MsalAuthenticationProvider)authenticationProvider).Interactive = true;
            _graphClient = new GraphServiceClient(authenticationProvider);
            return _graphClient;
        }

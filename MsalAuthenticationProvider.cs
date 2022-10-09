public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private IConfidentialClientApplication _clientApplication;

        private IPublicClientApplication _publicClientApplication;

        private string[] _scopes;

        public bool Interactive = false;

        public MsalAuthenticationProvider(IConfidentialClientApplication clientApplication, string[] scopes)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }

        public MsalAuthenticationProvider(IPublicClientApplication clientApplication, string[] scopes)
        {
            _publicClientApplication = clientApplication;
            _scopes = scopes;
        }


        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            string token;
            if (Interactive)
                token = await GetPublicTokenAsync();
            else
                token = await GetTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult authResult = null;
            authResult = await _clientApplication.AcquireTokenForClient(_scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
        //Only for the app
        public async Task ClearPublicTokenAsync()
        {
            if (_publicClientApplication != null)
            {
                // clear the cache
                var accounts = await _publicClientApplication.GetAccountsAsync();
                while (accounts.Any())
                {
                    await _publicClientApplication.RemoveAsync(accounts.First());
                    accounts = await _publicClientApplication.GetAccountsAsync();
                }
            }
        }
        public async Task<string> GetPublicTokenAsync()
        {
            AuthenticationResult authResult = null;


            var accounts = await _publicClientApplication.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();
            
            if (firstAccount != null)
            {
                authResult = await _publicClientApplication.AcquireTokenSilent(_scopes, firstAccount).ExecuteAsync();
                return authResult.AccessToken;
            }
            authResult = await _publicClientApplication.AcquireTokenInteractive(_scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
    }

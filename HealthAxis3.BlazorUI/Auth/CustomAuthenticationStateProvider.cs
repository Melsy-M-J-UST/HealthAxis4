using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxis3.BlazorUI.Auth
{
    public class CustomAuthenticationStateProvider(IJSRuntime js) : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        private readonly IJSRuntime _js = js;

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentUser.Identity!.IsAuthenticated)
            {
                return new AuthenticationState(_currentUser);
            }
            else
            {
                var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                _currentUser = new ClaimsPrincipal(identity);
                return new AuthenticationState(_currentUser);
            }
        }
        private static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var parts = jwt.Split(".");
            if (parts.Length < 2)
            {
                return claims;
            }
            var payload = parts[1];
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);
            if (keyValuePairs == null)
            {
                return claims;
            }
            foreach (var keyvaluepair in keyValuePairs)
            {
                if (keyvaluepair.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in keyvaluepair.Value.EnumerateArray())
                    {
                        claims.Add(new Claim(keyvaluepair.Key, element.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(keyvaluepair.Key, keyvaluepair.Value.ToString()));
                }
            }
            return claims;
        }
        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
        public void NotifyUserLoggedOut()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}
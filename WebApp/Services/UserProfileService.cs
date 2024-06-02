namespace eShop.WebApp.Services;

using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public class UserProfileService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
{
    private readonly string userProfileApiUrl = "/api/userProfile";
    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    public async Task<UserProfile> GetUserProfileAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        authState.User.Claims.ToList().ForEach(claim => Console.WriteLine($"{claim.Type}: {claim.Value}"));
        var user = authState.User;

        return new UserProfile
        {
            Name = ReadClaim("name", user),
            LastName = ReadClaim("last_name", user),
            Email = ReadClaim("email", user),
            EmailVerified = ReadClaim("email_verified", user),
        };
    }

    private static string? ReadClaim(string type, System.Security.Claims.ClaimsPrincipal? user) => user.FindFirst(x => x.Type == type)?.Value;

    /*public Task<HttpResponseMessage> UpdateProfile(UserProfile userProfile)
    {
        *//*await httpClient.PutAsJsonAsync(userProfileApiUrl, userProfile);*//*
        httpClient.ToString();
        Console.WriteLine(userProfileApiUrl + userProfile);
        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
    }*/

    // Use the AccountController to update the user profile
    public Task<HttpResponseMessage> UpdateProfile(UserProfile userProfile)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, userProfileApiUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(userProfile), Encoding.UTF8, "application/json")
        };

        return httpClient.SendAsync(request);
    }

    public async Task<string?> GetBuyerIdAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("sub")?.Value;
    }

    public async Task<string?> GetUserNameAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("name")?.Value;
    }

    public async Task<ClaimsPrincipal> GetUserAsync()
        => (await authenticationStateProvider.GetAuthenticationStateAsync()).User;

    public class UserProfile
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? EmailVerified { get; set; }
    }
}

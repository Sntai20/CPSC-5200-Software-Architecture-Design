namespace eShop.WebApp.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public class UserProfileService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
{
    private readonly string userProfileApiUrl = "/api/userProfile";
    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    public async Task<UserProfile> GetUserProfile()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
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

    public Task<HttpResponseMessage> UpdateProfile(UserProfile userProfile)
    {
        /*await httpClient.PutAsJsonAsync(userProfileApiUrl, userProfile);*/
        httpClient.ToString();
        Console.WriteLine(userProfileApiUrl + userProfile);
        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
    }

    public class UserProfile
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? EmailVerified { get; set; }
    }
}

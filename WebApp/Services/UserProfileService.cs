namespace eShop.WebApp.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public class UserProfileService(AuthenticationStateProvider authenticationStateProvider)
{
    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    public Task<UserProfile> GetProfileData(HttpContext httpContext)
    {
        var user = httpContext.User;

        return Task.FromResult(new UserProfile
        {
            UserId = ReadClaim(ClaimTypes.NameIdentifier, user),
            Name = ReadClaim("name", user),
            LastName = ReadClaim("last_name", user),
            Email = ReadClaim(ClaimTypes.Email, user)
        });
    }

    public async Task<UserProfile> UpdateProfileData(HttpContext httpContext, UserProfile updatedProfile)
    {
        // Access the existing profile data
        var user = httpContext.User;
        var userId = ReadClaim(ClaimTypes.NameIdentifier, user);
        var userName = ReadClaim("name", user);
        var lastName = ReadClaim("last_name", user);
        var email = ReadClaim(ClaimTypes.Email, user);

        // Update the profile data with the provided values
        if (updatedProfile.Name != null)
        {
            userName = updatedProfile.Name;
        }
        if (updatedProfile.LastName != null)
        {
            lastName = updatedProfile.LastName;
        }
        if (updatedProfile.Email != null)
        {
            email = updatedProfile.Email;
        }

        // Perform further operations with the updated profile data
        // For example, you can save the updated profile data to a database

        // Update the user claims with the updated profile data
        var identity = (ClaimsIdentity)user.Identity;
        identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
        identity.AddClaim(new Claim(ClaimTypes.Name, userName ?? string.Empty));
        identity.RemoveClaim(identity.FindFirst(ClaimTypes.Surname));
        identity.AddClaim(new Claim(ClaimTypes.Surname, lastName ?? string.Empty));
        identity.RemoveClaim(identity.FindFirst(ClaimTypes.Email));
        identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

        // save the updated profile data to the database

        // Update the authentication state
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

        // You can also update the profile data in other external systems or APIs

        // Optionally, you can return the updated profile data
        return new UserProfile
        {
            UserId = userId,
            Name = userName,
            LastName = lastName,
            Email = email
        };
    }

    public async Task<UserProfile> GetUserProfileAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        return new UserProfile
        {
            Name = ReadClaim("name", user),
            LastName = ReadClaim("last_name", user),
            Email = ReadClaim("email", user),
        };
    }

    private static string? ReadClaim(string type, System.Security.Claims.ClaimsPrincipal? user) => user.FindFirst(x => x.Type == type)?.Value;

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
        public string? UserId { get; set; }
    }
}

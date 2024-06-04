namespace eShop.Identity.API.Models.AccountViewModels;

public record RegisterViewModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; init; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; init; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; init; }

    public string CardNumber { get; set; }
    public string SecurityNumber { get; set; }
    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    public string Expiration { get; set; }
    public string CardHolderName { get; set; }
    public int CardType { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string ReturnUrl { get; set; }

    public ApplicationUser User { get; init; }
}

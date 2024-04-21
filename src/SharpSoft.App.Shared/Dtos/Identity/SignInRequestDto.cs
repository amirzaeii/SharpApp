
namespace SharpSoft.App.Shared.Dtos.Identity;

[DtoResourceType(typeof(AppStrings))]
public class SignInRequestDto
{
    /// <example>test@bitplatform.dev</example>
    //[Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [EmailAddress(ErrorMessage = nameof(AppStrings.EmailAddressAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Email))]
    public string? UserName { get; set; }

    /// <example>123456</example>
    //[Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Password))]
    public string? Password { get; set; }

    [JsonIgnore]
    [Display(Name = nameof(AppStrings.RememberMe))]
    public bool RememberMe { get; set; } = true;

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Code))]
    public string? Code { get; set; }

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Url(ErrorMessage = nameof(AppStrings.ServerAddressAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.ServerAddress))]
    public string? ServerAddress { get; set; } = "http://";
}

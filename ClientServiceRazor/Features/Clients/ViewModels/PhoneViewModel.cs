using System.ComponentModel.DataAnnotations;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.Services;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class PhoneViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(50)]
    public string Number { get; set; } = null!;
    
    public CountryCode CountryCode { get; set; } = CountryCode.UA;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Number))
        {
            yield return new ValidationResult("Phone number is required", new[] { nameof(Number) });
            yield break;
        }

        if (!PhoneMaskService.ValidatePhoneNumber(Number, CountryCode))
        {
            var maskInfo = PhoneMaskService.GetMaskInfo(CountryCode);
            yield return new ValidationResult(
                $"Invalid phone number format. Expected {maskInfo.DigitCount} digits for country code +{(int)CountryCode}",
                new[] { nameof(Number) });
        }
    }
}


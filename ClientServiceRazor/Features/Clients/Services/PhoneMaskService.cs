using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Clients.Services;

public class PhoneMaskService
{
    private static readonly Dictionary<CountryCode, PhoneMaskInfo> MaskConfiguration = new()
    {
        { CountryCode.UA, new PhoneMaskInfo { Prefix = "+380", DigitCount = 9, Mask = "+380 (XX) XXX-XX-XX" } },
        { CountryCode.US, new PhoneMaskInfo { Prefix = "+1", DigitCount = 10, Mask = "+1 (XXX) XXX-XXXX" } },
        { CountryCode.GB, new PhoneMaskInfo { Prefix = "+44", DigitCount = 10, Mask = "+44 XXXX XXXXXX" } },
        { CountryCode.DE, new PhoneMaskInfo { Prefix = "+49", DigitCount = 11, Mask = "+49 XXX XXXXXXXX" } },
        { CountryCode.FR, new PhoneMaskInfo { Prefix = "+33", DigitCount = 9, Mask = "+33 X XX XX XX XX" } }
    };

    public static PhoneMaskInfo GetMaskInfo(CountryCode countryCode)
    {
        return MaskConfiguration.TryGetValue(countryCode, out var info) 
            ? info 
            : new PhoneMaskInfo { Prefix = "+1", DigitCount = 10, Mask = "" };
    }

    public static string FormatPhoneNumber(string number, CountryCode countryCode)
    {
        if (string.IsNullOrWhiteSpace(number))
            return string.Empty;

        var maskInfo = GetMaskInfo(countryCode);
        
        // Remove all non-digit characters
        var digits = new string(number.Where(char.IsDigit).ToArray());
        
        // If digits start with country code, remove it
        if (digits.StartsWith(maskInfo.Prefix.Replace("+", "")))
        {
            digits = digits.Substring(maskInfo.Prefix.Replace("+", "").Length);
        }

        // Ensure we have the correct number of digits
        if (digits.Length > maskInfo.DigitCount)
        {
            digits = digits.Substring(0, maskInfo.DigitCount);
        }

        // Format with prefix
        return maskInfo.Prefix + digits;
    }

    public static bool ValidatePhoneNumber(string number, CountryCode countryCode)
    {
        if (string.IsNullOrWhiteSpace(number))
            return false;

        var maskInfo = GetMaskInfo(countryCode);
        
        // Remove all non-digit characters
        var digits = new string(number.Where(char.IsDigit).ToArray());
        
        // If digits start with country code, remove it
        var countryCodeDigits = maskInfo.Prefix.Replace("+", "");
        if (digits.StartsWith(countryCodeDigits))
        {
            digits = digits.Substring(countryCodeDigits.Length);
        }

        // Check if digit count is correct
        return digits.Length == maskInfo.DigitCount;
    }

    public static string GetMaskPattern(CountryCode countryCode)
    {
        return GetMaskInfo(countryCode).Mask;
    }

    public static int GetDigitCount(CountryCode countryCode)
    {
        return GetMaskInfo(countryCode).DigitCount;
    }

    public static string GetPrefix(CountryCode countryCode)
    {
        return GetMaskInfo(countryCode).Prefix;
    }
}

public class PhoneMaskInfo
{
    public string Prefix { get; set; } = string.Empty;
    public int DigitCount { get; set; }
    public string Mask { get; set; } = string.Empty;
}


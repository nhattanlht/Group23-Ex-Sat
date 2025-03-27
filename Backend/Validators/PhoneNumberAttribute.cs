using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

public class PhoneNumberAttribute : ValidationAttribute
{
    private readonly string _countryCode;

    public PhoneNumberAttribute(string countryCode)
    {
        _countryCode = countryCode;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success; // Allow null values

        var phoneNumber = value.ToString();
        
        // Get the validation service from DI container
        var serviceProvider = validationContext.GetService<IServiceProvider>();
        var validationService = serviceProvider.GetService<PhoneNumberValidationService>();

        if (validationService == null)
            return new ValidationResult("Phone number validation service not found.");

        var pattern = validationService.GetPattern(_countryCode);

        if (string.IsNullOrEmpty(pattern))
            return new ValidationResult($"No phone number rule found for {_countryCode}.");

        if (!Regex.IsMatch(phoneNumber, pattern))
            return new ValidationResult($"Số điện thoại không đúng định dạng.");

        return ValidationResult.Success;
    }
}

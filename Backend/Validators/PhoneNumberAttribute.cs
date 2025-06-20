using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using StudentManagement;
public class PhoneNumberAttribute : ValidationAttribute
{
    private readonly string _countryCode;

    public PhoneNumberAttribute(string countryCode)
    {
        _countryCode = countryCode;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var serviceProvider = validationContext.GetService<IServiceProvider>();

        // Lấy IStringLocalizer từ DI
        var localizer = serviceProvider?.GetService<IStringLocalizer<ValidationMessages>>();

        if (value == null)
        {
            return ValidationResult.Success;
        }

        var phoneNumber = value.ToString();
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return new ValidationResult(
                localizer?["PhoneNumber_Required"] ?? "Phone number is required."
            );
        }

        var validationService = serviceProvider?.GetService<PhoneNumberValidationService>();
        if (validationService == null)
            return new ValidationResult(
                localizer?["PhoneNumber_ServiceNotFound"]
                    ?? "Phone number validation service not found."
            );

        var pattern = validationService.GetPattern(_countryCode);
        if (string.IsNullOrEmpty(pattern))
            return new ValidationResult(
                localizer?["PhoneNumber_CountryNotFound", _countryCode]
                    ?? $"No phone number rule found for {_countryCode}."
            );

        if (!Regex.IsMatch(phoneNumber, pattern))
            return new ValidationResult(
                localizer?["PhoneNumber_InvalidFormat"] ?? "Số điện thoại không đúng định dạng."
            );

        return ValidationResult.Success;
    }
}

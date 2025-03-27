
public class PhoneNumberValidationService
{
    private readonly Dictionary<string, string> _phoneNumberRules;

    public PhoneNumberValidationService(IConfiguration configuration)
    {
        _phoneNumberRules = configuration.GetSection("PhoneNumberPatterns").Get<Dictionary<string, string>>() ?? new();
    }

    public string GetPattern(string countryCode)
    {
        return _phoneNumberRules.TryGetValue(countryCode, out var pattern) ? pattern : null;
    }
}

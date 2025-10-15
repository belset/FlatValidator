using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace System.Validation;

public static partial class FlatValidatorFuncs
{
    #region String - Empty or NotEmpty

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this string? text) => string.IsNullOrWhiteSpace(text);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmpty(this string? text) => !string.IsNullOrWhiteSpace(text);
    #endregion // String - Empty or NotEmpty

    #region Guid - Empty or NotEmpty
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmpty(this Guid guid) => guid != Guid.Empty;
    #endregion // Guid - Empty or NotEmpty

    #region Guid? (Nullable) - Empty or NotEmpty

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Guid? guid) => guid is null || guid.Value == Guid.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmpty(this Guid? guid) => guid is not null && guid.Value != Guid.Empty;

    #endregion Guid? (Nullable) - Empty or NotEmpty

    #region Uri
    public static bool IsAbsoluteUri(this string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var _uri) && _uri.Scheme.NotEmpty() && _uri.Host.NotEmpty();
    #endregion

    #region Email 

    //[GeneratedRegex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$")]
    //private static partial Regex s_IsEmailRegex();
    
    /// <summary>
    /// Check, the argument is valid email address
    /// </summary>
    public static bool IsEmail(this string? email) => System.Net.Mail.MailAddress.TryCreate(email, out _);
    #endregion

    #region PhoneNumber 
    [GeneratedRegex(@"^(?!\+?(?:990|997|999)\b)\+?\d{1,3}[-\s]?(?:\(\d{2,3}\)|\d{2,3})\s?(?:\d{1,3}[-\s]?\d{2,3}[-\s]?\d{2,4})(\s+\p{L}+\.?\d+)?$")]
    private static partial Regex s_IsPhoneNumberRegex();

    /// <summary>
    /// Check, the argument is valid phone number
    /// </summary>
    public static bool IsPhoneNumber(this string? phoneNumber) =>
        IsEmpty(phoneNumber) ? false : s_IsPhoneNumberRegex().IsMatch(phoneNumber!);
    #endregion

    #region CreditCardNumber
    [GeneratedRegex(@"^\+?[0-9]{1,3}[-\s]?(\(?[0-9]{2,3}\)|[0-9]{2,3})\s?([0-9]{1,3}\-?[0-9]{2,3}\-?[0-9]{2,3}|[0-9]{1,3}\s?[0-9]{2,3}\s?[0-9]{2,3})$")]
    private static partial Regex s_IsCreditCardNumberRegex();

    /// <summary>
    /// Check, the argument is valid credit card number
    /// </summary>
    public static bool IsCreditCardNumber(this string? creditCardNumber) =>
        string.IsNullOrWhiteSpace(creditCardNumber) ? false : s_IsCreditCardNumberRegex().IsMatch(creditCardNumber);
    #endregion

    #region CreditCardExpiryDate
    /// <summary>
    /// Check, the argument is not expired credit card date
    /// </summary>
    public static bool IsCreditCardExpiryDate(this string? creditCardExpiryDate)
    {
        CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.LCID);
        ci.Calendar.TwoDigitYearMax = 2099; // the end year, so it goes from 2000 to 2099.
        if (DateTime.TryParseExact(creditCardExpiryDate, "MM/yy", ci, DateTimeStyles.None, out DateTime cardExpiry))
        {
            // Check expiry greater than today & within next 6 years <7, 8>>.
            // Additional 1 day adds to compensate possible time error related to UTC.
            return cardExpiry > DateTime.UtcNow.AddDays(-1) && cardExpiry < DateTime.UtcNow.AddYears(6).AddDays(1);
        }
        return false;
    }
    #endregion

    #region CreditCardCVV
    [GeneratedRegex(@"^\d{3}$", RegexOptions.CultureInvariant)]
    private static partial Regex s_IsCreditCardCVVRegex();

    /// <summary>
    /// Check, the argument is valid CVV
    /// </summary>
    public static bool IsCreditCardCVV(this string? creditCardCVV) =>
        creditCardCVV.IsEmpty() ? false : s_IsCreditCardCVVRegex().IsMatch(creditCardCVV!);
    #endregion

    #region Language recognizing utils - https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#SupportedNamedBlocks

    [GeneratedRegex(@"\P{IsCyrillic}")] private static partial Regex s_AllCyrillicRegex();
    /// <summary>
    /// True, if there are only Cyrillic symbols.
    /// </summary>
    public static bool AllCyrillic(string stringToCheck) => !s_AllCyrillicRegex().IsMatch(stringToCheck);

    [GeneratedRegex(@"\p{IsCyrillic}")] private static partial Regex s_HasCyrillicRegex();
    /// <summary>
    /// True, if there is at least one Cyrillic symbol.
    /// </summary>
    public static bool HasCyrillic(string stringToCheck) => !s_HasCyrillicRegex().IsMatch(stringToCheck);

    [GeneratedRegex(@"\P{IsCyrillicSupplement}")] private static partial Regex s_AllCyrillicSupplementRegex();
    /// <summary>
    /// Cyrillic Supplement is a Unicode block containing Cyrillic letters for writing several minority languages, 
    /// including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    /// </summary>
    public static bool AllCyrillicSupplement(string stringToCheck) => !s_AllCyrillicSupplementRegex().IsMatch(stringToCheck);

    [GeneratedRegex(@"\p{IsCyrillicSupplement}")] private static partial Regex s_HasCyrillicSupplementRegex();
    /// <summary>
    /// Cyrillic Supplement is a Unicode block containing Cyrillic letters for writing several minority languages, 
    /// including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    /// </summary>
    public static bool HasCyrillicSupplement(string stringToCheck) => !s_HasCyrillicSupplementRegex().IsMatch(stringToCheck);

    [GeneratedRegex(@"\P{IsBasicLatin}")] private static partial Regex s_AllBasicLatinRegex();
    /// <summary>
    /// True, if there are only Latin symbols.
    /// </summary>
    public static bool AllBasicLatin(string stringToCheck) => !s_AllBasicLatinRegex().IsMatch(stringToCheck);

    [GeneratedRegex(@"\p{IsBasicLatin}")] private static partial Regex s_HasBasicLatinRegex();
    /// <summary>
    /// True, if there is at least one Latin symbol.
    /// </summary>
    public static bool HasBasicLatin(string stringToCheck) => !s_HasBasicLatinRegex().IsMatch(stringToCheck);

    #endregion // Language recognizing utils
}

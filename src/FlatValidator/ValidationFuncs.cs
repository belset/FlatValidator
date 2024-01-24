using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace System.Validation;

public static class ValidationFuncs
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

    #region Url
    public static bool IsWellFormedUri(this string? url) =>
        !string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute);
    #endregion

    #region Email 
    const string _emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
    public static bool IsEmail(this string? email) =>
        IsEmpty(email) ? false : Regex.IsMatch(email!, _emailPattern, RegexOptions.Compiled);
    #endregion

    #region PhoneNumber 
    const string _phoneNumberPattern = @"^([\+]?[0-9]?[0-9][0-9][-]?|[0])?[1-9][0-9]{8}$";
    static readonly Regex _phoneNumberRegEx = new Regex(_phoneNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
    public static bool IsPhoneNumber(this string? phoneNumber) =>
        IsEmpty(phoneNumber) ? false : _phoneNumberRegEx.IsMatch(phoneNumber!);
    #endregion

    #region CreditCardNumber
    const string _creditCardNumberPattern = @"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$";
    public static bool IsCreditCardNumber(this string? creditCardNumber) =>
        string.IsNullOrWhiteSpace(creditCardNumber) ? false : Regex.IsMatch(creditCardNumber, _creditCardNumberPattern, RegexOptions.Compiled);
    #endregion

    #region CreditCardExpiryDate
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
    const string _creditCardCVV = @"^\d{3}$";
    public static bool IsCreditCardCVV(this string? creditCardCVV) =>
        creditCardCVV.IsEmpty() ? false : Regex.IsMatch(creditCardCVV!, _creditCardCVV, RegexOptions.Compiled);
    #endregion

    #region Password
    /// <summary>
    /// Must be at least 10 characters
    /// Must contain at least one one lower case letter, one upper case letter, one digit and one special character
    /// Valid special characters are -   @#$%^&+=
    /// </summary>
    const string _password = @"^.*(?=.{NNN,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$";
    public static bool IsPassword(this string? password, byte minLength = 7) =>
        password.IsEmpty() ? false : Regex.IsMatch(password!.Replace("NNN", minLength.ToString()), _password, RegexOptions.Compiled);


    // "Password must contain at least one digit.";
    const string _passwordHasDigit = @"[0-9]+"; 
    public static bool PasswordHasDigit(this string? password) => password.IsEmpty() ? false : Regex.IsMatch(password!, _passwordHasDigit, RegexOptions.Compiled);

    // "Password must contain at least one upper case letter."
    const string _passwordHasUpperChar = @"[A-Z]+";
    public static bool PasswordHasUpperChar(this string? password) => password.IsEmpty() ? false : Regex.IsMatch(password!, _passwordHasUpperChar, RegexOptions.Compiled);

    // "Password must contain at least one lower case letter."
    const string _passwordHasLowerChar = @"[a-z]+";
    public static bool PasswordHasLowerChar(this string? password) => password.IsEmpty() ? false : Regex.IsMatch(password!, _passwordHasLowerChar, RegexOptions.Compiled);

    // "Password must contain at least one special character - @#$%^&+="
    const string _passwordHasSpecialCharacter = @"[@#$%^&+=]";
    public static bool PasswordHasSpecialCharacter(this string? password) => 
        password.IsEmpty() ? false : Regex.IsMatch(password!, _passwordHasSpecialCharacter, RegexOptions.Compiled);

    // "Password must be at least NNN characters."
    const string _passwordHasAtLeastNNNChars = @".{NNN,}";
    public static bool PasswordHasLengthAtLeast(this string? password, int charsNumber) => 
        password.IsEmpty() ? false : Regex.IsMatch(password!, _passwordHasAtLeastNNNChars.Replace("NNN", charsNumber.ToString()), RegexOptions.Compiled);
    #endregion

    #region Language recognizing utils - https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#SupportedNamedBlocks

    /// <summary>
    /// True, if there are only Cyrillic symbols
    /// </summary>
    public static bool AllCyrillic(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsCyrillic}");

    /// <summary>
    /// True, if there is at least one Cyrillic symbol
    /// </summary>
    public static bool HasCyrillic(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsCyrillic}");

    /// <summary>
    /// Cyrillic Supplement is a Unicode block containing Cyrillic letters for writing several minority languages, 
    /// including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    /// </summary>
    public static bool AllCyrillicSupplement(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsCyrillicSupplement}");
    public static bool HasCyrillicSupplement(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsCyrillicSupplement}");

    public static bool AllBasicLatin(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsBasicLatin}");
    public static bool HasBasicLatin(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsBasicLatin}");

    #endregion // Language recognizing utils
}

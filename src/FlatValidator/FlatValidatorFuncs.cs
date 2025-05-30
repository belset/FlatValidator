﻿using System.Globalization;
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
    private const string _emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
    private static Regex? _IsEmailRegex = null;
    public static bool IsEmail(this string? email) =>
        IsEmpty(email) ? false : (_IsEmailRegex ??= new Regex(_emailPattern, RegexOptions.Compiled)).IsMatch(email!);
    #endregion

    #region PhoneNumber 
    private const string _phoneNumberPattern = @"^\+?[0-9]{1,3}[-\s]?(\(?[0-9]{2,3}\)|[0-9]{2,3})\s?([0-9]{1,3}\-?[0-9]{2,3}\-?[0-9]{2,3}|[0-9]{1,3}\s?[0-9]{2,3}\s?[0-9]{2,3})$"; // @"^([\+]?[0-9]?[0-9][0-9][-]?|[0])?[1-9][0-9]{8}$";
    private static Regex? _phoneNumberRegex = null;
    public static bool IsPhoneNumber(this string? phoneNumber) =>
        IsEmpty(phoneNumber) ? false : (_phoneNumberRegex ??= new Regex(_phoneNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture)).IsMatch(phoneNumber!);
    #endregion

    #region CreditCardNumber
    private const string _creditCardNumberPattern = @"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$";
    private static Regex? _creditCardNumberRegex = null;
    public static bool IsCreditCardNumber(this string? creditCardNumber) =>
        string.IsNullOrWhiteSpace(creditCardNumber) ? false : (_creditCardNumberRegex ??= new Regex(_creditCardNumberPattern, RegexOptions.Compiled)).IsMatch(creditCardNumber);
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
    private const string _creditCardCVVPattern = @"^\d{3}$";
    private static Regex? _creditCardCVVRegex = null;
    public static bool IsCreditCardCVV(this string? creditCardCVV) =>
        creditCardCVV.IsEmpty() ? false : (_creditCardCVVRegex ??= new Regex(_creditCardCVVPattern, RegexOptions.Compiled)).IsMatch(creditCardCVV!);
    #endregion

    #region Language recognizing utils - https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#SupportedNamedBlocks

    /// <summary>
    /// True, if there are only Cyrillic symbols.
    /// </summary>
    public static bool AllCyrillic(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsCyrillic}");

    /// <summary>
    /// True, if there is at least one Cyrillic symbol.
    /// </summary>
    public static bool HasCyrillic(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsCyrillic}");

    /// <summary>
    /// Cyrillic Supplement is a Unicode block containing Cyrillic letters for writing several minority languages, 
    /// including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    /// </summary>
    public static bool AllCyrillicSupplement(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsCyrillicSupplement}");
    public static bool HasCyrillicSupplement(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsCyrillicSupplement}");

    /// <summary>
    /// True, if there are only Latin symbols.
    /// </summary>
    public static bool AllBasicLatin(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\P{IsBasicLatin}");
    
    /// <summary>
    /// True, if there is at least one Latin symbol.
    /// </summary>
    public static bool HasBasicLatin(string stringToCheck) => !Regex.IsMatch(stringToCheck, @"\p{IsBasicLatin}");

    #endregion // Language recognizing utils
}

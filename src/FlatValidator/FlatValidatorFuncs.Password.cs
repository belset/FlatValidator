using System.Buffers;
using System.Text.RegularExpressions;


namespace System.Validation;

public static partial class FlatValidatorFuncs
{
    static readonly string[] c_QwertyKeyboardPatterns =
    [
        "`12","21`","123","321","234","432","345","543","456","654","567","765","678","876","789","987",

        "qwe","ewq","wer","rew","ert","tre","rty","ytr","tyu","uyt","yui","iuy","uio","oiu","asd","dsa",
        "sdf","fds","dfg","gfd","fgh","hgf","ghj","jhg","hjk","kjh","zxc","cxz","xcv","vcx","cvb","bvc",
        "vbn","nbv",
        "QWE","EWQ","WER","REW","ERT","TRE","RTY","YTR","TYU","UYT","YUI","IUY","UIO","OIU","ASD","DSA",
        "SDF","FDS","DFG","GFD","FGH","HGF","GHJ","JHG","HJK","KJH","ZXC","CXZ","XCV","VCX","CVB","BVC",
        "VBN","NBV",

        "1qa","aq1","qaz","zaq","2ws","sw2","wsx","xsw","3ed","de3","edc","cde","4rf","fr4","rfv","vfr",
        "5tg","gt5","tgb","bgt","6yh","hy6","yhn","nhy","7uj","ju7","ujm","mju","8ik","ki8","9ol","lo9",
        "1QA","AQ1","QAZ","ZAQ","2WS","SW2","WSX","XSW","3ED","DE3","EDC","CDE","4RF","FR4","RFV","VFR",
        "5TG","GT5","TGB","BGT","6YH","HY6","YHN","NHY","7UJ","JU7","UJM","MJU","8IK","KI8","9OL","LO9",

        "esz","zse","rdx","xdr","tfc","cft","ygv","vgy","uhb","bhu","ijn","nji","okm","mko",
        "ESZ","ZSE","RDX","XDR","TFC","CFT","YGV","VGY","UHB","BHU","IJN","NJI","OKM","MKO"
    ];

    #region Password
    /// <summary>
    /// Length of the password must be at least 'minLength' symbols (by default = 8).
    /// Password must contain at least the 'minLower' number of the lower case symbols.
    /// Password must contain at least the 'minUpper' number of the upper case symbols.
    /// Password must contain at least the 'minDigits' number of the digits.
    /// Password must contain at least the 'minSpecial' number of the special symbols which may also be provided additionally.
    /// </summary>
    public static bool IsPassword(this string? password, byte minLength = 8,
                                  byte minLower = 1, byte minUpper = 1, byte minDigits = 1, 
                                  byte minSpecial = 1, string specialSymbols = null!)
    {
        if (password is not null && password.Length >= minLength)
        {
            int lowerCount = 0, upperCount = 0, digitCount = 0, specialCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                var ch = password[i];
                if (char.IsLower(ch)) lowerCount++;
                else if (char.IsUpper(ch)) upperCount++;
                else if (char.IsDigit(ch)) digitCount++;
                else if (specialSymbols is null || specialSymbols.Length == 0 || specialSymbols.Contains(ch)) specialCount++;
            }
            return minUpper <= upperCount && minLower <= lowerCount && minDigits <= digitCount && minSpecial <= specialCount;
        }
        return false;
    }
    #endregion

    #region GetPasswordStrength

    /// <summary>
    /// Defines the password security level
    /// </summary>
    public enum PasswordStrength
    {
        None,
        VeryWeak,
        Weak,
        Medium,
        Strong,
        VeryStrong
    }

    /// <summary>
    /// Calculate the cardinality of the minimal character sets necessary to brute force the password (roughly).
    /// </summary>
    /// <param name="password">The password to evaluate.</param>
    /// <returns>Strength of the password.</returns>
    public static PasswordStrength GetPasswordStrength(string? password) => 
        GetPasswordStrength(password, out _, out _);

    [GeneratedRegex(@"(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[0-2])(19\d{2}|202\d{1})|(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(19\d{2}|202\d{1})|(19\d{2}|202\d{1})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])")]
    private static partial Regex LongDateRegex();

    [GeneratedRegex(@"(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[0-2])\d{2}|(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{2}|\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])")]
    private static partial Regex ShortDateRegex();

    /// <summary>
    /// Calculate the cardinality of the minimal character sets necessary to brute force the password (roughly).
    /// </summary>
    /// <param name="password">The password to evaluate.</param>
    /// <param name="score">Calculated score for the password.</param>
    /// <param name="maxScore">Max score that is possible for the password.</param>
    /// <returns>Strength of the password.</returns>
    public static PasswordStrength GetPasswordStrength(string? password, out int score, out int maxScore)
    {
        score = 0; maxScore = 0;
        if (string.IsNullOrEmpty(password))
            return PasswordStrength.None;

        int passwordLength = password.Length;

        int lowerCount = 0, upperCount = 0, digitCount = 0, symbolCount = 0,
            consecutiveLowers = 0, consecutiveUppers = 0, consecutiveDigits = 0,
            sequential2Letters = 0, sequential2Digits = 0, sequential2Symbols = 0,
            sequential3Letters = 0, sequential3Digits = 0, sequential3Symbols = 0,
            middleDigitsOrSymbols = 0, repeatableCount = 0;
        
        char prev3 = '\0', prev2 = '\0', prev = '\0';
        for (int i = 0; i < passwordLength; i++)
        {
            char ch = password[i];

            if (char.IsLower(ch))
            {
                lowerCount++;
                consecutiveLowers += char.IsLower(prev) ? 1 : 0;
                if (prev != '\0' && (ch - 1 == prev || ch + 1 == prev))
                {
                    sequential2Letters++;
                    if (prev2 != '\0' && (ch - 2 == prev2 || ch + 2 == prev2))
                    {
                        sequential3Letters++;
                    }
                }
            }
            else if (char.IsUpper(ch))
            {
                upperCount++;
                consecutiveUppers += char.IsUpper(prev) ? 1 : 0;
                if (prev != '\0' && (ch - 1 == prev || ch + 1 == prev))
                {
                    sequential2Letters++;
                    if (prev2 != '\0' && (ch - 2 == prev2 || ch + 2 == prev2))
                    {
                        sequential3Letters++;
                    }
                }
            }
            else if (char.IsDigit(ch))
            {
                digitCount++;
                consecutiveDigits += char.IsDigit(prev) ? 1 : 0;
                if (prev != '\0' && (ch - 1 == prev || ch + 1 == prev))
                {
                    sequential2Digits++;
                    if (prev2 != '\0' && (ch - 2 == prev2 || ch + 2 == prev2))
                    {
                        sequential3Digits++;
                    }
                }
                middleDigitsOrSymbols += (i > 0 && i + 1 < passwordLength) ? 1 : 0;
            }
            else
            {
                symbolCount++;
                if (prev != '\0' && (ch - 1 == prev || ch + 1 == prev))
                {
                    sequential2Symbols++;
                    if (prev2 != '\0' && (ch - 2 == prev2 || ch + 2 == prev2))
                    {
                        sequential3Symbols++;
                    }
                }
                middleDigitsOrSymbols += (i > 0 && i + 1 < passwordLength) ? 1 : 0;
            }

            repeatableCount += (prev == ch) ? 2 : 0;
            repeatableCount += (prev2 == prev && prev == ch) ? 2 : 0;
            repeatableCount += (prev3 == prev2 && prev2 == prev && prev == ch) ? 2 : 0;
            repeatableCount += (prev3 != '\0' && prev2 != '\0' && prev != '\0' && prev3 == prev && prev2 == ch) ? 2 : 0;

            prev3 = prev2;
            prev2 = prev;
            prev = ch;
        }

        //Requirements
        if (passwordLength >= 8 && lowerCount > 0 && upperCount > 0 && digitCount > 0)
        {
            score = symbolCount > 0 ? 10 : 8;
        }

        // ADDITIONS
        score += passwordLength * 4; // Number of characters
        score += lowerCount > 0 ? (passwordLength - lowerCount) * 2 : 0; // Lowercase Letters
        score += upperCount > 0 ? (passwordLength - upperCount) * 2 : 0; // Uppercase letters
        score += digitCount * 4; // Digits
        score += symbolCount * 6; // Symbols
        score += middleDigitsOrSymbols * 2; // Middle numbers or symbols

        maxScore = score;

        // DEDUCUTIONS
        score -= (lowerCount + upperCount >= passwordLength) ? passwordLength : 0; // Letters only
        score -= (digitCount >= passwordLength) ? passwordLength : 0; // digits only

        score -= consecutiveLowers * 2; // Consecutive lowercase letters
        score -= consecutiveUppers * 2; // Consecutive uppercase letters
        score -= consecutiveDigits * 2; // Consecutive numbers

        score -= (sequential2Letters + sequential2Digits + sequential2Symbols) * 2; // Sequential (2+)
        score -= (sequential3Letters + sequential3Digits + sequential3Symbols) * 3; // Sequential (3+)

        score -= repeatableCount;

        // DDMMYYYY, MMDDYYYY, YYYYMMDD - long date-related patterns in the password
        var longDatePatternScore = 5 * LongDateRegex().Count(password);
        if (longDatePatternScore == 0)
        {
            // DDMMYY, MMDDYY, YYMMDD - short date-related patterns in the password
            longDatePatternScore = 5 * ShortDateRegex().Count(password);
        }
        score -= longDatePatternScore;

        // Keyboard-related patterns
        int keyboardPatternScore = 0;
        for (int i = 0; i < c_QwertyKeyboardPatterns.Length; i++)
        {
            if (password.Contains(c_QwertyKeyboardPatterns[i]))
                keyboardPatternScore += 3 + ((keyboardPatternScore + 1) >> 2);
        }
        score -= keyboardPatternScore;

        // Determine complexity based on overall score
        score = score < 0 ? 0 : score;
        return (int)Math.Round(score * 100.0 / maxScore) switch
        {
            >= 90 => password.Length <= 10 ? PasswordStrength.Strong : PasswordStrength.VeryStrong,
            >= 75 and < 90 => password.Length <= 8 ? PasswordStrength.Weak : PasswordStrength.Medium,
            >= 45 and < 75 => PasswordStrength.Weak,
            _ => PasswordStrength.VeryWeak
        };
    }
    #endregion

    #region Entropy

    /// <summary>
    /// This uses the Shannon entropy equation to estimate the
    /// average minimum number of bits needed to encode a
    /// string of symbols, based on the frequency of the symbols.
    /// </summary>
    /// <param name="password">Password to evaluate.</param>
    /// <param name="shannonEntropyInBits">Password entropy in bits.</param>
    /// <returns>Shannon entropy for the password.</returns>
    public static double GetShannonEntropy(string password, out int shannonEntropyInBits)
    {
        var shannonEntropy = GetShannonEntropy(password);
        shannonEntropyInBits = (int)Math.Round(password.Length * shannonEntropy);
        return shannonEntropy;
    }

    /// <summary>
    /// This uses the Shannon entropy equation to estimate the
    /// average minimum number of bits needed to encode a
    /// string of symbols, based on the frequency of the symbols.
    /// </summary>
    /// <param name="password">Password to evaluate.</param>
    /// <returns>Shannon entropy for the password.</returns>
    public static double GetShannonEntropy(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        
        int passwordLength = password.Length;
        
        char[]? buffer = null;
        Span<char> chars = passwordLength <= 128
                ? stackalloc char[passwordLength]
                : (buffer = ArrayPool<char>.Shared.Rent(passwordLength)).AsSpan(0, passwordLength);
        password.CopyTo(chars);

        double entropy = 0d;
        for (int i = 0; i < passwordLength; i++)
        {
            char ch = chars[i];
            if (ch != '\0')
            {
                int count = 1;
                for (int j = i + 1; j < passwordLength; j++)
                {
                    if (ch == chars[j])
                    {
                        count++;
                        chars[j] = '\0';
                    }
                }
                double frequency = (double)count / passwordLength;
                entropy += - frequency * Math.Log2(frequency);
            }
        }
        if (buffer is not null)
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
        return Math.Round(entropy, 4);
    }
    #endregion
}

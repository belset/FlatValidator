using System.Validation;

namespace FlatValidatorTests;

public class ValidationFuncTests
{
    public record class TestModel(
        string AString,
        Guid AGuid,
        Guid? ANullableGuid
    )
    {
        public static TestModel Empty => new TestModel(null!, Guid.Empty, null!);
    }

    public record class UrlModel(Uri Url);
    public record class NullableUrlModel(Uri? Url);

    #region _01_String_Empty_For_Null
    [Fact]
    public void _01_String_Empty_For_Null()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ErrorIf(m => m.AString.IsEmpty(), "AString is null", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is null");
    }
    #endregion // _01_String_Empty_For_Null

    #region _02_String_Empty_For_Empty
    [Fact]
    public void _02_String_Empty_For_Empty()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ErrorIf(m => m.AString.IsEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");
    }
    #endregion // _02_String_Empty_For_Empty

    #region _03_String_Empty_For_Spaced
    [Fact]
    public void _03_String_Empty_For_Spaced()
    {
        var model = new TestModel(" ", Guid.Empty, null!);
        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.AString.IsEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");

        model = new TestModel("  ", Guid.Empty, null!);
        result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.AString.IsEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");

        model = new TestModel("   ", Guid.Empty, null!);
        result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.AString.IsEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");
    }
    #endregion // _03_String_Empty_For_Spaced

    #region _04_String_NotEmpty_For_Null
    [Fact]
    public void _04_String_NotEmpty_For_Null()
    {
        var model = new TestModel(null!, Guid.Empty, null!);
        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.AString.NotEmpty(), "AString is null", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is null");
    }
    #endregion // _04_String_NotEmpty_For_Null

    #region _05_String_NotEmpty_For_Empty
    [Fact]
    public void _05_String_NotEmpty_For_Empty()
    {
        var model = new TestModel(string.Empty, Guid.Empty, null!);
        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.AString.NotEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");
    }
    #endregion // _05_String_NotEmpty_For_Empty

    #region _06_String_NotEmpty_For_Spaced
    [Fact]
    public void _06_String_NotEmpty_For_Spaced()
    {
        var model = new TestModel(" ", Guid.Empty, null!);
        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.AString.NotEmpty(), "AString is empty", m => m.AString);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AString" && e.ErrorMessage == "AString is empty");
    }
    #endregion // _06_String_NotEmpty_For_Spaced

    #region _07_AGuid_IsEmpty
    [Fact]
    public void _07_AGuid_IsEmpty()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ErrorIf(m => m.AGuid.IsEmpty(), "AGuid is null", m => m.AGuid);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AGuid" && e.ErrorMessage == "AGuid is null");
    }
    #endregion // _07_AGuid_IsEmpty

    #region _08_AGuid_NotEmpty
    [Fact]
    public void _08_AGuid_NotEmpty()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ValidIf(m => m.AGuid.NotEmpty(), "AGuid is null", m => m.AGuid);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "AGuid" && e.ErrorMessage == "AGuid is null");
    }
    #endregion // _08_AGuid_NotEmpty

    #region _09_ANullableGuid_IsEmpty
    [Fact]
    public void _09_ANullableGuid_IsEmpty()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ErrorIf(m => m.ANullableGuid.IsEmpty(), "ANullableGuid is null", m => m.ANullableGuid);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "ANullableGuid" && e.ErrorMessage == "ANullableGuid is null");
    }
    #endregion // _09_AGuid_IsEmpty

    #region _10_ANullableGuid_NotEmpty
    [Fact]
    public void _10_ANullableGuid_NotEmpty()
    {
        var result = FlatValidator.Validate(TestModel.Empty, v =>
        {
            v.ValidIf(m => m.ANullableGuid.NotEmpty(), "ANullableGuid is null", m => m.ANullableGuid);
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "ANullableGuid" && e.ErrorMessage == "ANullableGuid is null");
    }
    #endregion // _10_ANullableGuid_NotEmpty

    #region _11_IsAbsoluteUri_Null
    [Fact]
    public void _11_IsAbsoluteUri_Null()
    {
        var result = FlatValidator.Validate((string?)null, v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsAbsoluteUri_Null

    #region _11_IsAbsoluteUri_Empty
    [Fact]
    public void _11_IsAbsoluteUri_Empty()
    {
        var result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsAbsoluteUri_Empty

    #region _11_IsAbsoluteUri_Spaced
    [Fact]
    public void _11_IsAbsoluteUri_Spaced()
    {
        var result = FlatValidator.Validate(" ", v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsAbsoluteUri_Spaced

    #region _12_IsEmail_Null
    [Fact]
    public void _12_IsEmail_Null()
    {
        var result = FlatValidator.Validate((string?)null, v =>
        {
            v.ValidIf(m => m.IsEmail(), "Email is null", m => "email");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "email" && e.ErrorMessage == "Email is null");
    }
    #endregion // _12_IsEmail_Null

    #region _12_IsEmail_Empty
    [Fact]
    public void _12_IsEmail_Empty()
    {
        var result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsEmail(), "Email is empty", m => "email");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "email" && e.ErrorMessage == "Email is empty");
    }
    #endregion // _12_IsEmail_Empty

    #region _12_IsEmail_Spaced
    [Fact]
    public void _12_IsEmail_Spaced()
    {
        var result = FlatValidator.Validate(" ", v =>
        {
            v.ValidIf(m => m.IsEmail(), "Email is spaced", m => "email");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "email" && e.ErrorMessage == "Email is spaced");
    }
    #endregion // _12_IsAbsoluteUri_Spaced

    #region _12_IsEmail_ValidAddresses
    [Fact]
    public void _12_IsEmail_ValidAddresses()
    {
        string[] validEmails = [
            "simple@example.com",
            "very.common@example.com",
            "disposable.style.email.with+symbol@example.com",
            "other.email-with-hyphen@example.com",
            "fully-qualified-domain@example.co.uk",
            "user.name+tag+sorting@example.com",
            "x@example.com",
            "example-indeed@strange-example.com",
            "admin@mailserver1",
            "user@[IPv6:2001:db8::1]",
            "\"!#$%&'*+-/=?^_`{}|~\"@example.org",
            "üñîçøðé@example.com",

            "\"Abc\\@def\"@example.com",
            "\"Fred Bloggs\"@example.com",
            "\"Joe\\\\Blow\"@example.com",
            "\"Abc@def\"@example.com",
            "customer/department=shipping@example.com",
            "$A12345@example.com",
            "!def!xyz%abc@example.com",
            "_somename@example.com",
            "valid@[1.1.1.1]",
            "valid.ipv4.addr@[123.1.72.10]",
            "valid.ipv4.addr@[255.255.255.255]",
            "valid.ipv6.addr@[IPv6:::]",
            "valid.ipv6.addr@[IPv6:0::1]",
            "valid.ipv6.addr@[IPv6:::12.34.56.78]",
            "valid.ipv6.addr@[IPv6:::3333:4444:5555:6666:7777:8888]",
            "valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]",
            "valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]",
            "valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]",
            "valid.ipv6v4.addr@[IPv6:::12.34.56.78]",
            "valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]",
            new string ('a', 64) + "@example.com", // max local-part length (64 characters)
			"valid@" + new string ('a', 63) + ".com", // max subdomain length (64 characters)
			"valid@" + new string ('a', 60) + "." + new string ('b', 60) + "." + new string ('c', 60) + "." + new string ('d', 61) + ".com", // max length (254 characters)
			new string ('a', 64) + "@" + new string ('a', 45) + "." + new string ('b', 46) + "." + new string ('c', 45) + "." + new string ('d', 46) + ".com", // max local-part length (64 characters)

			// examples from wikipedia
			"niceandsimple@example.com",
            "very.common@example.com",
            "a.little.lengthy.but.fine@dept.example.com",
            "disposable.style.email.with+symbol@example.com",
            "user@[IPv6:2001:db8:1ff::a0b:dbd0]",
            "\"much.more unusual\"@example.com",
            "\"very.unusual.@.unusual.com\"@example.com",
            "\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@strange.example.com",
            "postbox@com",
            "admin@mailserver1",
            "!#$%&'*+-/=?^_`{}|~@example.org",
            "\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@example.org",
            "\" \"@example.org",

			// examples from https://github.com/Sembiance/email-validator
			"\"\\e\\s\\c\\a\\p\\e\\d\"@sld.com",
            "\"back\\slash\"@sld.com",
            "\"escaped\\\"quote\"@sld.com",
            "\"quoted\"@sld.com",
            "\"quoted-at-sign@sld.org\"@sld.com",
            "&'*+-./=?^_{}~@other-valid-characters-in-local.net",
            "01234567890@numbers-in-local.net",
            "a@single-character-in-local.org",
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@letters-in-local.org",
            "backticksarelegit@test.com",
            "bracketed-IP-instead-of-domain@[127.0.0.1]",
            "country-code-tld@sld.rw",
            "country-code-tld@sld.uk",
            "letters-in-sld@123.com",
            "local@dash-in-sld.com",
            "local@sld.newTLD",
            "local@sub.domains.com",
            "mixed-1234-in-{+^}-local@sld.net",
            "one-character-third-level@a.example.com",
            "one-letter-sld@x.org",
            "punycode-numbers-in-tld@sld.xn--3e0b707e",
            "single-character-in-sld@x.org",
            "the-character-limit@for-each-part.of-the-domain.is-sixty-three-characters.this-is-exactly-sixty-three-characters-so-it-is-valid-blah-blah.com",
            "the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-six-characters.and-this-address-is-256-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-blah.org",
            "uncommon-tld@sld.mobi",
            "uncommon-tld@sld.museum",
            "uncommon-tld@sld.travel",

            "伊昭傑@郵件.商務", // Chinese
			"राम@मोहन.ईन्फो", // Hindi
			"юзер@екзампл.ком", // Russian
			"θσερ@εχαμπλε.ψομ", // Greek
			"𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈@example.com", // surrogate pair local-part
		];
        foreach (var email in validEmails)
        {
            var result = FlatValidator.Validate(email, v =>
            {
                v.ValidIf(m => m.IsEmail(), "Email is invalid", m => "email");
            });
            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
    #endregion // _12_IsEmail_ValidAddresses

    #region _12_IsEmail_InvalidAddresses
    [Fact]
    public void _12_IsEmail_InvalidAddresses()
    {
        string[] invalidEmails = new[]
        {
            "Abc.example.com",
            "A@b@c@example.com",
            "just\"not\"right@example.com",
            "this is\"not\\allowed@example.com",
            "this\\ still\\\"not\\\\allowed@example.com",

            // Unfortunately, System.Net.Mail.MailAddress passes some invalid addresses
            // ------------------------------------------------------------------------
            // "i_like_underscore@but_its_not_allowed_in_domain.com",
            // "example@localhost",
            // "example@.com",
            // "example@com.",
            // ".example@domain.com",
            // "example.@domain.com",
            // "example..email@domain.com",
            // "example@domain..com"
        };
        foreach (var email in invalidEmails)
        {
            var result = FlatValidator.Validate(email, v =>
            {
                v.ValidIf(m => m.IsEmail(), "Email is invalid", m => "email");
            });
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Contains(result.Errors, e => e.PropertyName == "email" && e.ErrorMessage == "Email is invalid");
        }
    }
    #endregion // _12_IsEmail_InvalidAddresses

    #region _12_IsPhoneNumber_Null
    [Fact]
    public void _12_IsPhoneNumber_Null()
    {
        var result = FlatValidator.Validate((string?)null, v =>
        {
            v.ValidIf(m => m.IsPhoneNumber(), "Phone is null", m => "phone");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "phone" && e.ErrorMessage == "Phone is null");
    }
    #endregion // _12_IsPhoneNumber_Null

    #region _12_IsPhoneNumber_Empty
    [Fact]
    public void _12_IsPhoneNumber_Empty()
    {
        var result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsPhoneNumber(), "Phone is empty", m => "phone");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "phone" && e.ErrorMessage == "Phone is empty");
    }
    #endregion // _12_IsPhoneNumber_Empty

    #region _12_IsPhoneNumber_Spaced
    [Fact]
    public void _12_IsPhoneNumber_Spaced()
    {
        var result = FlatValidator.Validate(" ", v =>
        {
            v.ValidIf(m => m.IsPhoneNumber(), "Phone is spaced", m => "phone");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "phone" && e.ErrorMessage == "Phone is spaced");
    }
    #endregion // _12_IsPhoneNumber_Spaced

    #region _12_IsPhoneNumber_Valid
    [Fact]
    public void _12_IsPhoneNumber_Valid()
    {
        string[] validPhoneNumbers = 
        [
            "+12025550173",
            "+1 202-555-0173",
            "+1 (202) 555-0173",
            "2025550173",
            "202 555 0173",
            "+44 20 7946 0958",
            "+442079460958",
            "+91-9876543210",
            "09876543210",
            "+1-303-555-1212 x123",
            "+1 (303) 555-1212 ext.123",
            "+86 10 8765 4321"
        ];
        foreach (var phone in validPhoneNumbers)
        {
            var result = FlatValidator.Validate(phone, v =>
            {
                v.ValidIf(m => m.IsPhoneNumber(), "Phone is invalid", m => "phone");
            });
            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
    #endregion // _12_IsPhoneNumber_Valid

    #region _12_IsPhoneNumber_Invalid
    [Fact]
    public void _12_IsPhoneNumber_Invalid()
    {
        string[] invalidPhoneNumbers =
        [
            "123",
            "+1",
            "++1 202 555 0173",
            "+1 202-ABC-0173",
            "(202 555-0173",
            "202) 555-0173",
            "+999 1234567",
            "+1-800-FLOWERS",
            "202_555_0173",
            "202/555/0173",
            "202  555 0173",
            "+1 (202 555-0173",
            "202-555-0173 ext."
        ];
        foreach (var phone in invalidPhoneNumbers)
        {
            var result = FlatValidator.Validate(phone, v =>
            {
                v.ValidIf(m => m.IsPhoneNumber(), "Phone is invalid", m => "phone");
            });
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Contains(result.Errors, e => e.PropertyName == "phone" && e.ErrorMessage == "Phone is invalid");
        }
    }
    #endregion // _12_IsPhoneNumber_Invalid

    #region _20_IsPassword
    [Fact]
    public void _20_IsPassword()
    {
        // password can not be null
        var result = FlatValidator.Validate((string?)null, v => 
        {
            v.ValidIf(m => m.IsPassword(), "Password is null", m => "password");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "password" && e.ErrorMessage == "Password is null");

        // password can not be empty string
        result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsPassword(), "Password is null", m => "password");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "password" && e.ErrorMessage == "Password is null");

        // min length = 0
        result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsPassword(0), "Invalid password", m => "password");
        });
        Assert.True(!result.IsValid);

        Assert.False(FlatValidator.Validate("1", v => v.ValidIf(m => m.IsPassword(1), "err", m => "p")));
        Assert.False(FlatValidator.Validate("1a", v => v.ValidIf(m => m.IsPassword(2), "err", m => "p")));
        Assert.False(FlatValidator.Validate("1aA", v => v.ValidIf(m => m.IsPassword(3), "err", m => "p")));

        Assert.True(FlatValidator.Validate("1aA`", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.False(FlatValidator.Validate("1aA`", v => v.ValidIf(m => m.IsPassword(5), "err", m => "p")));

        Assert.False(FlatValidator.Validate("1ёЁ", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.False(FlatValidator.Validate("1ёЁ~", v => v.ValidIf(m => m.IsPassword(5), "err", m => "p")));

        Assert.False(FlatValidator.Validate("12345", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.False(FlatValidator.Validate("abcde", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.False(FlatValidator.Validate("ABCDE", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.False(FlatValidator.Validate(@"~`!@#$%^&-+=", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));

        Assert.False(FlatValidator.Validate(@"~`!@#$%^&-+=", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));

        Assert.True(FlatValidator.Validate("a1A`", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("aA1`", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("aA`1", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("Aa`1", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("A`a1", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("A`1a", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`A1a", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1Aa", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1aA", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));

        Assert.True(FlatValidator.Validate("``1aA", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1`aA", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1a`A", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1a1A", v => v.ValidIf(m => m.IsPassword(4), "err", m => "p")));
        Assert.True(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m => m.IsPassword(5), "err", m => "p")));
    }
    #endregion // _20_IsPassword

    #region _21_IsPassword
    [Fact]
    public void _21_IsPassword()
    {
        Assert.True(FlatValidator.Validate("`1az0AZ?",  v => v.ValidIf(m => 
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 1, minDigits: 1, minSpecial: 1, ""), "err", m => "p")));

        Assert.True(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 1, minDigits: 1, minSpecial: 1, "`?"), "err", m => "p")));

        Assert.True(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 2, minUpper: 2, minDigits: 2, minSpecial: 2, ""), "err", m => "p")));

        Assert.False(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 3, minUpper: 1, minDigits: 0, minSpecial: 1, ""), "err", m => "p")));

        Assert.False(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 3, minDigits: 0, minSpecial: 1, ""), "err", m => "p")));

        Assert.False(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 1, minDigits: 3, minSpecial: 1, ""), "err", m => "p")));

        Assert.False(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 1, minDigits: 1, minSpecial: 3, ""), "err", m => "p")));

        Assert.False(FlatValidator.Validate("`1az0AZ?", v => v.ValidIf(m =>
            m.IsPassword(minLength: 5, minLower: 1, minUpper: 1, minDigits: 1, minSpecial: 1, "@@"), "err", m => "p")));
    }
    #endregion // _21_IsPassword
}
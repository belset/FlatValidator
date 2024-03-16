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

    #region _11_IsWellFormedUri_Null
    [Fact]
    public void _11_IsWellFormedUri_Null()
    {
        var result = FlatValidator.Validate((string?)null, v =>
        {
#pragma warning disable CS0618 //supress usage of obsolet method
            v.ValidIf(m => m.IsWellFormedUri(), "Url is null", m => "url");
#pragma warning restore CS0618
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsWellFormedUri_Null

    #region _11_IsWellFormedUri_Empty
    [Fact]
    public void _11_IsWellFormedUri_Empty()
    {
        var result = FlatValidator.Validate(string.Empty, v =>
        {
#pragma warning disable CS0618 //supress usage of obsolet method
            v.ValidIf(m => m.IsWellFormedUri(), "Url is null", m => "url");
#pragma warning restore CS0618
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsWellFormedUri_Empty

    #region _11_IsWellFormedUri_Spaced
    [Fact]
    public void _11_IsWellFormedUri_Spaced()
    {
        var result = FlatValidator.Validate(" ", v =>
        {
#pragma warning disable CS0618 //supress usage of obsolet method
            v.ValidIf(m => m.IsWellFormedUri(), "Url is null", m => "url");
#pragma warning restore CS0618
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _11_IsWellFormedUri_Spaced

    #region _12_IsAbsoluteUri_Null
    [Fact]
    public void _12_IsAbsoluteUri_Null()
    {
        var result = FlatValidator.Validate((string?)null, v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _12_IsAbsoluteUri_Null

    #region _12_IsAbsoluteUri_Empty
    [Fact]
    public void _12_IsAbsoluteUri_Empty()
    {
        var result = FlatValidator.Validate(string.Empty, v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _12_IsAbsoluteUri_Empty

    #region _12_IsAbsoluteUri_Spaced
    [Fact]
    public void _12_IsAbsoluteUri_Spaced()
    {
        var result = FlatValidator.Validate(" ", v =>
        {
            v.ValidIf(m => m.IsAbsoluteUri(), "Url is null", m => "url");
        });
        Assert.True(!result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == "url" && e.ErrorMessage == "Url is null");
    }
    #endregion // _12_IsAbsoluteUri_Spaced
}
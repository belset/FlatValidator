using System.Validation;

namespace FlatValidatorTests;

public class MetaDataTests
{
    // TestModel may contain data of two different entities: personal and company data
    // In general, it will be wrong situation if both types of the data are presented.
    public record class TestModel(
        int Id, 
        string FullName, DateTime BirthDate, float Rate,
        string CompanyName, string PostalAddress
    );

    
    public record class ModelLevel0(int Id, string Name0, ModelLevel1 Level1);
    public record class ModelLevel1(int Id, string Name1, ModelLevel2 Level2);
    public record class ModelLevel2(int Id, string Name2);

    #region _01_Add_MetaData_BeforeRules
    [Fact]
    public void _01_Add_MetaData_BeforeRules()
    {
        var now = DateTimeOffset.UtcNow.ToString();
        var result = FlatValidator.Validate(DateTime.Now, v =>
        {
            v.MetaData["ValidationTime"] = now;

            v.ErrorIf(m => m.AddDays(1) > DateTime.Now, "", m => "Any text as PropName1");
        });

        Assert.True(result.MetaData.Count == 1);
        Assert.Contains(result.MetaData, pair => pair.Key == "ValidationTime" && pair.Value == now);
    }
    #endregion // _01_Add_MetaData_BeforeRules

    #region _01_Add_MetaData_AfterRules
    [Fact]
    public void _01_Add_MetaData_AfterRules()
    {
        var now = DateTimeOffset.UtcNow.ToString();
        var result = FlatValidator.Validate(DateTime.Now, v =>
        {
            v.ErrorIf(m => m.AddDays(1) > DateTime.Now, "", m => "Any text as PropName1");

            v.MetaData["ValidationTime"] = now;
        });

        Assert.True(result.MetaData.Count == 1);
        Assert.Contains(result.MetaData, pair => pair.Key == "ValidationTime" && pair.Value == now);
    }
    #endregion // _01_Add_MetaData_AfterRules

    #region _02_Add_MetaData_InsideOfRule
    [Fact]
    public void _02_Add_MetaData_InsideOfRule()
    {
        var now = DateTimeOffset.UtcNow.ToString();
        var result = FlatValidator.Validate(DateTime.Now, v =>
        {
            v.ErrorIf(m => m.AddDays(1) > DateTime.Now, m => 
            {
                v.MetaData["ValidationTime"] = now;
                return "Some error";
            }, m => "Any text as PropName1");
        });

        Assert.True(result.MetaData.Count == 1);
        Assert.Contains(result.MetaData, pair => pair.Key == "ValidationTime" && pair.Value == now);
    }
    #endregion // _02_Add_MetaData_InsideOfRule
}
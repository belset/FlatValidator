using System.Validation;

namespace FlatValidatorTests;

public class InlineTests
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

    #region _01_Any_text_as_PropName + Tag
    [Fact]
    public void _01_Any_text_as_PropName()
    {
        var result = FlatValidator.Validate(DateTime.Now, v =>
        {
            v.ErrorIf(m => m.AddDays(1) > DateTime.Now, "", m => "Any text as PropName1");
            v.ValidIf(m => m.AddDays(1) < DateTime.Now, "", m => "Any text as PropName2");
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Trim('\"') == "Any text as PropName1");
        Assert.Contains(result.Errors, e => e.PropertyName.Trim('\"') == "Any text as PropName2");
    }
    #endregion // _01_Any_text_as_PropName + Tag

    #region _02_Single_ErrorIf(+FormattedErrorMessage) + _02_Single_ValidIf(+FormattedErrorMessage)
    [Fact]
    public void _02_Single_ErrorIf()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, "Id is incorrect", m => m.Id);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id) && e.ErrorMessage == "Id is incorrect");
    }

    [Fact]
    public void _02_Single_ErrorIf_With_FormattedErrorMessage()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, m => $"Id '{m.Id}' is incorrect", m => m.Id);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id) && e.ErrorMessage == "Id '-1' is incorrect");
    }

    [Fact]
    public void _02_Single_ValidIf()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.Id > 0, "Id is incorrect", m => m.Id);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id) && e.ErrorMessage == "Id is incorrect");
    }

    [Fact]
    public void _02_Single_ValidIf_FormattedErrorMessage()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.Id > 0, m => $"Id '{m.Id}' is incorrect", m => m.Id);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id) && e.ErrorMessage == "Id '-1' is incorrect");
    }
    #endregion // _02_Single_ErrorIf(+FormattedErrorMessage) + _02_Single_ValidIf(+FormattedErrorMessage)

    #region _03_Many_ErrorIf + _03_Many_ValidIf()
    [Fact]
    public void _03_Many_ErrorIf()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, "Id is incorrect", m => m.Id);
            v.ErrorIf(m => m.FullName.IsEmpty(), "FullName cannot be empty", m => m.FullName);
            v.ErrorIf(m => m.BirthDate.AddYears(15) >= DateTime.Now, "Incorrect age", m => m.BirthDate);
            v.ErrorIf(m => m.Rate < 0 && m.BirthDate.AddYears(15) >= DateTime.Now, "Rate cannot be negative if age < 15", m => m.Rate);
            v.ErrorIf(m => m.CompanyName.IsEmpty(), "CompanyName cannot be empty", m => m.CompanyName);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 5);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.BirthDate));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Rate));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.CompanyName));
    }

    [Fact]
    public void _03_Many_ValidIf()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.Id > 0, "Id is incorrect", m => m.Id);
            v.ValidIf(m => m.FullName.NotEmpty(), "FullName cannot be empty", m => m.FullName);
            v.ValidIf(m => m.BirthDate.AddYears(15) < DateTime.Now, "Incorrect age", m => m.BirthDate);
            v.ValidIf(m => m.Rate >= 0 || m.BirthDate.AddYears(15) < DateTime.Now, "Rate cannot be negative if age < 15", m => m.Rate);
            v.ValidIf(m => m.CompanyName.NotEmpty(), "CompanyName cannot be empty", m => m.CompanyName);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 5);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.BirthDate));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Rate));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.CompanyName));
    }
    #endregion // _03_Many_ErrorIf + _03_Many_ValidIf()

    #region _04_ErrorIf_with_2_MemberSelectors(+FmtErrMsg) + _04_ValidIf_with_2_MemberSelectors(+FmtErrMsg)

    [Fact]
    public void _04_ErrorIf_with_2_MemberSelectors()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "Company Ltd", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            const string errorMessage = "Only personal info or company info may be represented at the same time";
            v.ErrorIf(m => m.Id <= 0 || m.CompanyName.IsEmpty(), errorMessage, m => m.Id, m => m.CompanyName);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 2);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.CompanyName));
    }
    
    [Fact]
    public void _04_ErrorIf_with_2_MemberSelectors_FmtErrMsg()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0 || m.CompanyName.IsEmpty(), m => $"Id={m.Id}, CompanyName={m.CompanyName}", m => m.Id, m => m.CompanyName);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 2);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.CompanyName));
    }

    [Fact]
    public void _04_ValidIf_with_2_MemberSelectors_FmtErrMsg()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.Id > 0 && m.CompanyName.NotEmpty(), m => $"Id={m.Id}, CompanyName={m.CompanyName}", m => m.Id, m => m.CompanyName);
        });

        Assert.False(result.IsValid);

        Assert.True(result.Errors.Count == 2);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.Id));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TestModel.CompanyName));
    }
    #endregion // _04_ErrorIf_with_2_MemberSelectors(+FmtErrMsg) + _04_ValidIf_with_2_MemberSelectors(+FmtErrMsg)

    #region _05_ErrorIf_with_3_MemberSelectors(+FmtErrMsg) + _05_ValidIf_with_3_MemberSelectors(+FmtErrMsg)
    [Fact]
    public void _05_ErrorIf_with_3_MemberSelectors()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "Company Ltd", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            const string errorMessage = "Only personal info or company info may be represented at the same time";
            v.ErrorIf(m => m.FullName.NotEmpty() && m.CompanyName.NotEmpty(), 
                errorMessage, m => m.FullName, m => m.CompanyName, m => m.PostalAddress);
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.CompanyName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.PostalAddress));
    }

    [Fact]
    public void _05_ValidIf_with_3_MemberSelectors()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "Company Ltd", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            const string errorMessage = "Some fields are incorrect: [Id, CompanyName, PostalAddress]";
            v.ValidIf(m => m.Id > 0 && m.CompanyName.NotEmpty() && m.PostalAddress.NotEmpty(), 
                        errorMessage, m => m.FullName, m => m.CompanyName, m => m.PostalAddress);
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.CompanyName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.PostalAddress));
    }

    [Fact]
    public void _05_ErrorIf_with_3_MemberSelectors_FmtErrMsg()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "Company Ltd", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.FullName.NotEmpty() && m.CompanyName.NotEmpty(),
                m => $"FullName={m.FullName}, CompanyName={m.CompanyName}, PostalAddress={m.PostalAddress}", 
                m => m.FullName, m => m.CompanyName, m => m.PostalAddress);
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.CompanyName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.PostalAddress));
        Assert.Contains(result.Errors, x => x.ErrorMessage == "FullName=John Doe, CompanyName=Company Ltd, PostalAddress=");
    }

    [Fact]
    public void _05_ValidIf_with_3_MemberSelectors_FmtErrMsg()
    {
        var model = new TestModel(0, "John Doe", DateTime.Now, 0, "Company Ltd", null!);

        var result = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(m => m.Id > 0 && m.CompanyName.NotEmpty() && m.PostalAddress.NotEmpty(),
                m => $"FullName={m.FullName}, CompanyName={m.CompanyName}, PostalAddress={m.PostalAddress}", 
                m => m.FullName, m => m.CompanyName, m => m.PostalAddress);
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.FullName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.CompanyName));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(TestModel.PostalAddress));
        Assert.Contains(result.Errors, x => x.ErrorMessage == "FullName=John Doe, CompanyName=Company Ltd, PostalAddress=");
    }
    #endregion // _05_ErrorIf_with_3_MemberSelectors(+FmtErrMsg) + _05_ValidIf_with_3_MemberSelectors(+FmtErrMsg)

    [Fact]
    public void _06_Group_NestedGroup_ExecutionOrder()
    {
        ModelLevel0 model = new(1, "L0", new(2, "L1", new(3, "L3")));

        var r = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id == 1, "ErrorIf for Level0.Id", m => m.Id);
            v.ValidIf(m => m.Id != 1, "ValidIf for Level0.Id", m => m.Id);
            v.ErrorIf(m => m.Level1.Id == 2, "ErrorIf for Level1.Id", m => m.Level1.Id);
            v.ValidIf(m => m.Level1.Id != 2, "ValidIf for Level1.Id", m => m.Level1.Id);
            v.ErrorIf(m => m.Level1.Level2.Id == 3, "ErrorIf for Level1.Level2.Id", m => m.Level1.Level2.Id);
            v.ValidIf(m => m.Level1.Level2.Id != 3, "ValidIf for Level1.Level2.Id", m => m.Level1.Level2.Id);

            v.Grouped(m => m.Name0.NotEmpty(), m =>
            {
                v.Grouped(m => m.Level1.Name1.NotEmpty(), m =>
                {
                    v.ErrorIf(m => m.Level1.Level2.Id == 3, "Grouped ErrorIf for Level1.Level2.Id", m => m.Level1.Level2.Id);
                    v.ValidIf(m => m.Level1.Level2.Id != 3, "Grouped ValidIf for Level1.Level2.Id", m => m.Level1.Level2.Id);
                });
                v.ErrorIf(m => m.Level1.Id == 2, "Grouped ErrorIf for Level1.Id", m => m.Level1.Id);
                v.ValidIf(m => m.Level1.Id != 2, "Grouped ValidIf for Level1.Id", m => m.Level1.Id);
            });

            v.Grouped(m => false, m =>
            {
                Assert.True(false, "This line must never be reached.");
            },
            @else: m => // test 'else' block
            {
                v.Error(m => $"PropName:{m.Level1.Level2.Name2}", m => m.Level1.Level2.Name2);
            });
        });

        Assert.False(r);

        // test count and order of errors 
        Assert.True(r.Errors.Count == 11);
        Assert.True(r.Errors[0].PropertyName == "Id" && r.Errors[0].ErrorMessage == "ErrorIf for Level0.Id");
        Assert.True(r.Errors[1].PropertyName == "Id" && r.Errors[1].ErrorMessage == "ValidIf for Level0.Id");
        Assert.True(r.Errors[2].PropertyName == "Level1.Id" && r.Errors[2].ErrorMessage == "ErrorIf for Level1.Id");
        Assert.True(r.Errors[3].PropertyName == "Level1.Id" && r.Errors[3].ErrorMessage == "ValidIf for Level1.Id");
        Assert.True(r.Errors[4].PropertyName == "Level1.Level2.Id" && r.Errors[4].ErrorMessage == "ErrorIf for Level1.Level2.Id");
        Assert.True(r.Errors[5].PropertyName == "Level1.Level2.Id" && r.Errors[5].ErrorMessage == "ValidIf for Level1.Level2.Id");
        Assert.True(r.Errors[6].PropertyName == "Level1.Level2.Id" && r.Errors[6].ErrorMessage == "Grouped ErrorIf for Level1.Level2.Id");
        Assert.True(r.Errors[7].PropertyName == "Level1.Level2.Id" && r.Errors[7].ErrorMessage == "Grouped ValidIf for Level1.Level2.Id");
        Assert.True(r.Errors[8].PropertyName == "Level1.Id" && r.Errors[8].ErrorMessage == "Grouped ErrorIf for Level1.Id");
        Assert.True(r.Errors[9].PropertyName == "Level1.Id" && r.Errors[9].ErrorMessage == "Grouped ValidIf for Level1.Id");

        Assert.True(r.Errors[10].PropertyName == "Level1.Level2.Name2" && r.Errors[10].ErrorMessage == "PropName:L3");

        // test ToDictionary()
        var dict = r.ToDictionary();
        Assert.True(dict.Count == 4);
        Assert.True(dict.ContainsKey("Id"));
        Assert.True(dict.ContainsKey("Level1.Id"));
        Assert.True(dict.ContainsKey("Level1.Level2.Id"));
        Assert.True(dict.ContainsKey("Level1.Level2.Name2"));
    }

    [Fact]
    public void _07_ToDictionary_for_FlatValidatorResults()
    {
        var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

        var results = new FlatValidationResult[2];
        results[0] = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, "Error0", m => m.Id);
        });
        results[1] = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, "Error1", m => m.Id);
        });
        Assert.False(results[0].IsValid);
        Assert.False(results[1].IsValid);

        Assert.Contains(results[0].Errors, e => e.ErrorMessage == "Error0");
        Assert.Contains(results[1].Errors, e => e.ErrorMessage == "Error1");

        var dict = results.ToDictionary<TestModel>();
        Assert.True(string.Join(", ", dict.Keys) == nameof(TestModel.Id));

        var messages = dict[nameof(TestModel.Id)];
        Assert.True(messages.Length == 2);
        Assert.Contains(messages, msg => msg == "Error0");
        Assert.Contains(messages, msg => msg == "Error1");
    }

    //[Fact]
    //public void _07_Error_With_Tag()
    //{
    //    var model = new TestModel(-1, "", DateTime.Now, -100, "", null!);

    //    var result = FlatValidator.Validate(model, v =>
    //    {
    //        v.ErrorIf(m => m.Id <= 0, "ErrorMessageWithTag", m => m.Id).Tag("Tag1");
    //        v.ErrorIf(m => m.Id <= 0, m => "ErrorMessageCallbackWithTag", m => m.Id).Tag("Tag2");
    //    });

    //    Assert.False(result.IsValid);

    //    Assert.Contains(result.Errors, e => e.ErrorMessage == "ErrorMessageWithTag" && e.Tag == "Tag1");
    //    Assert.Contains(result.Errors, e => e.ErrorMessage == "ErrorMessageCallbackWithTag" && e.Tag == "Tag2");
    //}
}
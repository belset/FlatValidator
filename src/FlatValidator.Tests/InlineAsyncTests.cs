using System.Diagnostics;
using System.Validation;

namespace FlatValidatorTests;

public class InlineAsyncTests
{
    public record class TestModel(
        int Id,
        string FullName, DateOnly BirthDate, float Rate,
        string CompanyName, string PostalAddress
    );

    #region _01_Cancalletion_in_ErrorIf_1_Member()
    [Fact]
    public async void _01_Cancalletion_in_ErrorIf_1_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, m => $"Something wrong. {m.Id}", model => model.Id);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, "Something wrong.", model => model.Id);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ErrorIf_1_Member()

    #region _01_Cancalletion_in_ErrorIf_2_Member()
    [Fact]
    public async void _01_Cancalletion_in_ErrorIf_2_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, m => $"Something wrong. {m.Id}", model => model.Id, model => model.BirthDate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, "Something wrong.", model => model.Id, model => model.BirthDate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ErrorIf_2_Member()

    #region _01_Cancalletion_in_ErrorIf_3_Member()
    [Fact]
    public async void _01_Cancalletion_in_ErrorIf_3_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, m => $"Something wrong. {m.Id}", model => model.Id, model => model.BirthDate, model => model.Rate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ErrorIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(true);

                }, "Something wrong.", model => model.Id, model => model.BirthDate, model => model.Rate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ErrorIf_3_Member()

    #region _01_Cancalletion_in_ValidIf_1_Member()
    [Fact]
    public async void _01_Cancalletion_in_ValidIf_1_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, m => $"Something wrong. {m.Id}", model => model.Id);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, "Something wrong.", model => model.Id);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ValidIf_1_Member()

    #region _01_Cancalletion_in_ValidIf_2_Member()
    [Fact]
    public async void _01_Cancalletion_in_ValidIf_2_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, m => $"Something wrong. {m.Id}", model => model.Id, model => model.BirthDate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, "Something wrong.", model => model.Id, model => model.BirthDate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ValidIf_2_Member()

    #region _01_Cancalletion_in_ValidIf_3_Member()
    [Fact]
    public async void _01_Cancalletion_in_ValidIf_3_Member()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        // for fmt message
        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, m => $"Something wrong. {m.Id}", model => model.Id, model => model.BirthDate, model => model.Rate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);

        // for plain message
        isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.ValidIf((model, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Debug.WriteLine("This line must never be reached.");

                    return ValueTask.FromResult(false);

                }, "Something wrong.", model => model.Id, model => model.BirthDate, model => model.Rate);

            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ValidIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _01_Cancalletion_in_ValidIf_3_Member()

    #region _02_Cancalletion_in_GroupConditions()
    [Fact]
    public async void _02_Cancalletion_in_GroupConditions()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.Grouped(
                    (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.True(false, "This line must never be reached.");

                        return ValueTask.FromResult(true);
                    },
                    (model, cancellationToken) =>
                    {
                        validator.ErrorIf(model => true, "Something wrong.", model => model.Id);
                    }
                );
            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _02_Cancalletion_in_GroupConditions()

    #region _02_Cancalletion_in_GroupAction()
    [Fact]
    public async void _02_Cancalletion_in_GroupAction()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.Grouped(
                    (model, cancellationToken) =>
                    {
                        return ValueTask.FromResult(true);
                    },
                    (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.True(false, "This line must never be reached.");

                        validator.ErrorIf(model => true, "Something wrong.", model => model.Id);
                    }
                );
            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _02_Cancalletion_in_GroupAction()

    #region _02_Cancalletion_in_GroupElseAction()
    [Fact]
    public async void _02_Cancalletion_in_GroupElseAction()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.Grouped(
                    (model, cancellationToken) =>
                    {
                        return ValueTask.FromResult(false);
                    },
                    (model, cancellationToken) =>
                    {
                        Assert.True(false, "This line must never be reached.");
                    },
                    @else: (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.True(false, "This line must never be reached.");

                        validator.ErrorIf(model => true, "Something wrong.", model => model.Id);
                    }
                );
            }, cancellationToken: cts.Token);
        }
        catch (OperationCanceledException)
        {
            isOperationCancelled = true;
            Debug.WriteLine("ErrorIf has been cancelled due to CancalletionToken was raised.");
        }
        Assert.True(isOperationCancelled);
    }
    #endregion // _02_Cancalletion_in_GroupElseAction()

    #region _03_Async_ErrorIf_1_MemberSelector()
    [Fact]
    public void _03_Async_ErrorIf_1_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(m.Id < 0), "Id is incorrect", m => m.Id);
        });
        Assert.True(result1.Errors.Count == 1);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id is incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(m.Id < 0), m => $"Id is incorrect ({m.Id})", m => m.Id);
        });
        Assert.True(result2.Errors.Count == 1);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id is incorrect (-1)");
    }
    #endregion _03_Async_ErrorIf_1_MemberSelector()

    #region _03_Async_ValidIf_1_MemberSelector()
    [Fact]
    public void _03_Async_ValidIf_1_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(m.Id > 0), "Id is incorrect", m => m.Id);
        });
        Assert.True(result1.Errors.Count == 1);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id is incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(m.Id > 0), m => $"Id is incorrect ({m.Id})", m => m.Id);
        });
        Assert.True(result2.Errors.Count == 1);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id is incorrect (-1)");
    }
    #endregion _03_Async_ValidIf_1_MemberSelector()

    #region _03_Async_ErrorIf_2_MemberSelector()
    [Fact]
    public void _03_Async_ErrorIf_2_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(true), "Id,BirthDate are incorrect", m => m.Id, m => m.BirthDate);
        });
        Assert.True(result1.Errors.Count == 2);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id,BirthDate are incorrect");
        Assert.True(result1.Errors[1].PropertyName == "BirthDate" && result1.Errors[1].ErrorMessage == "Id,BirthDate are incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(true), m => $"Id,BirthDate are incorrect ({m.Id},{m.BirthDate.ToString("yyyy-MM-dd")})", m => m.Id, m => m.BirthDate);
        });
        Assert.True(result2.Errors.Count == 2);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id,BirthDate are incorrect (-1,2000-01-01)");
        Assert.True(result2.Errors[1].PropertyName == "BirthDate" && result2.Errors[1].ErrorMessage == "Id,BirthDate are incorrect (-1,2000-01-01)");
    }
    #endregion _03_Async_ErrorIf_2_MemberSelector()

    #region _03_Async_ValidIf_2_MemberSelector()
    [Fact]
    public void _03_Async_ValidIf_2_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(false), "Id,BirthDate are incorrect", m => m.Id, m => m.BirthDate);
        });
        Assert.True(result1.Errors.Count == 2);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id,BirthDate are incorrect");
        Assert.True(result1.Errors[1].PropertyName == "BirthDate" && result1.Errors[1].ErrorMessage == "Id,BirthDate are incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(false), m => $"Id,BirthDate are incorrect ({m.Id},{m.BirthDate.ToString("yyyy-MM-dd")})", m => m.Id, m => m.BirthDate);
        });
        Assert.True(result2.Errors.Count == 2);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id,BirthDate are incorrect (-1,2000-01-01)");
        Assert.True(result2.Errors[1].PropertyName == "BirthDate" && result2.Errors[1].ErrorMessage == "Id,BirthDate are incorrect (-1,2000-01-01)");
    }
    #endregion _03_Async_ValidIf_2_MemberSelector()

    #region _03_Async_ErrorIf_3_MemberSelector()
    [Fact]
    public void _03_Async_ErrorIf_3_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(true), 
                "Id,BirthDate,Rate are incorrect", 
                m => m.Id, m => m.BirthDate, m => m.Rate);
        });
        Assert.True(result1.Errors.Count == 3);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id,BirthDate,Rate are incorrect");
        Assert.True(result1.Errors[1].PropertyName == "BirthDate" && result1.Errors[1].ErrorMessage == "Id,BirthDate,Rate are incorrect");
        Assert.True(result1.Errors[2].PropertyName == "Rate" && result1.Errors[2].ErrorMessage == "Id,BirthDate,Rate are incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(async m => await ValueTask.FromResult(true), 
                m => $"Id,BirthDate,Rate are incorrect ({m.Id},{m.BirthDate.ToString("yyyy-MM-dd")})", 
                m => m.Id, m => m.BirthDate, m => m.Rate);
        });
        Assert.True(result2.Errors.Count == 3);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01)");
        Assert.True(result2.Errors[1].PropertyName == "BirthDate" && result2.Errors[1].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01)");
        Assert.True(result2.Errors[2].PropertyName == "Rate" && result2.Errors[2].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01)");
    }
    #endregion _03_Async_ErrorIf_3_MemberSelector()

    #region _03_Async_ValidIf_3_MemberSelector()
    [Fact]
    public void _03_Async_ValidIf_3_MemberSelector()
    {
        var model = new TestModel(-1, "", new DateOnly(2000, 1, 1), -100, "", null!);

        var result1 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(false), 
                "Id,BirthDate,Rate are incorrect", 
                m => m.Id, m => m.BirthDate, m => m.Rate);
        });
        Assert.True(result1.Errors.Count == 3);
        Assert.True(result1.Errors[0].PropertyName == "Id" && result1.Errors[0].ErrorMessage == "Id,BirthDate,Rate are incorrect");
        Assert.True(result1.Errors[1].PropertyName == "BirthDate" && result1.Errors[1].ErrorMessage == "Id,BirthDate,Rate are incorrect");
        Assert.True(result1.Errors[2].PropertyName == "Rate" && result1.Errors[2].ErrorMessage == "Id,BirthDate,Rate are incorrect");

        var result2 = FlatValidator.Validate(model, v =>
        {
            v.ValidIf(async m => await ValueTask.FromResult(false), 
                m => $"Id,BirthDate,Rate are incorrect ({m.Id},{m.BirthDate.ToString("yyyy-MM-dd")},{m.Rate})", 
                m => m.Id, m => m.BirthDate, m => m.Rate);
        });
        Assert.True(result2.Errors.Count == 3);
        Assert.True(result2.Errors[0].PropertyName == "Id" && result2.Errors[0].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01,-100)");
        Assert.True(result2.Errors[1].PropertyName == "BirthDate" && result2.Errors[1].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01,-100)");
        Assert.True(result2.Errors[2].PropertyName == "Rate" && result2.Errors[2].ErrorMessage == "Id,BirthDate,Rate are incorrect (-1,2000-01-01,-100)");
    }
    #endregion _03_Async_ValidIf_3_MemberSelector()

}
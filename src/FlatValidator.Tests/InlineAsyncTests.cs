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

    [Fact]
    public async void _01_Cancalletion_in_ErrorIf()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

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

    [Fact]
    public async void _01_Cancalletion_in_ValidIf()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

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

}
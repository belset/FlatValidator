using System.Diagnostics;
using System.Validation;

namespace FlatValidatorTests;

public class MultithreadTests
{
    record class TestModel(
        int Id,
        string FullName, DateOnly BirthDate, float Rate,
        string CompanyName, string PostalAddress
    );
    class LockAndWaitValidator : FlatValidator<TestModel>
    {
        public LockAndWaitValidator()
        {
            ErrorIf(m => m.Id <= 0, "Invalid Id.", m => m.Id);
        }
    };

    [Fact]
    public async Task _01_Cancalletion_in_ValidIf()
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
    public async Task _02_Cancalletion_in_GroupConditions()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.When(
                    (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.Fail("This line must never be reached.");

                        return ValueTask.FromResult(true);
                    },
                    @then: (model, cancellationToken) =>
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
    public async Task _02_Cancalletion_in_GroupAction()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.When(
                    (model, cancellationToken) =>
                    {
                        return ValueTask.FromResult(true);
                    },
                    @then: (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.Fail("This line must never be reached.");

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
    public async Task _02_Cancalletion_in_GroupElseAction()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        bool isOperationCancelled = false;
        try
        {
            var cts = new CancellationTokenSource();
            _ = await FlatValidator.ValidateAsync(model, validator =>
            {
                cts.Cancel();
                validator.When(
                    (model, cancellationToken) =>
                    {
                        return ValueTask.FromResult(false);
                    },
                    @then: (model, cancellationToken) =>
                    {
                        Assert.Fail("This line must never be reached.");
                    },
                    @else: (model, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Assert.Fail("This line must never be reached.");

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


    class Sync_Conditions_Validator : FlatValidator<TestModel>
    {
        public Sync_Conditions_Validator()
        {
            ErrorIf(m =>
            {
                Task.Delay(Random.Shared.Next(2));
                return true;

            }, "Something wrong.", model => model.Id);
        }
    };

    class Async_Conditions_Validator : FlatValidator<TestModel>
    {
        public Async_Conditions_Validator()
        {
            ErrorIf(async m =>
            {
                await Task.Delay(Random.Shared.Next(2));
                return true;

            }, "Something wrong.", model => model.Id);
        }
    };

    [Fact]
    public void _03_Concurently_Sync_For_SingleInstance()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        var syncConditionsValidator = new Sync_Conditions_Validator();
        var asyncConditionsValidator = new Async_Conditions_Validator();
        var options = new ParallelOptions() { MaxDegreeOfParallelism = 10 };
        Parallel.ForEach(Enumerable.Range(1, 100), options, _ =>
        {
            try
            {
                var synchResult = syncConditionsValidator.Validate(model, TimeSpan.FromSeconds(10000));
                var asyncResult = asyncConditionsValidator.Validate(model, TimeSpan.FromSeconds(10000));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        });
    }

    [Fact]
    public void _03_Concurently_Async_For_SingleInstance()
    {
        var model = new TestModel(-1, "", DateOnly.FromDayNumber(365), -100, "", null!);

        var syncConditionsValidator = new Sync_Conditions_Validator();
        var asyncConditionsValidator = new Async_Conditions_Validator();
        var options = new ParallelOptions() { MaxDegreeOfParallelism = 10 };
        Parallel.ForEach(Enumerable.Range(1, 100), options, async _ =>
        {
            try
            {
                var synchResult = await syncConditionsValidator.ValidateAsync(model, TimeSpan.FromSeconds(10000));
                var asyncResult = await asyncConditionsValidator.ValidateAsync(model, TimeSpan.FromSeconds(10000));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        });
    }

}
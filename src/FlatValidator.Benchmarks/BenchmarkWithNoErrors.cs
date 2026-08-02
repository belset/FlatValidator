using System.Data;
using System.Diagnostics;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using FlatValidatorBenchmarks.Models;

namespace FlatValidatorBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared)]
public class BenchmarkWithNoErrors
{
    private List<BigModel> _noErrorModels { get; set; } = default!;
    
    [Params(100, 1_000, 10_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Bogus.Randomizer.Seed = new Random(1000);

        _noErrorModels = Enumerable.Range(0, Size).Select(x => BigModel.CreateWithNoErrors()).ToList();
    }

    [Benchmark(Baseline = true)]
    public void FlatValidator_NoErrors()
    {
        var validator = new FlatValidatorForBigModel();
        foreach (var model in _noErrorModels)
        {
            using var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                Debug.Assert(validationResult.Errors.Count == 0);
            }
        }
    }

    [Benchmark]
    public void FluentValidator_NoErrors()
    {
        var validator = new FluentValidationForBigModel();
        foreach (var model in _noErrorModels)
        {
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                Debug.Assert(validationResult.Errors.Count == 0);
            }
        }
    }
}

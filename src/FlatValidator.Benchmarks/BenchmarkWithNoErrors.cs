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
        _noErrorModels = Enumerable.Range(0, 999).Select(x => BigModel.CreateWithNoErrors()).ToList();

        foreach (var model in _noErrorModels)
        {
            var flatResult = new FlatValidatorForBigModel().Validate(model).ToDictionary();
            var fluentResult = new FluentValidatorForBigModel().Validate(model).ToDictionary();

            if (!flatResult.All(pair => fluentResult[pair.Key].Length == pair.Value.Length) ||
                !fluentResult.All(pair => flatResult[pair.Key].Length == pair.Value.Length))
            {
                throw new ApplicationException("ValidationResuls do not match.");
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void FlatValidator_NoErrors()
    {
        foreach (var model in _noErrorModels)
        {
            var validationResult = new FlatValidatorForBigModel().Validate(model);
            var validationStatus = validationResult.IsValid;
            if (!validationStatus)
            {
                var problems = validationResult.ToDictionary();
                Debug.Assert(problems.Count > 0);
            }
        }
    }

    [Benchmark]
    public void FluentValidator_NoErrors()
    {
        foreach (var model in _noErrorModels)
        {
            var validationResult = new FluentValidatorForBigModel().Validate(model);
            var validationStatus = validationResult.IsValid;
            if (!validationStatus)
            {
                var problems = validationResult.ToDictionary();
                Debug.Assert(problems.Count > 0);
            }
        }
    }
}

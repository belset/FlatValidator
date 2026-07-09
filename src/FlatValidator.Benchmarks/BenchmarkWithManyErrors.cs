using System.Data;
using System.Diagnostics;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using FlatValidatorBenchmarks.Models;

namespace FlatValidatorBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared)]
public class BenchmarkWithManyErrors
{
    private List<BigModel> _manyErrorModels { get; set;  } = default!;

    [Params(100, 1_000, 10_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Bogus.Randomizer.Seed = new Random(1000);

        _manyErrorModels = Enumerable.Range(0, Size).Select(x => BigModel.CreateWithManyErrors()).ToList();

    }

    [Benchmark(Baseline = true)]
    public void FlatValidator_ManyErrors()
    {
        var validator = new FlatValidatorForBigModel();
        foreach (var model in _manyErrorModels)
        {
            using var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                Debug.Assert(validationResult.Errors.Count > 0);
            }
        }
    }

    [Benchmark]
    public void FluentValidator_ManyErrors()
    {
        var validator = new FluentValidatorForBigModel();
        foreach (var model in _manyErrorModels)
        {
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                Debug.Assert(validationResult.Errors.Count > 0);
            }
        }
    }
}

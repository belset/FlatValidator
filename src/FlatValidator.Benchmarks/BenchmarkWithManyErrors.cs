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
        _manyErrorModels = Enumerable.Range(0, 999).Select(x => BigModel.CreateWithManyErrors()).ToList();
    }

    [Benchmark(Baseline = true)]
    public void FlatValidator_ManyErrors()
    {
        foreach (var model in _manyErrorModels)
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
    public void FluentValidator_ManyErrors()
    {
        foreach (var model in _manyErrorModels)
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

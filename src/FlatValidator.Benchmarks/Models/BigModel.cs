using Bogus;

namespace FlatValidatorBenchmarks.Models;

public class BigModel
{
    public class NestedModel
    {
        public string NestedText1 { get; set; } = default!;
        public string NestedText2 { get; set; } = default!;
    }

    public required string Text1 { get; set; }
    public required string Text2 { get; set; }
    public required string Text3 { get; set; }
    public required string Text4 { get; set; }
    public required string Text5 { get; set; }

    public int Number1 { get; set; }
    public int Number2 { get; set; }
    public int Number3 { get; set; }

    public decimal? DecimalNumber1 { get; set; }
    public decimal? DecimalNumber2 { get; set; }
    public decimal? DecimalNumber3 { get; set; }

    public required List<string> StringCollection { get; set; }
    public required List<int> IntCollection { get; set; }
    public required List<decimal> DecimalCollection { get; set; }

    public required NestedModel NestedModel1 { get; set; } = new();

    public static NestedModel CreateNestedWithNoErrors() => new Faker<NestedModel>()
        .RuleFor(m => m.NestedText1, m => "NestedText1: " + m.Lorem.Word())  // startsWith("NestedText1:")
        .RuleFor(m => m.NestedText2, m => "NestedText2: " + m.Lorem.Word()); // startsWith("NestedText2:")

    public static NestedModel CreateNestedWithManyErrors() => new Faker<NestedModel>()
        .RuleFor(m => m.NestedText1, f => f.Lorem.Word().OrNull(f, 0.5f))
        .RuleFor(m => m.NestedText2, f => f.Lorem.Word().OrNull(f, 0.5f));

    public static BigModel CreateWithNoErrors() => new Faker<BigModel>()
        .RuleFor(m => m.Text1, m => m.Lorem.Word() + "a") // contains('a')
        .RuleFor(m => m.Text2, m => m.Lorem.Word() + "b") // contains('b')
        .RuleFor(m => m.Text3, m => m.Lorem.Word() + "c") // contains('c')
        .RuleFor(m => m.Text4, m => m.Lorem.Word() + "d") // contains('d')
        .RuleFor(m => m.Text5, m => m.Lorem.Word() + "e") // contains('e')
        .RuleFor(m => m.Number1, m => m.Random.Int(0, 10)) // 0 <= x <= 10
        .RuleFor(m => m.Number2, m => m.Random.Int(0, 10)) // 0 <= x <= 10
        .RuleFor(m => m.Number3, m => m.Random.Int(0, 10)) // 0 <= x <= 10
        .RuleFor(m => m.DecimalNumber1, m => m.Random.Decimal(0, 10)) // 0 <= x <= 10
        .RuleFor(m => m.DecimalNumber2, m => m.Random.Decimal(0, 10)) // 0 <= x <= 10
        .RuleFor(m => m.DecimalNumber3, m => m.Random.Decimal(0, 10)) // 0 <= x <= 10
                                                                      // IsValidTestString
        .RuleFor(m => m.StringCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.String2(100, Chars.HexLowerCase)).ToList())
        // 0 <= x <= 10_000
        .RuleFor(m => m.IntCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.Number(0, 10_000)).ToList())
        // 0 <= x <= 10_000
        .RuleFor(m => m.DecimalCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.Decimal(0, 10_000)).ToList())
        .RuleFor(h => h.NestedModel1, () => CreateNestedWithNoErrors());


    public static BigModel CreateWithManyErrors() => new Faker<BigModel>()
        .RuleFor(m => m.Text1, f => f.Lorem.Word().OrNull(f, 0.1f))
        .RuleFor(m => m.Text2, f => f.Lorem.Word().OrNull(f, 0.15f))
        .RuleFor(m => m.Text3, f => f.Lorem.Word().OrNull(f, 0.2f))
        .RuleFor(m => m.Text4, f => f.Lorem.Word().OrNull(f, 0.25f))
        .RuleFor(m => m.Text5, f => f.Lorem.Word().OrNull(f, 0.3f))
        .RuleFor(m => m.Number1, f => f.Random.Int(0, 10 + 10))
        .RuleFor(m => m.Number2, f => f.Random.Int(0, 10 + 10))
        .RuleFor(m => m.Number3, f => f.Random.Int(0, 10 + 10))
        .RuleFor(m => m.DecimalNumber1, f => f.Random.Decimal(0, 10 + 10).OrNull(f, 0.20f))
        .RuleFor(m => m.DecimalNumber2, f => f.Random.Decimal(0, 10 + 10).OrNull(f, 0.25f))
        .RuleFor(m => m.DecimalNumber2, f => f.Random.Decimal(0, 10 + 10).OrNull(f, 0.30f))
        .RuleFor(m => m.StringCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.String2(100, Chars.AlphaNumericLowerCase)).ToList())
        .RuleFor(m => m.IntCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.Number(-10_000, 10_000)).ToList())
        .RuleFor(m => m.DecimalCollection, f => Enumerable.Range(1, 100).Select(_ => f.Random.Decimal(-10_000, 10_000)).ToList())
        .RuleFor(h => h.NestedModel1, () => CreateNestedWithManyErrors());

    public static Func<string, bool> IsValidStringInCollection => (s) => s.All(ch => Chars.HexLowerCase.Contains(ch));
    public static Func<int, bool> IsValidIntInCollection => (x) => x >= 0 && x <= 10_000;
    public static Func<decimal, bool> IsValidDecimalInCollection => (x) => x >= 0 && x <= 10_000;
}

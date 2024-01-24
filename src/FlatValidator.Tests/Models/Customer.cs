namespace FlatValidatorBenchmarks.Models;

public record class Customer(int Id, string Name)
{
    public static Customer[] Samples = [
        new(0, "With incorrect Id"),
        new(1, "Microsoft"),
        new(2, "Apple"),
        new(3, "AbracadabrA"),
        new(4, "Something@Else"),
        new(5, null!)
    ];
};


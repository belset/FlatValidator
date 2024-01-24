using System.Validation;

using FlatValidator.Profiling;

internal class Program
{
    static void Main(string[] args)
    {
        ProfilingModel profilingModel = new ProfilingModel();

        for (int i = 0; i < 100000; i++)
        {
            var validator = new ProfilingFlatValidator();
            var result = validator.Validate(profilingModel);
            if (!result)
            {
                var ret1 = result.ToDictionary();
                var ret2 = result.Errors.ToValidationResults();
            }
        }

        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}

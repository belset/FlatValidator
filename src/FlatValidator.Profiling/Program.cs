using System.Validation;

using FlatValidator.Profiling;

internal class Program
{
    static void Main(string[] args)
    {
        bool abortProcess = false;
        Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) => e.Cancel = abortProcess = true;

        ProfilingModel profilingModel = new ProfilingModel();

        Console.Write("Working...");
        for (int i = 0; !abortProcess && i < 500000; i++)
        {
            var validator = new ProfilingFlatValidator();
            var result = validator.Validate(profilingModel);
            if (!result)
            {
                var ret1 = result.ToDictionary();
                var ret2 = result.Errors.ToValidationResults();
            }
            if (i % 10000 == 0)
            {
                Console.Write(".");
            }
        }

        if (!abortProcess)
        {
            Console.WriteLine("\r\nPress any key...");
            Console.ReadKey();
        }
    }
}

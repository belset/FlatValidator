using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using FlatValidatorBenchmarks;

try
{
    //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, 
    //    DefaultConfig.Instance
    //        .WithOption(ConfigOptions.DisableLogFile, false)
    //        .WithOption(ConfigOptions.KeepBenchmarkFiles, false)
    //        .WithOption(ConfigOptions.JoinSummary, true)
    //);

    BenchmarkRunner.Run<BenchmarkWithNoErrors>(
        DefaultConfig.Instance
            .WithOption(ConfigOptions.DisableLogFile, false)
            .WithOption(ConfigOptions.KeepBenchmarkFiles, false)
            .WithOption(ConfigOptions.JoinSummary, true)
    );

    Console.WriteLine();
    Console.Write("Press any key to continue...");
    Console.ReadKey();

    BenchmarkRunner.Run<BenchmarkWithManyErrors>(
        DefaultConfig.Instance
            .WithOption(ConfigOptions.DisableLogFile, false)
            .WithOption(ConfigOptions.KeepBenchmarkFiles, false)
            .WithOption(ConfigOptions.JoinSummary, true)
    );
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    Console.WriteLine(ex);
}

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.Write("Press any key to finish...");
Console.ReadKey();


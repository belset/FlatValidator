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
            .HideColumns("Gen0", "Gen1", "Gen2", "RatioSD")
            .WithOption(ConfigOptions.DisableLogFile, false)
            .WithOption(ConfigOptions.KeepBenchmarkFiles, false)
            .WithOption(ConfigOptions.JoinSummary, true)
#if DEBUG
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
#endif
    );

    Console.WriteLine();
    Console.Write("Press any key to continue...");
    Console.ReadKey();

    BenchmarkRunner.Run<BenchmarkWithManyErrors>(
        DefaultConfig.Instance
            .HideColumns("Gen0", "Gen1", "Gen2", "RatioSD")
            .WithOption(ConfigOptions.DisableLogFile, false)
            .WithOption(ConfigOptions.KeepBenchmarkFiles, false)
            .WithOption(ConfigOptions.JoinSummary, true)
#if DEBUG
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
#endif
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


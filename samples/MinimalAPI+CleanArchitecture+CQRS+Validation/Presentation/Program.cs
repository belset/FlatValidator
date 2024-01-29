using Presentation._Startup;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args).ConfigureApplicationBuilder();
    var app = builder.Build().ConfigureApplication();

    app.DatabaseInitialization();

    Log.Information("Starting host...");
    app.Run();
    Log.Information("Host stopped.");

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

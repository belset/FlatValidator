using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Application.Common.Services;

using Presentation.Endpoints;

using Serilog;


namespace Presentation._Startup;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        #region Swagger

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                $"MinimalAPI - {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

        #endregion Swagger

        #region MinimalApi

        _ = app.MapBrandEndpoints();
        _ = app.MapStoreEndpoints();
        _ = app.MapProductEndpoints();

        #endregion MinimalApi

        return app;
    }

    public static WebApplication DatabaseInitialization(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<IDataInitialization>();
            context.Initialize();
        }
        return app;
    }
}


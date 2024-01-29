using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Application._Startup;

using Infrastructure._Startup;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

using Serilog;


namespace Presentation._Startup;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
    {
        #region Logging

        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            var assembly = Assembly.GetEntryAssembly();

            _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                .Enrich.WithProperty(
                    "Assembly Version",
                    assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                .Enrich.WithProperty(
                    "Assembly Informational Version",
                    assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        });

        #endregion Logging

        #region Serialization

        builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        #endregion // Serialization

        #region Swagger

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"MinimalAPI+Clean+CQRS+Validation - {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(builder.Environment.EnvironmentName)}",
                    Description = "FlatValidator usage example.",
                    TermsOfService = new Uri("https://github.com/belset/FlatValidator")
                });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.DocInclusionPredicate((name, api) => true);
        });

        #endregion // Swagger

        #region Project Dependencies

        _ = builder.Services.AddDatabase();
        _ = builder.Services.AddApplication();

        #endregion // Project Dependencies

        return builder;
    }
}

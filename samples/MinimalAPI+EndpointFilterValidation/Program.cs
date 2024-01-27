using System.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFlatValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseHttpsRedirection();

app.MapPost("/", (RegisterRequest entity, CancellationToken cancellationToken) =>
{
    return TypedResults.Ok(entity);

})
.AddEndpointFilter<ValidationFilter<RegisterRequest>>()
.WithName("FlatValidator")
.WithOpenApi();

app.Run();

public record class RegisterRequest(string EmailOrUsername, string Password);

/// <summary>
/// It was registered above - builder.Services.AddFlatValidatorsFromAssembly()
/// </summary>
public class RegisterRequestValidator : FlatValidator<RegisterRequest>
{
    public RegisterRequestValidator(EmailService emailService)
    {
        Grouped(model => model.EmailOrUsername.IsEmail(), model =>
        {
            ErrorIf(model => emailService.EmailExists(model),
                    model => $"Email {model.EmailOrUsername} has already been registered.",
                    model => model.EmailOrUsername);
        });

        ValidIf(model => model.Password.IsPassword(5),
                "Must be at least 5 symbols with lower and upper case letter, digits, special symbols - @#$%^&+=", 
                model => model.Password);
    }
}

/// <summary>
/// It was registered above - builder.Services.AddScoped<EmailService>() 
/// </summary>
public class EmailService
{
    public ValueTask<bool> EmailExists(RegisterRequest entity) => ValueTask.FromResult(true);
}
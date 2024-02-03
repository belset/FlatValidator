
/// <summary>
/// It is an emulation of some service with custom business logic.
/// Registed in IServiceCollection: builder.Services.AddScoped<EmailService>() 
/// </summary>
public interface IEmailService
{
    ValueTask<bool> EmailExists(RegisterRequest entity);
}

/// <summary>
/// It is an emulation of some service with custom business logic.
/// Registed in IServiceCollection: builder.Services.AddScoped<EmailService>() 
/// </summary>
public class EmailService : IEmailService
{
    public ValueTask<bool> EmailExists(RegisterRequest entity)
        => ValueTask.FromResult(entity.EmailOrUsername.Length % 2 == 0);
}

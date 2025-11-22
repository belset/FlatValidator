
/// <summary>
/// It is an emulation of some service with custom business logic.
/// Registed in IServiceCollection: builder.Services.AddScoped<EmailService>() 
/// </summary>
public interface IEmailService
{
    ValueTask<bool> IsGmailComEmail(RegisterRequest entity);
}

/// <summary>
/// It is an emulation of some service with custom business logic.
/// Registed in IServiceCollection: builder.Services.AddScoped<EmailService>() 
/// </summary>
public class EmailService : IEmailService
{
    public ValueTask<bool> IsGmailComEmail(RegisterRequest entity)
        => ValueTask.FromResult(entity.Email.EndsWith("gmail.com"));
}

using System.Validation;

public record class RegisterRequest(string Email, string Password)
{
    // We can define a validator inside of the model class,
    // no necessity for that but it's also possible.
    public class RegisterRequestValidator : FlatValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IEmailService emailService)
        {
            When(model => model.Email.IsEmail(), @then: model =>
            {
                ValidIf(model => emailService.IsGmailComEmail(model),
                        model => $"Email '{model.Email}' has not registered on gmail.com. Use a 'xxx@gmail.com' email instead.",
                        model => model.Email);
            }, @else: model => 
            {
                Error($"Email '{model.Email}' is invalid.", model => model.Email);
            });

            ValidIf(model => model.Password.IsPassword(10),
                    "Must be at least 10 symbols with lower and upper case letter, digits, special symbols - @#$%^&+=",
                    model => model.Password);
        }
    }
}


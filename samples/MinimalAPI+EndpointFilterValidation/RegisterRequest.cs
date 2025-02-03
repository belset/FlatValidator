using System.Validation;

public record class RegisterRequest(string EmailOrUsername, string Password)
{
    // We can define a validator inside of the model class,
    // no necessity for that but it's also possible.
    public class RegisterRequestValidator : FlatValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IEmailService emailService)
        {
            When(model => model.EmailOrUsername.IsEmail(), model =>
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
}


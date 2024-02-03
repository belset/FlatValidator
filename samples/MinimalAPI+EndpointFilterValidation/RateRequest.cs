using System.Validation;

public record class RateRequest(Guid RateId, string Metadata)
{
    // We can define a validator inside of the model class,
    // no necessity for that but it's also possible.
    public class RateRequestValidator : FlatValidator<RateRequest>
    {
        public RateRequestValidator(IHttpContextAccessor httpContextAccessor)
        {
            // we can change the validation flow in accordance with HTTP-method
            var method = httpContextAccessor.HttpContext?.Request?.Method;

            if (method == HttpMethods.Post)
            {
                ValidIf(m => m.RateId == Guid.Empty,
                        m => $"Bad RateID ({m.RateId}) for POST method.",
                        m => m.RateId);
            }
            else if (method == HttpMethods.Put)
            {
                ErrorIf(m => m.RateId == Guid.Empty,
                        m => $"Bad RateID ({m.RateId}) for PUT method.",
                        m => m.RateId);
            }
        }
    }
}

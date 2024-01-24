using FlatValidatorBenchmarks.Models;
using FluentValidation;

public class FluentValidatorForBigModel : AbstractValidator<BigModel>
{
    public FluentValidatorForBigModel()
    {
        RuleFor(x => x.Text1).NotNull();
        RuleFor(x => x.Text1).Must(t => t.Contains('a', StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Message T1").When(m => m.Text1 != null);

        RuleFor(x => x.Text2).NotNull();
        RuleFor(x => x.Text2).Must(t => t.Contains('b', StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Message T2").When(m => m.Text2 != null);

        RuleFor(x => x.Text3).NotNull();
        RuleFor(x => x.Text3).Must(t => t.Contains('c', StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Message T3").When(m => m.Text3 != null);

        RuleFor(x => x.Text4).NotNull();
        RuleFor(x => x.Text4).Must(t => t.Contains('d', StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Message T4").When(m => m.Text4 != null);

        RuleFor(x => x.Text5).NotNull();
        RuleFor(x => x.Text5).Must(t => t.Contains('e', StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Message T5").When(m => m.Text5 != null);

        //
        RuleFor(x => x.Number1).Must(m => m <= 10).WithMessage("Message N1");
        RuleFor(x => x.Number2).Must(m => m <= 10).WithMessage("Message N2");
        RuleFor(x => x.Number3).Must(m => m <= 10).WithMessage("Message N3");

        //
        RuleFor(x => x.DecimalNumber1).NotNull();
        RuleFor(x => x.DecimalNumber1).Must(m => m <= 10).WithMessage("Message S1").When(m => m.DecimalNumber1 != null);

        RuleFor(x => x.DecimalNumber2).NotNull();
        RuleFor(x => x.DecimalNumber2).Must(m => m <= 10).WithMessage("Message S2").When(m => m.DecimalNumber2 != null);

        RuleFor(x => x.DecimalNumber3).NotNull();
        RuleFor(x => x.DecimalNumber3).Must(m => m <= 10).WithMessage("Message S3").When(m => m.DecimalNumber3 != null);

        //
        RuleFor(x => x.StringCollection).NotNull();
        RuleFor(x => x.StringCollection).Must(coll => coll.All(BigModel.IsValidStringInCollection))
            .WithMessage("String collection").When(m => m.StringCollection != null);

        RuleFor(x => x.StringCollection).NotNull();
        RuleFor(x => x.IntCollection).Must(coll => coll.All(BigModel.IsValidIntInCollection))
            .WithMessage("Int collection").When(m => m.IntCollection != null);

        RuleFor(x => x.DecimalCollection).NotNull();
        RuleFor(x => x.DecimalCollection).Must(coll => coll.All(BigModel.IsValidDecimalInCollection))
            .WithMessage("Decimal collection").When(m => m.DecimalCollection != null);
    }
}


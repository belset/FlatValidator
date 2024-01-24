using System.Validation;
using FlatValidatorBenchmarks.Models;

public class FleeValidatorForBigModel : FleeValidator<BigModel>
{
    public FleeValidatorForBigModel()
    {
        ErrorIf(m => m.Text1 is null, "Text1 cannot be null", m => m.Text1);
        ValidIf(m => m.Text1 is not null && m.Text1.Contains('a', StringComparison.InvariantCultureIgnoreCase), "Message T1", m => m.Text1);

        ErrorIf(m => m.Text2 is null, "Text2 cannot be null", m => m.Text2);
        ValidIf(m => m.Text2 is not null && m.Text2.Contains('b', StringComparison.InvariantCultureIgnoreCase), "Message T2", m => m.Text2);

        ErrorIf(m => m.Text3 is null, "Text3 cannot be null", m => m.Text3);
        ValidIf(m => m.Text3 is not null && m.Text3.Contains('c', StringComparison.InvariantCultureIgnoreCase), "Message T3", m => m.Text3);

        ErrorIf(m => m.Text4 is null, "Text4 cannot be null", m => m.Text4);
        ValidIf(m => m.Text4 is not null && m.Text4.Contains('d', StringComparison.InvariantCultureIgnoreCase), "Message T4", m => m.Text4);

        ErrorIf(m => m.Text5 is null, "Text5 cannot be null", m => m.Text5);
        ValidIf(m => m.Text5 is not null && m.Text5.Contains('e', StringComparison.InvariantCultureIgnoreCase), "Message T5", m => m.Text5);

        //
        ErrorIf(m => m.Number1 > 10, "Message N1", m => m.Number1);
        ErrorIf(m => m.Number2 > 10, "Message N2", m => m.Number2);
        ErrorIf(m => m.Number3 > 10, "Message N3", m => m.Number3);

        //
        ErrorIf(m => m.DecimalNumber1 is null, "S1 cannot be null", m => m.DecimalNumber1);
        ValidIf(m => m.DecimalNumber1 is not null && m.DecimalNumber1 <= 10, "Message S1", m => m.DecimalNumber1);

        ErrorIf(m => m.DecimalNumber2 is null, "S2 cannot be null", m => m.DecimalNumber2);
        ValidIf(m => m.DecimalNumber2 is not null && m.DecimalNumber2 <= 10, "Message S2", m => m.DecimalNumber2);

        ErrorIf(m => m.DecimalNumber3 is null, "S3 cannot be null", m => m.DecimalNumber3);
        ValidIf(m => m.DecimalNumber3 is not null && m.DecimalNumber3 <= 10, "Message S3", m => m.DecimalNumber3);

        //
        ErrorIf(m => m.StringCollection is null, "String collection cannot be null", m => m.StringCollection);
        ValidIf(m => m.StringCollection is not null && m.StringCollection.All(BigModel.IsValidStringInCollection), "String collection", m => m.StringCollection);

        ErrorIf(m => m.IntCollection is null, "Int collection cannot be null", m => m.IntCollection);
        ValidIf(m => m.IntCollection is not null && m.IntCollection.All(BigModel.IsValidIntInCollection), "Int collection", m => m.IntCollection);

        ErrorIf(m => m.DecimalCollection is null, "Decimal collection cannot be null", m => m.DecimalCollection);
        ValidIf(m => m.DecimalCollection is not null && m.DecimalCollection.All(BigModel.IsValidDecimalInCollection), "Decimal collection", m => m.DecimalCollection);

        //
        ErrorIf(m => m.NestedModel1.NestedText1 is null, "NestedText1 cannot be null", m => m.NestedModel1.NestedText1);
        ValidIf(m => m.NestedModel1.NestedText1 is not null && m.NestedModel1.NestedText1.StartsWith("NestedText1:"), "NestedText1 is not valid", m => m.NestedModel1.NestedText1);

        ErrorIf(m => m.NestedModel1.NestedText2 is null, "NestedText2 cannot be null", m => m.NestedModel1.NestedText2);
        ValidIf(m => m.NestedModel1.NestedText2 is not null && m.NestedModel1.NestedText2.StartsWith("NestedText2:"), "NestedText2 is not valid", m => m.NestedModel1.NestedText1);
    }
}


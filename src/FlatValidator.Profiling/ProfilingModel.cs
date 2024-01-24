namespace FlatValidator.Profiling;

public class ProfilingModel
{
    public class NestedModel
    {
        public string NestedText1 { get; set; } = "NestedText1";
        public string NestedText2 { get; set; } = "NestedText2";
    }

    public string Text1 { get; set; } = "Text1";
    public string Text2 { get; set; } = "Text2";
    public string Text3 { get; set; } = "Text3";
    public string Text4 { get; set; } = "Text4";
    public string Text5 { get; set; } = "Text5";

    public int Number1 { get; set; } = 100;
    public int Number2 { get; set; } = 101;
    public int Number3 { get; set; } = 102;

    public decimal? DecimalNumber1 { get; set; } = new decimal(0.100);
    public decimal? DecimalNumber2 { get; set; } = new decimal(0.101);
    public decimal? DecimalNumber3 { get; set; } = null;

    public NestedModel NestedModel1 { get; set; } = new();
}

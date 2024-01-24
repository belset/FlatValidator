using System.Linq.Expressions;
using System.Validation;

namespace FlatValidatorTests;


public class ExressionToStringTests
{
    public record class NestedModel(
        int Id, 
        string Name, 
        DateTime Date
    );
    public class BaseModel
    { 
        public int Id { get; set; }
        public required string FullName { get; set; }
        public DateTime Date { get; set; }
        
        public int[] Ints { get; set; } = { 1, 2, 3, 4, 5 };
        public Dictionary<string, string> Dict = new() { { "key", "value" } };

        public required NestedModel Nested { get; set; }
        public NestedModel GetNested() => Nested;
    }


    [Fact]
    public void _01_Base_Valued()
    {
        Expression<Func<BaseModel, int>> expression = x => x.Id;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == nameof(BaseModel.Id));
    }

    [Fact]
    public void _02_Base_Referenced()
    {
        Expression<Func<BaseModel, string>> expression = x => x.FullName;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == nameof(BaseModel.FullName));
    }

    [Fact]
    public void _03_Base_Struct()
    {
        Expression<Func<BaseModel, DateTime>> expression = x => x.Date;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == nameof(BaseModel.Date));
    }

    [Fact]
    public void _04_Nested_Valued_via_Property()
    {
        Expression<Func<BaseModel, int>> expression = x => x.Nested.Id;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == $"{nameof(BaseModel.Nested)}.{nameof(NestedModel.Id)}");
    }

    [Fact]
    public void _05_Nested_References_via_Property()
    {
        Expression<Func<BaseModel, string>> expression = x => x.Nested.Name;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == $"{nameof(BaseModel.Nested)}.{nameof(NestedModel.Name)}");
    }

    [Fact]
    public void _06_Nested_Struct_via_Property()
    {
        Expression<Func<BaseModel, DateTime>> expression = x => x.Nested.Date;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == $"{nameof(BaseModel.Nested)}.{nameof(NestedModel.Date)}");
    }

    [Fact]
    public void _07_Nested_Valued_via_Method()
    {
        Expression<Func<BaseModel, int>> expression = x => x.GetNested().Id;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "GetNested().Id");
    }

    [Fact]
    public void _08_Nested_Referenced_via_Method()
    {
        Expression<Func<BaseModel, string>> expression = x => x.GetNested().Name;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "GetNested().Name");
    }

    [Fact]
    public void _09_Nested_Struct_via_Method()
    {
        Expression<Func<BaseModel, DateTime>> expression = x => x.GetNested().Date;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "GetNested().Date");
    }

    [Fact]
    public void _10_Base_Array_IntIndexer()
    {
        Expression<Func<BaseModel, int>> expression = x => x.Ints[0];
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "Ints[0]");
    }

    [Fact]
    public void _11_Base_Array_Length()
    {
        Expression<Func<BaseModel, int>> expression = x => x.Ints.Length;
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "Ints.Length");
    }

    [Fact]
    public void _12_Base_Dict_StringIndexer()
    {
        Expression<Func<BaseModel, string>> expression = x => x.Dict["key"];
        Assert.True(FlatValidatorExstensions.GetExpressionPath(expression) == "Dict.get_Item(\"key\")");
    }
}


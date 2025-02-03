using System.Text.Json.Serialization;
using System.Validation;

var sampleTodos = new List<Todo> {
    new(0, "Record with incorrect Id"),
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddProblemDetails().ConfigureHttpJsonOptions(options 
    => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

// register array of test models
builder.Services.AddSingleton(sampleTodos);

// register a validator for the Todo model
builder.Services.AddScoped<IFlatValidator<Todo>, TodoValidator>();

var app = builder.Build();

app.MapGet("/demo", () =>
{
    var ret = sampleTodos.Select(m => FlatValidator.Validate(m, v => // inline validation
    {
        v.ErrorIf(m => m.Id <= 0, $"Invalid Id", m => m.Id);
        v.ErrorIf(m => m.DueBy is null, "DueBy can not be null.", m => m.DueBy);

        v.When(m => m.Title.NotEmpty(), m =>
        {
            v.ValidIf(m => m.Title!.Contains('r'),
                      m => $"Title '{m.Title}' does not contain 'r'.",
                      m => m.Title);
        });
    })).ToDictionary();

    return TypedResults.ValidationProblem(ret);
});

app.MapGet("/todos", () => sampleTodos);

app.MapPost("/todos", (Todo todo) => sampleTodos.Add(todo))
   .AddEndpointFilter<ValidationFilter<Todo>>();

app.Run("http://localhost:5400");


// Model to test validation functionality
public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);


// TodoValidator will be called from ValidationFilter for the request 'POST:todos'
public class TodoValidator : FlatValidator<Todo>
{
    public TodoValidator(List<Todo> todos)
    {
        ErrorIf(m => m.Id <= 0, $"Invalid Id", m => m.Id);
        ErrorIf(m => m.DueBy is null, "DueBy can not be null.", m => m.DueBy);

        When(m => m.Title.NotEmpty(), @then: m =>
        {
            ValidIf(m => m.Title!.Contains('r'),
                      m => $"Title '{m.Title}' does not contain 'r'.",
                      m => m.Title);
        });

        ErrorIf(m => todos.Any(x => x.Id == m.Id), m => $"Id {m.Id} already exists.", m => m.Id);
    }
}


// NativeAOT supporting
[JsonSerializable(typeof(List<Todo>))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{ }
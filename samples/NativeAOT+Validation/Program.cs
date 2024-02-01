using System.Text.Json.Serialization;
using System.Validation;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddProblemDetails().ConfigureHttpJsonOptions(options 
    => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

var app = builder.Build();

var sampleTodos = new Todo[] {
    new(0, "Record with incorrect Id"),
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

app.MapGet("/todos", () => sampleTodos);

app.MapGet("/validate", () =>
{
    List<FlatValidationResult> results = new();

    foreach (var model in sampleTodos)
    {
        // no need to create any class outside, just define rules inside
        var result = FlatValidator.Validate(model, v =>
        {
            v.ErrorIf(m => m.Id <= 0, $"Invalid Id", m => m.Id);
            v.ErrorIf(m => m.DueBy is null, "DueBy can not be null.", m => m.DueBy);

            v.If(m => m.Title.NotEmpty(), @then: m =>
            {
                v.ValidIf(m => m.Title!.Contains('r'), 
                          m => $"Title '{m.Title}' does not contain 'r'.", 
                          m => m.Title);
            });
        });
        results.Add(result);
    }
    var groupedProblems = results.ToDictionary(); // group each model results into one collection
    return TypedResults.ValidationProblem(groupedProblems);

/* // same logic but a bit shortly

    var ret = sampleTodos.Select(m => FlatValidator.Validate(m, v =>
    {
        v.ErrorIf(m => m.Id <= 0, $"Invalid Id", m => m.Id);
        v.ErrorIf(m => m.DueBy is null, "DueBy can not be null.", m => m.DueBy);

        v.Grouped(m => m.Title.NotEmpty(), m =>
        {
            v.ValidIf(m => m.Title!.Contains('r'),
                      m => $"Title '{m.Title}' does not contain 'r'.",
                      m => m.Title);
        });
    })).ToDictionary());

    return TypedResults.ValidationProblem(ret);
*/
});

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{ }

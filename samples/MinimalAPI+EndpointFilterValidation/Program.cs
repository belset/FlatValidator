using System.Reflection;
using System.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddFlatValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// register example of custom service to use inside of validator
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseHttpsRedirection();

app.MapPost("/emails", (RegisterRequest entity, CancellationToken cancellationToken) =>
{
    return TypedResults.Ok(entity);
})
.AddEndpointFilter<ValidationFilter<RegisterRequest>>();

// The entity will be valid if entity.RateId is empty (look into RateRequest.cs)
app.MapPost("/rates", (RateRequest entity, CancellationToken cancellationToken) =>
{
    return TypedResults.Ok(entity);
})
.WithDescription("Must be equal to Guid.Empty")
.AddEndpointFilter<ValidationFilter<RateRequest>>();

// The entity will be valid if entity.RateId is NOT empty  (look into RateRequest.cs)
app.MapPut("/rates", (RateRequest entity, CancellationToken cancellationToken) =>
{
    return TypedResults.Ok(entity);
})
.WithDescription("Must NOT be equal to Guid.Empty")
.AddEndpointFilter<ValidationFilter<RateRequest>>();


app.Run();

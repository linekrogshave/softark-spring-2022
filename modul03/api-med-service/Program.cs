using Service;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS in the app.
// Learn more here: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Singleton since this data is shared between all requests.
// When using a database connection, use AddScoped instead.
builder.Services.AddSingleton<DataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // TODO: Add development stuff if needed
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware that runs before each request handler
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

// Request handlers
app.MapGet("/", (HttpContext context, DataService service) =>
{
    context.Response.ContentType = "text/html;charset=utf-8";
    return "Nothing to see here. Try instead: " + 
            "<a href=\"/api/tasks\">/api/tasks</a>";
});

app.MapGet("/api/tasks", (DataService service) =>
{
    return service.GetTasks();
});

app.MapGet("/api/tasks/{id}", (DataService service, int id) =>
{
    return service.GetTaskById(id);
});

app.MapPost("/api/tasks/", (TaskData data, DataService service) =>
{
    return service.CreateTask(data.text, data.done);
});

app.Run();

record TaskData(string text, bool done);
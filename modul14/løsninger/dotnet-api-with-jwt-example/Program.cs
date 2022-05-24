using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<TokenService>();
builder.Services.AddSingleton<LoginService>();

builder.Services.AddAuthentication("JwtAuthentication")
                .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("JwtAuthentication", null);
builder.Services.AddAuthorization(options => {
    options.AddPolicy("CakeLover", policy => policy.RequireClaim("Role", "CakeLover"));
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
});

var app = builder.Build();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapGet("/api/cake", [Authorize(Policy= "CakeLover")]
    () => {
        return "Hello Cake Lover!";
    });

app.MapGet("/api/admin", [Authorize(Policy= "Admin")]
    (HttpRequest request, TokenService tokenService) => {
        var authHeader = request.Headers["Authorization"].ToString();
        var token = authHeader.Substring("Bearer ".Length).Trim();
        var username = tokenService.GetUsername(token);
        return $"Hello Admin! ({username})";
    });    

app.MapGet("/api/hello", [AllowAnonymous]
    () => "Hello World!");

app.MapPost("/api/users/login", [AllowAnonymous]
    (LoginService loginService, TokenService tokenService, LoginData data) => {
        var valid = loginService.ValidateLogin(data.username, data.password);
        if (valid) {
            var roles = loginService.GetRoles(data.username);
            var token = tokenService.GenerateToken(data.username, roles);
            return Results.Json(new { msg = "Login succeded", token = token}, statusCode: 200);
        } else {
            return Results.Json(new { msg = "Login failed" }, statusCode: 401);
        }
    });

app.MapPost("/api/users/", [AllowAnonymous]
    (LoginService loginService, TokenService tokenService, LoginData data) => {
        if (String.IsNullOrEmpty(data.username) || String.IsNullOrEmpty(data.password)) {
            return Results.Json(new { msg = "Password or username missing" }, statusCode: 400);
        }
        LoginService.UserRecord user = loginService.CreateLogin(data.username, data.password, 
            new string[] {"Admin", "CakeLover", "Noob"});
        return Results.Json(new { msg = "Login created", user = data.username});
    });

Console.WriteLine("App starting up!");

app.Run();

record LoginData(string username, string password);




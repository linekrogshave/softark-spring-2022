using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using todolist_blazor_app;
using todolist_blazor_app.Model;
using todolist_blazor_app.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<TodoListService>();
builder.Services.AddSingleton(
  sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

await builder.Build().RunAsync();

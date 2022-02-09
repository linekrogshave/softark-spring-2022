// TodoListService.cs
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using todolist_blazor_app.Model;

// Tjek i Program.cs hvad dit namespace bør hedde
namespace todolist_blazor_app.Service;

public class TodoListService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public TodoListService(HttpClient http, IConfiguration configuration) {
        this.http = http;
        this.configuration = configuration;
        // Denne konfiguration læses fra filen "appsettings.json". Se mere i opgave 5.
        this.baseAPI = configuration["base_api"];
    }

    public async Task<TodoListTask[]> GetTodoListTasks()
    {
        string url = $"{baseAPI}tasks/";
        //string url = "https://krdo-todo.azurewebsites.net/api/tasks/";
        return await http.GetFromJsonAsync<TodoListTask[]>(url);
    }
}
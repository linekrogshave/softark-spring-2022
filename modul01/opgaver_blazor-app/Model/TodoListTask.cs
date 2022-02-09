namespace todolist_blazor_app.Model;

public class TodoListTask
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public bool Done { get; set; }
}
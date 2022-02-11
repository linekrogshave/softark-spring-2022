namespace Model
{
    public class TodoTask
    {
        public TodoTask(int id, string text, bool done) {
            this.TodoTaskId = id;
            this.Text = text;
            this.Done = done;
        }

        public long TodoTaskId { get; set; }
        public string? Text { get; set; }
        public bool Done { get; set; }

        public override string ToString() {
            return $"{TodoTaskId}, {Text}, {Done}";
        }
    }
}
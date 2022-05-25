namespace ElprisApp.Models
{
    public class User
    {
        public User (string username, string token) {
            this.Username = username;
            this.Token = token;
        }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
namespace InventorySystem.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        private string _password;
        public string Role { get; private set; }

        public User(int id, string username, string password, string role)
        {
            Id = id;
            Username = username;
            _password = password;
            Role = role;
        }

        //Encapsulation
        public bool ValidatePassword(string password)
        {
            return _password == password;
        }

        public override string ToString()
        {
            return $"[{Id}] {Username} ({Role})";
        }
    }
}

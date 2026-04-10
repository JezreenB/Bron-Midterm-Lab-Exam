namespace InventorySystem.Models
{
    public class Supplier
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ContactNumber { get; private set; }
        public string Email { get; private set; }

        public Supplier(int id, string name, string contactNumber, string email)
        {
            Id = id;
            Name = name;
            ContactNumber = contactNumber;
            Email = email;
        }

        public void Update(string name, string contactNumber, string email)
        {
            Name = name;
            ContactNumber = contactNumber;
            Email = email;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} | Contact: {ContactNumber} | Email: {Email}";
        }
    }
}

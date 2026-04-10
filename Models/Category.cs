namespace InventorySystem.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Description}";
        }
    }
}

namespace InventorySystem.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public int LowStockThreshold { get; private set; }
        public int CategoryId { get; private set; }
        public int SupplierId { get; private set; }

        public Product(int id, string name, string description, decimal price,
                       int stock, int lowStockThreshold, int categoryId, int supplierId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            LowStockThreshold = lowStockThreshold;
            CategoryId = categoryId;
            SupplierId = supplierId;
        }

        public void Update(string name, string description, decimal price, int lowStockThreshold, int categoryId, int supplierId)
        {
            Name = name;
            Description = description;
            Price = price;
            LowStockThreshold = lowStockThreshold;
            CategoryId = categoryId;
            SupplierId = supplierId;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            Stock += quantity;
        }

        public void DeductStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            if (quantity > Stock) throw new InvalidOperationException($"Insufficient stock. Available: {Stock}");
            Stock -= quantity;
        }

        public bool IsLowStock()
        {
            return Stock <= LowStockThreshold;
        }

        public decimal TotalValue()
        {
            return Price * Stock;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} | Price: {Price:C} | Stock: {Stock} | Category ID: {CategoryId} | Supplier ID: {SupplierId}";
        }
    }
}

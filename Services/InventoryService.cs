using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class InventoryService
    {
        private List<Product> _products = new List<Product>();
        private List<Category> _categories = new List<Category>();
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<User> _users = new List<User>();
        private List<TransactionRecord> _transactions = new List<TransactionRecord>();

        private int _nextProductId = 1;
        private int _nextCategoryId = 1;
        private int _nextSupplierId = 1;
        private int _nextUserId = 1;
        private int _nextTransactionId = 1;

        private User? _currentUser;

        public InventoryService()
        {
            SeedData();
        }

        private void SeedData()
        {
            _users.Add(new User(_nextUserId++, "admin", "admin123", "Admin"));
            _users.Add(new User(_nextUserId++, "staff", "staff123", "Staff"));

            _categories.Add(new Category(_nextCategoryId++, "Electronics", "Electronic gadgets and devices"));
            _categories.Add(new Category(_nextCategoryId++, "Office Supplies", "Stationery and office tools"));

            _suppliers.Add(new Supplier(_nextSupplierId++, "TechWorld", "09171234567", "techworld@email.com"));
            _suppliers.Add(new Supplier(_nextSupplierId++, "OfficeHub", "09281234567", "officehub@email.com"));

            _products.Add(new Product(_nextProductId++, "Laptop", "14-inch laptop", 35000m, 10, 3, 1, 1));
            _products.Add(new Product(_nextProductId++, "Ballpen (Box)", "Box of 12 ballpens", 85m, 50, 10, 2, 2));
            _products.Add(new Product(_nextProductId++, "USB Flash Drive", "64GB USB 3.0", 350m, 2, 5, 1, 1));
        }

        //AUTHENTICATION

        public bool Login(string username, string password)
        {
            var user = _users.Find(u => u.Username == username);
            if (user != null && user.ValidatePassword(password))
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        public void Logout() => _currentUser = null;

        public string GetCurrentUsername()
        {
            string name = _currentUser?.Username ?? "Unknown";
            return char.ToUpper(name[0]) + name.Substring(1);
        }

        //CATEGORIES

        public void AddCategory(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name cannot be empty.");
            _categories.Add(new Category(_nextCategoryId++, name, description));
        }

        public List<Category> GetAllCategories() => _categories;

        public Category? GetCategoryById(int id) => _categories.Find(c => c.Id == id);

        //SUPPLIERS

        public void AddSupplier(string name, string contactNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Supplier name cannot be empty.");
            _suppliers.Add(new Supplier(_nextSupplierId++, name, contactNumber, email));
        }

        public List<Supplier> GetAllSuppliers() => _suppliers;

        public Supplier? GetSupplierById(int id) => _suppliers.Find(s => s.Id == id);

        //PRODUCTS

        public void AddProduct(string name, string description, decimal price,
                               int stock, int lowStockThreshold, int categoryId, int supplierId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty.");
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (stock < 0) throw new ArgumentException("Stock cannot be negative.");
            if (GetCategoryById(categoryId) == null) throw new ArgumentException("Invalid category ID.");
            if (GetSupplierById(supplierId) == null) throw new ArgumentException("Invalid supplier ID.");

            var product = new Product(_nextProductId++, name, description, price, stock, lowStockThreshold, categoryId, supplierId);
            _products.Add(product);

            LogTransaction(product.Id, product.Name, TransactionType.Added, stock, "Initial stock added.");
        }

        public List<Product> GetAllProducts() => _products;

        public Product? GetProductById(int id) => _products.Find(p => p.Id == id);

        public List<Product> SearchProducts(string keyword)
        {
            string kw = keyword.ToLower();
            return _products.FindAll(p =>
                p.Name.ToLower().Contains(kw) ||
                p.Description.ToLower().Contains(kw));
        }

        public void UpdateProduct(int id, string name, string description, decimal price,
                                  int lowStockThreshold, int categoryId, int supplierId)
        {
            var product = GetProductById(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty.");
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (GetCategoryById(categoryId) == null) throw new ArgumentException("Invalid category ID.");
            if (GetSupplierById(supplierId) == null) throw new ArgumentException("Invalid supplier ID.");

            product.Update(name, description, price, lowStockThreshold, categoryId, supplierId);
            LogTransaction(product.Id, product.Name, TransactionType.Updated, 0, "Product details updated.");
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            LogTransaction(product.Id, product.Name, TransactionType.Deleted, 0, "Product removed from inventory.");
            _products.Remove(product);
        }

        public void RestockProduct(int id, int quantity)
        {
            var product = GetProductById(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            product.AddStock(quantity);
            LogTransaction(product.Id, product.Name, TransactionType.Restock, quantity);
        }

        public void DeductStock(int id, int quantity)
        {
            var product = GetProductById(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            product.DeductStock(quantity);
            LogTransaction(product.Id, product.Name, TransactionType.Deduction, quantity);
        }

        public List<Product> GetLowStockProducts() => _products.FindAll(p => p.IsLowStock());

        public decimal GetTotalInventoryValue() => _products.Sum(p => p.TotalValue());

        //TRANSACTIONS

        private void LogTransaction(int productId, string productName,
                                    TransactionType type, int quantity, string notes = "")
        {
            _transactions.Add(new TransactionRecord(
                _nextTransactionId++,
                productId,
                productName,
                type,
                quantity,
                GetCurrentUsername(),
                notes
            ));
        }

        public List<TransactionRecord> GetAllTransactions() => _transactions;
    }
}

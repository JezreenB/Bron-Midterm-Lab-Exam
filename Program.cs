using InventorySystem.Services;

class Program
{
    private static InventoryService _service = new InventoryService();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "CLI Inventory Management System";

        if (!LoginScreen())
        {
            Console.WriteLine("\nExiting system.");
            return;
        }

        MainMenu();
    }

    //LOGIN

    static bool LoginScreen()
    {
        int attempts = 0;
        while (attempts < 3)
        {
            PrintHeader("LOGIN");
            Console.Write("  Username : ");
            string username = Console.ReadLine() ?? "";

            Console.Write("  Password : ");
            string password = ReadPassword();

            if (_service.Login(username, password))
            {
                PrintSuccess($"\nLogin successful!");
                PressAnyKey();
                return true;
            }

            attempts++;
            PrintError($"\nInvalid credentials. Attempts left: {3 - attempts}");
            PressAnyKey();
        }

        PrintError("\nToo many failed attempts. Access denied.");
        return false;
    }

    //MAIN MENU

    static void MainMenu()
    {
        while (true)
        {
            PrintHeader($"MAIN MENU  [ User: {_service.GetCurrentUsername()} ]");
            Console.WriteLine("  [1] Manage Categories");
            Console.WriteLine("  [2] Manage Suppliers");
            Console.WriteLine("  [3] Manage Products");
            Console.WriteLine("  [4] Stock Operations");
            Console.WriteLine("  [5] Reports & History");
            Console.WriteLine("  [0] Logout & Exit");
            PrintDivider();
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": CategoryMenu(); break;
                case "2": SupplierMenu(); break;
                case "3": ProductMenu(); break;
                case "4": StockMenu(); break;
                case "5": ReportsMenu(); break;
                case "0":
                    _service.Logout();
                    PrintSuccess("\n  Logged out successfully.");
                    return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    PressAnyKey();
                    break;
            }
        }
    }

    //CATEGORY MENU

    static void CategoryMenu()
    {
        while (true)
        {
            PrintHeader("MANAGE CATEGORIES");

            Console.WriteLine("  [1] Add Category    [0] Return");
            Console.WriteLine("  ────────────────────────────────────────────────────────────────────────");

            var categories = _service.GetAllCategories();
            if (categories.Count == 0)
            {
                Console.WriteLine("  No categories found.");
            }
            else
            {
                Console.WriteLine("  Current Categories:\n");
                foreach (var c in categories)
                    Console.WriteLine($"  {c}");
            }

            Console.WriteLine("  ────────────────────────────────────────────────────────────────────────");
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddCategory(); break;
                case "0": return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    static void AddCategory()
    {
        PrintHeader("ADD CATEGORY");
        try
        {
            Console.Write("  Name        : ");
            string name = Console.ReadLine() ?? "";

            Console.Write("  Description : ");
            string desc = Console.ReadLine() ?? "";

            _service.AddCategory(name, desc);
            PrintSuccess("\n  Category added successfully!");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    //SUPPLIER MENU

    static void SupplierMenu()
    {
        while (true)
        {
            PrintHeader("MANAGE SUPPLIERS");

            Console.WriteLine("  [1] Add Supplier    [0] Return");
            Console.WriteLine("  ────────────────────────────────────────────────────────────────────────");

            var suppliers = _service.GetAllSuppliers();
            if (suppliers.Count == 0)
            {
                Console.WriteLine("  No suppliers found.");
            }
            else
            {
                Console.WriteLine("  Current Suppliers:\n");
                foreach (var s in suppliers)
                    Console.WriteLine($"  {s}");
            }

            Console.WriteLine("  ────────────────────────────────────────────────────────────────────────");
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddSupplier(); break;
                case "0": return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    static void AddSupplier()
    {
        PrintHeader("ADD SUPPLIER");
        try
        {
            Console.Write("  Name           : ");
            string name = Console.ReadLine() ?? "";

            Console.Write("  Contact Number : ");
            string contact = Console.ReadLine() ?? "";

            Console.Write("  Email          : ");
            string email = Console.ReadLine() ?? "";

            _service.AddSupplier(name, contact, email);
            PrintSuccess("\n  Supplier added successfully!");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }


    //PRODUCT MENU

    static void ProductMenu()
    {
        while (true)
        {
            PrintHeader("MANAGE PRODUCTS");

            Console.WriteLine("  [1] Add    [2] Search    [3] Update    [4] Delete    [0] Return");
            Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");

            var products = _service.GetAllProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("  No products found.");
            }
            else
            {
                Console.WriteLine($"  {"ID",-4} {"Name",-25} {"Price",14} {"Stock",8} {"Category",10} {"Supplier",10}");
                Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
                foreach (var p in products)
                {
                        string lowTag = p.IsLowStock() ? " LOW!" : "";
                        string price = "₱" + p.Price.ToString("N2");
                    Console.Write($"  {p.Id,-4} {p.Name,-25} {price,14} {p.Stock,8} {p.CategoryId,10} {p.SupplierId,10}");
                    if (p.IsLowStock())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("  LOW");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddProduct(); break;
                case "2": SearchProduct(); break;
                case "3": UpdateProduct(); break;
                case "4": DeleteProduct(); break;
                case "0": return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    static void AddProduct()
    {
        PrintHeader("ADD PRODUCT");
        try
        {
            ViewCategories_Inline();
            Console.Write("  Category ID    : ");
            int catId = ParseInt(Console.ReadLine());

            ViewSuppliers_Inline();
            Console.Write("  Supplier ID    : ");
            int supId = ParseInt(Console.ReadLine());

            Console.Write("  Name           : ");
            string name = Console.ReadLine() ?? "";

            Console.Write("  Description    : ");
            string desc = Console.ReadLine() ?? "";

            Console.Write("  Price (PhP)    : ");
            decimal price = ParseDecimal(Console.ReadLine());

            Console.Write("  Initial Stock  : ");
            int stock = ParseInt(Console.ReadLine());

            Console.Write("  Low Stock Alert: ");
            int lowStock = ParseInt(Console.ReadLine());

            _service.AddProduct(name, desc, price, stock, lowStock, catId, supId);
            PrintSuccess("\n  Product added successfully!");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    static void SearchProduct()
    {
        PrintHeader("SEARCH PRODUCT");
        Console.Write("  Enter keyword: ");
        string keyword = Console.ReadLine() ?? "";

        var results = _service.SearchProducts(keyword);
        Console.WriteLine();

        if (results.Count == 0)
        {
            Console.WriteLine("  No matching products found.");
        }
        else
        {
            Console.WriteLine($"  Found {results.Count} result(s):\n");
            foreach (var p in results)
                Console.WriteLine($"  {p}");
        }
        PressAnyKey();
    }

    static void UpdateProduct()
    {
        PrintHeader("UPDATE PRODUCT");
        try
        {
            ViewAllProducts_Inline();
            Console.Write("  Enter Product ID to update: ");
            int id = ParseInt(Console.ReadLine());

            var product = _service.GetProductById(id);
            if (product == null) { PrintError("  Product not found."); PressAnyKey(); return; }

            Console.WriteLine($"\n  Updating: {product.Name}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            Console.Write($"  Name [{product.Name}]: ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name)) name = product.Name;

            Console.Write($"  Description [{product.Description}]: ");
            string desc = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(desc)) desc = product.Description;

            Console.Write($"  Price [{product.Price}]: ");
            string priceInput = Console.ReadLine() ?? "";
            decimal price = string.IsNullOrWhiteSpace(priceInput) ? product.Price : ParseDecimal(priceInput);

            Console.Write($"  Low Stock Threshold [{product.LowStockThreshold}]: ");
            string lstInput = Console.ReadLine() ?? "";
            int lst = string.IsNullOrWhiteSpace(lstInput) ? product.LowStockThreshold : ParseInt(lstInput);

            ViewCategories_Inline();
            Console.Write($"  Category ID [{product.CategoryId}]: ");
            string catInput = Console.ReadLine() ?? "";
            int catId = string.IsNullOrWhiteSpace(catInput) ? product.CategoryId : ParseInt(catInput);

            ViewSuppliers_Inline();
            Console.Write($"  Supplier ID [{product.SupplierId}]: ");
            string supInput = Console.ReadLine() ?? "";
            int supId = string.IsNullOrWhiteSpace(supInput) ? product.SupplierId : ParseInt(supInput);

            _service.UpdateProduct(id, name, desc, price, lst, catId, supId);
            PrintSuccess("\n  Product updated successfully!");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    static void DeleteProduct()
    {
        PrintHeader("DELETE PRODUCT");
        try
        {
            ViewAllProducts_Inline();
            Console.Write("  Enter Product ID to delete: ");
            int id = ParseInt(Console.ReadLine());

            var product = _service.GetProductById(id);
            if (product == null) { PrintError("  Product not found."); PressAnyKey(); return; }

            Console.Write($"\n  Are you sure you want to delete '{product.Name}'? (yes/no): ");
            if (Console.ReadLine()?.Trim().ToLower() == "yes")
            {
                _service.DeleteProduct(id);
                PrintSuccess("  Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("  Deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    //STOCK MENU

    static void StockMenu()
    {
        while (true)
        {
            PrintHeader("STOCK OPERATIONS");
            Console.WriteLine("  [1] Restock a Product");
            Console.WriteLine("  [2] Deduct Stock (Sales/Use)");
            Console.WriteLine("  [0] Return");
            Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": RestockProduct(); break;
                case "2": DeductStock(); break;
                case "0": return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    static void RestockProduct()
    {
        PrintHeader("RESTOCK PRODUCT");
        try
        {
            ViewAllProducts_Inline();
            Console.Write("  Enter Product ID: ");
            int id = ParseInt(Console.ReadLine());

            Console.Write("  Quantity to add : ");
            int qty = ParseInt(Console.ReadLine());

            _service.RestockProduct(id, qty);
            PrintSuccess($"\n  Successfully restocked {qty} unit(s).");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    static void DeductStock()
    {
        PrintHeader("DEDUCT STOCK");
        try
        {
            ViewAllProducts_Inline();
            Console.Write("  Enter Product ID  : ");
            int id = ParseInt(Console.ReadLine());

            Console.Write("  Quantity to deduct: ");
            int qty = ParseInt(Console.ReadLine());

            _service.DeductStock(id, qty);
            PrintSuccess($"\n  Successfully deducted {qty} unit(s).");
        }
        catch (Exception ex)
        {
            PrintError($"\n  Error: {ex.Message}");
        }
        PressAnyKey();
    }

    //REPORTS MENU
    static void ReportsMenu()
    {
        while (true)
        {
            PrintHeader("REPORTS & HISTORY");
            Console.WriteLine("  [1] View Transaction History");
            Console.WriteLine("  [2] Show Low-Stock Items");
            Console.WriteLine("  [3] Total Inventory Value");
            Console.WriteLine("  [0] Return");
            PrintDivider();
            Console.Write("  Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ViewTransactionHistory(); break;
                case "2": ShowLowStockItems(); break;
                case "3": ShowTotalInventoryValue(); break;
                case "0": return;
                default:
                    PrintError("  Invalid option. Please try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    static void ViewTransactionHistory()
    {
        PrintHeader("TRANSACTION HISTORY");
        var transactions = _service.GetAllTransactions();

        if (transactions.Count == 0)
        {
            Console.WriteLine("  No transactions recorded.");
        }
        else
        {
            foreach (var t in transactions)
                Console.WriteLine($"  {t}");
        }
        PressAnyKey();
    }

    static void ShowLowStockItems()
    {
        PrintHeader("LOW-STOCK ITEMS");
        var items = _service.GetLowStockProducts();

        if (items.Count == 0)
        {
            PrintSuccess("  All products have sufficient stock.");
        }
        else
        {
            Console.WriteLine($"   {items.Count} item(s) are running low:\n");
            foreach (var p in items)
                Console.WriteLine($"  {p.Name,-20} | Stock: {p.Stock} | Alert at: {p.LowStockThreshold}");
        }
        PressAnyKey();
    }

    static void ShowTotalInventoryValue()
    {
        PrintHeader("TOTAL INVENTORY VALUE");
        decimal total = _service.GetTotalInventoryValue();

        Console.WriteLine("  Product-by-Product Breakdown:\n");
        Console.WriteLine($"  {"Name",-20} {"Price",10} {"Stock",6} {"Value",12}");
        Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");

        foreach (var p in _service.GetAllProducts())
            Console.WriteLine($"  {p.Name,-20} {"₱" + p.Price.ToString("N2"),12} {p.Stock,6} {p.TotalValue(),12:C}");

        Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine($"  {"TOTAL INVENTORY VALUE",-38} {total,12:C}");
        PressAnyKey();
    }

    //HELPERS

    static void ViewCategories_Inline()
    {
        Console.WriteLine("\n  Categories");
        foreach (var c in _service.GetAllCategories())
            Console.WriteLine($"  {c}");
        Console.WriteLine();
    }

    static void ViewSuppliers_Inline()
    {
        Console.WriteLine("\n  Suppliers");
        foreach (var s in _service.GetAllSuppliers())
            Console.WriteLine($"  {s}");
        Console.WriteLine();
    }

    static void ViewAllProducts_Inline()
    {
        Console.WriteLine();
        Console.WriteLine($"  {"ID",-4} {"Name",-25} {"Price",14} {"Stock",8} {"Category",10} {"Supplier",10}");
        Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
        foreach (var p in _service.GetAllProducts())
        {
            string price = "₱" + p.Price.ToString("N2");
            Console.Write($"  {p.Id,-4} {p.Name,-25} {price,14} {p.Stock,8} {p.CategoryId,10} {p.SupplierId,10}");
            if (p.IsLowStock())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  LOW");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine();
    }

    static int ParseInt(string? input)
    {
        if (!int.TryParse(input?.Trim(), out int result))
            throw new FormatException("Please enter a valid whole number.");
        return result;
    }

    static decimal ParseDecimal(string? input)
    {
        if (!decimal.TryParse(input?.Trim(), out decimal result))
            throw new FormatException("Please enter a valid number.");
        return result;
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(intercept: true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }

    static void PrintHeader(string title)
    {
        Console.Clear();
        Console.WriteLine();
        // Main title - bright white (bold effect)
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  ╔══════════════════════════════════════════╗");
        string appTitle = "CLI Inventory Management System";
        string centeredTitle = appTitle.PadLeft((40 + appTitle.Length) / 2).PadRight(40);
        Console.WriteLine($"  ║  {centeredTitle}║");
        Console.WriteLine("  ╚══════════════════════════════════════════╝ ");
        // Subtitle - cyan
        Console.WriteLine($"\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {title,-40}");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void PrintDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  ──────────────────────────────────────────");
        Console.ResetColor();
    }

    static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void PressAnyKey()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  Press any key to continue...");
        Console.ResetColor();
        Console.ReadKey();
    }
}

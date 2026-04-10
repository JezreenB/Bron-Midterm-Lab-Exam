namespace InventorySystem.Models
{
    public enum TransactionType
    {
        Restock,
        Deduction,
        Added,
        Deleted,
        Updated
    }

    public class TransactionRecord
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public TransactionType Type { get; private set; }
        public int Quantity { get; private set; }
        public string PerformedBy { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Notes { get; private set; }

        public TransactionRecord(int id, int productId, string productName,
                                 TransactionType type, int quantity,
                                 string performedBy, string notes = "")
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            Type = type;
            Quantity = quantity;
            PerformedBy = performedBy;
            Timestamp = DateTime.Now;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"[{Id}] {Timestamp:yyyy-MM-dd HH:mm:ss} | {Type,-10} | Product: {ProductName} | Qty: {Quantity,5} | By: {PerformedBy} | {Notes}";
        }
    }
}

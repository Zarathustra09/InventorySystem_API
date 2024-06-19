namespace InventorySystem_API.Models
{
    public class TransactionDto
    {
        public int? Transaction_Id { get; set; }
        public int Product_Id { get; set; }
        public string Transaction_Type { get; set; }
        public int Quantity { get; set; }
        public DateTime Transaction_Date { get; set; }
    }
}
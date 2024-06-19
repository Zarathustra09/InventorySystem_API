using System;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem_API.Models
{
    public class Product
    {
        [Key]
        public int? Product_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Product_Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public DateTime Updated_At { get; set; } = DateTime.UtcNow;
    }
}

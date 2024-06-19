using InventorySystem_API.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem_API.Models
{
    public class Transaction
    {
        [Key]
        public int? Transaction_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }

        [ForeignKey("Product_Id")]
        public Product Product { get; set; }

        [Required]
        [StringLength(3)]
        public string Transaction_Type { get; set; } // IN or OUT

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime Transaction_Date { get; set; } = DateTime.UtcNow;
    }
}

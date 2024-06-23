using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystem_API.DataConnection;
using InventorySystem_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InventorySystem_API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _context;

        public ProductController(DbContextClass context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new ProductDto
                {
                    Product_Id = p.Product_Id,
                    Product_Name = p.Product_Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Created_At = p.Created_At,
                    Updated_At = p.Updated_At
                })
                .ToListAsync();

            return products;
        }

        // GET: api/Products/5
  
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => p.Product_Id == id)
                .Select(p => new ProductDto
                {
                    Product_Id = p.Product_Id,
                    Product_Name = p.Product_Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Created_At = p.Created_At,
                    Updated_At = p.Updated_At
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Product_Name = productDto.Product_Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Product_Id }, productDto);
        }

        // PUT: api/Products/5
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Product_Id)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Update product properties
            product.Product_Name = productDto.Product_Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete related transactions first
            var transactions = _context.Transactions.Where(t => t.Product_Id == id);
            _context.Transactions.RemoveRange(transactions);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Product_Id == id);
        }
    }
}

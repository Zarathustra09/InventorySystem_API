using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystem_API.DataConnection;
using InventorySystem_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly DbContextClass _context;

        public TransactionController(DbContextClass context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var transactions = await _context.Transactions
                .Select(t => new TransactionDto
                {
                    Transaction_Id = t.Transaction_Id,
                    Product_Id = t.Product_Id,
                    Transaction_Type = t.Transaction_Type,
                    Quantity = t.Quantity,
                    Transaction_Date = t.Transaction_Date
                })
                .ToListAsync();

            return transactions;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions
                .Where(t => t.Transaction_Id == id)
                .Select(t => new TransactionDto
                {
                    Transaction_Id = t.Transaction_Id,
                    Product_Id = t.Product_Id,
                    Transaction_Type = t.Transaction_Type,
                    Quantity = t.Quantity,
                    Transaction_Date = t.Transaction_Date
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionDto transactionDto)
        {
            var transaction = new Transaction
            {
                Product_Id = transactionDto.Product_Id,
                Transaction_Type = transactionDto.Transaction_Type,
                Quantity = transactionDto.Quantity,
                Transaction_Date = transactionDto.Transaction_Date
            };

            // Check transaction type and update product quantity accordingly
            if (transaction.Transaction_Type == "IN")
            {
                // Increase product quantity
                var product = await _context.Products.FindAsync(transaction.Product_Id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                product.Quantity += transaction.Quantity;
            }
            else if (transaction.Transaction_Type == "OUT")
            {
                // Decrease product quantity
                var product = await _context.Products.FindAsync(transaction.Product_Id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                if (product.Quantity < transaction.Quantity)
                {
                    return BadRequest("Insufficient stock.");
                }

                product.Quantity -= transaction.Quantity;
            }
            else
            {
                return BadRequest("Invalid transaction type.");
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Transaction_Id }, transactionDto);
        }


        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, TransactionDto transactionDto)
        {
            if (id != transactionDto.Transaction_Id)
            {
                return BadRequest();
            }

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            // Update transaction properties
            transaction.Product_Id = transactionDto.Product_Id;
            transaction.Transaction_Type = transactionDto.Transaction_Type;
            transaction.Quantity = transactionDto.Quantity;
            transaction.Transaction_Date = transactionDto.Transaction_Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Transaction_Id == id);
        }
    }
}

using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Transaction transaction)
        {
            if(transaction is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productEntity = await _context.Products.FindAsync(transaction.ProductSKU);
            //check if its in stock
            if(productEntity.IsInStock is false) 
            {
                return BadRequest($"Product: {productEntity.Name} is currently out of stock");
            }

            //check for enough product in inventory to complete the transaction
            if(productEntity.NumberInInventory < transaction.ItemCount)
            {
                return BadRequest($"Not enough inventory of {productEntity.Name} to complete transaction.");
            }

            //remove product from inventory
            var newInventory = productEntity.NumberInInventory - transaction.ItemCount;
            productEntity.NumberInInventory = newInventory;

            transaction.DateOfTransaction = DateTime.Now;
            _context.Transactions.Add(transaction);

            if(await _context.SaveChangesAsync()>0)
            {
                return Ok($"Transaction: {transaction.Id} has been added to the database.");
            }

            return InternalServerError();
        }

        [HttpGet] 
        public async Task<IHttpActionResult> Get()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll([FromUri] int id)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(t => t.Id == id);
            
            if(transaction is null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
    }
}

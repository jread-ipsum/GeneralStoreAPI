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
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Product product)
        {
            if(product is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            if(await _context.SaveChangesAsync()>0)
            {
                return Ok($"Product: {product.Name} was added to the database.");
            }

            return InternalServerError();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/{Sku}")]
        public async Task<IHttpActionResult> GetById([FromUri] string sku)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.SKU == sku);

            if(product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}

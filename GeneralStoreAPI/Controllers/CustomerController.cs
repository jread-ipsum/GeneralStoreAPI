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
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Customer customer)
        {
            if(customer is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok($"Customer: {customer.FullName} was added to the database.");
            }

            return InternalServerError(); 
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if(customer is null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromUri] int id, [FromBody] Customer updatedCustomer)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            if(id != updatedCustomer?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = await _context.Customers.FindAsync(id);
            if(customer is null)
            {
                return NotFound();
            }

            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;

            if(await _context.SaveChangesAsync()>0)
            {
                return Ok("The customer was updated.");
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if(customer is null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);

            if(await _context.SaveChangesAsync()>0)
            {
                return Ok("The customer was deleted.");
            }

            return InternalServerError();
        }
    }
}

using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
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
            
            
             
        }
    }
}

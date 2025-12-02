using AIOS.Models;
using AIOS.Repositories;
using Microsoft.AspNetCore.Mvc;
using AIOS.Models;

namespace AIOS.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersApiController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersApiController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(string id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                    return NotFound(new { error = "Customer not found" });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] Customer customer)
        {
            try
            {
                var createdCustomer = await _customerRepository.CreateAsync(customer);
                return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Customer customer)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id);

                if (existingCustomer == null)
                    return NotFound(new { error = "Customer not found" });

                customer.Id = id;
                await _customerRepository.UpdateAsync(id, customer);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                    return NotFound(new { error = "Customer not found" });

                await _customerRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/customers/search?term=john&filter=name
        [HttpGet("search")]
        public async Task<ActionResult<List<Customer>>> Search([FromQuery] string term = "", [FromQuery] string filter = "all")
        {
            try
            {
                var customers = await _customerRepository.SearchAsync(term, filter);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
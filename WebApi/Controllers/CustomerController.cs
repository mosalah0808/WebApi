using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> _userRepository;
        public CustomerController(IRepository<Customer> userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _userRepository.GetAllAsync();
        }


        [HttpGet("{id}")]   
        public async Task<Customer?> GetCustomerAsync([FromRoute] long id)
        {
            return await _userRepository.GetAsync(id);
        }

        [HttpPost]   
        public async Task<Customer> CreateCustomerAsync([FromBody] Customer customer)
        {
            customer.Id = new long();
            await _userRepository.AddAsync(customer);
            return customer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Customer customer)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }

            await _userRepository.UpdateAsync(customer);
            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
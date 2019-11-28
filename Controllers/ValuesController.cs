using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Filter;
using WebApiCore.Models;
using WebApiCore.Helpers;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static List<User> _cachedUsers= new List<User>();

        public ValuesController()
        {
            _cachedUsers.InitUsersCachAsync().Wait();
        }

        // GET: api/Values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _cachedUsers;
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        [SimpleActionFilter]
        public User Get(int id)
        {
            return _cachedUsers.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Values
        [HttpPost]
        public async void Post([FromBody] User user)
        {
            _cachedUsers.SaveNewUserAsync(user); //не шарю нащо чекати
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
    }
}

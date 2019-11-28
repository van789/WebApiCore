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
    // по ТЗ маршрут має бути /user
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static List<User> _cachedUsers= new List<User>();
        
        //cтворити склас ЮзерСервіс замість цього ліста. Юзер сервіс має реалізувати інтерфес ІЮзерСервіс
        // З допомогою Dependency Injection зареєструвати ІЮзерСервіс як сігелтон
        
        //перийняти у конструкторі ІЮзерСервіс
        public ValuesController()
        {
            //ініціалізація має бути у ЮзерСервісі
            _cachedUsers.InitUsersCachAsync().Wait();
        }

        // GET: api/Values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            // використовувати тут ЮзерСервіс
            return _cachedUsers;
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        [SimpleActionFilter]
        public User Get(int id)
        {
            // використовувати тут ЮзерСервіс
            return _cachedUsers.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Values
        [HttpPost]
        public async void Post([FromBody] User user)
        {
            // використовувати тут ЮзерСервіс
            _cachedUsers.SaveNewUserAsync(user); //не шарю нащо чекати
            //якщо буде помилка, клієнт буде думати, що все ок, а це не так
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
        //Додати до інтерфейсу ІЮзерСервіс метод для видалення
        // використовувати тут ЮзерСервіс і видаляти по ід
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiCore.Models;

namespace WebApiCore.Helpers
{
    public static class UserHelpers
    {
        public static async Task InitUsersCachAsync(this List<User> cachedUsers, int startInitCount = 10)
        {
            var taskList = new List<Task>();
            for (int i = 0; i < startInitCount; i++)
            {
                var id = i;
                
                //дивсь комент до методу AddUserToCachAsync
                // цю строку можна видалити
                var newTask = Task.Run(() => cachedUsers.AddUserToCachAsync(id));
                //коли метод AddUserToCachAsync буде повертати Task<bool>
                //можна буде просто додати taskList.Add(cachedUsers.AddUserToCachAsync(id));
                taskList.Add(newTask);
            }
            
           // в даний момент це WhenAll виконається одразу бо він опрацьовує синхронні методи у яких асинхронний код
           // () => { // асинхроннийКод, запускає задачу але не чекає її, 
            //з синхронного методу можна чекати .Result або .Wait(), якщо у тебе є десь async то більшість методів має бути async} 
            await Task.WhenAll(taskList);
        }
        
        public static async Task SaveNewUserAsync(this List<User> cachedUsers, User newUser)
        {
            SaveUserToFileAsync(newUser);

            if (!cachedUsers.Any(x => x.Id == newUser.Id))
            {
                cachedUsers.Add(newUser);
            }
        }

        #region Приватні методи
            
            
        private static string GetPath(int id)
        {
            string appDataPath = Environment.CurrentDirectory;
            string fileName = $"user{id}.json";
            return Path.Combine(appDataPath, fileName);
        }
        
        //Зберігати всіх юзерів у одному файлі у вигляді масиву юзерів
        // Зробити цей метод потокобезпечним, зараз якщо 2 потока почне писати у файл, то буде пізда
        private static async Task SaveUserToFileAsync(User user)
        {
            var path = GetPath(user.Id);
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<User>(fs, user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR Saving: " + e.Message);
            }
        }
        
        //Зробити цей метод async Task<bool>, якщо все пройшло успішно повернути true
        private static void AddUserToCachAsync(this List<User> cachedUsers, int newId = 0)
        {
            if (!cachedUsers.Any(x => x.Id == newId))
            {
                //коли цей метод буде async, можна використати await GetUserFromFileAsync, і тоді .Result не потрібен
                var newUser = GetUserFromFileAsync(newId).Result; //підчитали з файлу
                if (newUser != null)
                {
                    cachedUsers.Add(newUser);
                }
            }
        }

        private static async Task<User> GetUserFromFileAsync(int newId = 0)
        {
            var path = GetPath(newId);
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        return await JsonSerializer.DeserializeAsync<User>(fs);
                    }
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR Reading: " + e.Message);
            }
            return null;
        }

        #endregion
    }
}

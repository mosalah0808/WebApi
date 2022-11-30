using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleKey consoleKey;
            bool IsExit = false;

            Console.WriteLine("------------ Взаимодействие между клиентом и сервером ----------");

            while (IsExit == false)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("1 - Добавить запись в таблицу Customers");
                Console.WriteLine("2 - Найти запись  по ID в таблице Customers");
                Console.WriteLine("3 - Завершить работу");
                Console.WriteLine("==============================");

                consoleKey = Console.ReadKey().Key;

                switch (consoleKey)
                {
                    case ConsoleKey.D1:
                        await CreateNewCustomer();
                        break;

                    case ConsoleKey.D2:
                        await GetUserById();
                        break;

                    case ConsoleKey.D3:
                        IsExit = true;
                        break;
                }


            }
        }

        private static CustomerCreateRequest RandomCustomer()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int next = rnd.Next(1, 100);

            return new CustomerCreateRequest
            {
                Firstname = $"FirstName{next}",
                Lastname = $"LastName{next}"
            };
        }

        static async Task CreateNewCustomer()
        {
            
            var customer = RandomCustomer();
            using var webClient = new HttpClient();
            using var response = await webClient.PostAsJsonAsync("https://localhost:5001/api/Customer", customer);
            Customer? newcustomer = await response.Content.ReadFromJsonAsync<Customer>();

            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Пользователь с таким Id уже существует...");
                Console.WriteLine($"Status code: {response.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Status code: {response.StatusCode}");
                Console.WriteLine($"Новый покупатель создан: {newcustomer?.Id} - {newcustomer?.Firstname} - {newcustomer?.Lastname}");
            }

            

        }

        static async Task GetUserById()
        {
            Console.Clear();

            Console.Write("Введите ID: ");
            var input = Console.ReadLine();

            if (!long.TryParse(input, out long id))
            {
                Console.WriteLine("Не удалось считать ID");
                return;
            }
           
            using var webClient = new HttpClient();

            using var response  = await webClient.GetAsync($"https://localhost:5001/api/Customer/{id}");
            
            if (response.StatusCode !=  HttpStatusCode.OK )
            {
                Console.WriteLine("Пользователя с таким Id не существует...");
                Console.WriteLine($"Status code: {response.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Status code: {response.StatusCode}");
                Customer? customer = await webClient.GetFromJsonAsync<Customer>($"https://localhost:5001/api/Customer/{id}");
                Console.WriteLine($"FirstName: {customer.Firstname}");
                Console.WriteLine($"LastName: {customer.Lastname}");                
            }
            
        }


       
    }
}
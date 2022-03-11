using ApiLiteDB.Fakes;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace ApiLiteDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("GerarFakes")]
        public IActionResult GerarFakes(int quantidade = 100)
        {

            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                // Create your new customer instance
                var fakeCustomers = FakeCustomer.ListarClientesFake(quantidade);

                // Insert new customer document (Id will be auto-incremented)
                col.InsertBulk(fakeCustomers);


            }

            return StatusCode(201);
        }

        [HttpPost]
        [Route("Cadastrar")]
        public IActionResult Cadastrar(Customer customer)
        {

            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(customer);


            }

            return StatusCode(201);
        }
        [HttpGet]
        [Route("ObterTodos")]
        public IActionResult ObterTodos(int page = 0, int pageSize = 10)
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                var pageIndex = page - 1;

                var offset = pageIndex * pageSize;

                // Use LINQ to query documents (filter, sort, transform)
                var results = page == 0 ? 
                    col.Query()
                    .OrderBy(x => x.Id)
                    .ToList() :
                   col.Query()
                    .OrderBy(x => x.Id)
                    .Offset(offset)
                    .Limit(pageSize)
                    .ToList();

                return StatusCode(200, results);
            }


        }
        [HttpGet]
        [Route("ObterPorId/{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                // Use LINQ to query documents (filter, sort, transform)
                var customer = col.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (customer == null)
                {
                    return StatusCode(404, $"Cliente não encontrado com o Id {id}");
                }

                return StatusCode(200, customer);
            }


        }

        [HttpPut]
        [Route("Atualizar/{id}")]
        public IActionResult Put(int id, Customer customer)
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                // Use LINQ to query documents (filter, sort, transform)
                var customerResult = col.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (customerResult == null)
                {
                    return StatusCode(404, $"Cliente não encontrado com o Id {id}");
                }

                customer.Id = id;

                col.Update(customer);

                return StatusCode(204);
            }

        }
        [HttpDelete]
        [Route("Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyData.db")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Customer>("customers");

                // Use LINQ to query documents (filter, sort, transform)
                var customerResult = col.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (customerResult == null)
                {
                    return StatusCode(404, $"Cliente não encontrado com o Id {id}");
                }

                return col.Delete(id) ? StatusCode(204) : StatusCode(400, "Deu ruim!");

            }
        }
    }
}
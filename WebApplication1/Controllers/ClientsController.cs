using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApplication1.Core;
using WebApplication1.Core.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {

        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public async Task<IEnumerable<Client>> Get()
        {
            var clients = new List<Client>();
            await using var connectionString = new MySqlConnection(Environment.GetEnvironmentVariable("ConnStr"));

            await connectionString.OpenAsync();

            await using var command = new MySqlCommand("SELECT * FROM Clients;", connectionString);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                clients.Add(new Client()
                {
                    Name = reader.GetString(0),
                    LastName = reader.GetString(1)
                });
            }
            return clients;
        }
    }
}

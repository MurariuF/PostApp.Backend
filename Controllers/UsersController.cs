using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostApp.Backend.Models;
using System.Data.SqlClient;

namespace PostApp.Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration config;

        public UsersController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> Users()
        {
            var sqlQuery = @"Select * 
                            FROM Users";
            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            var users = await connection.QueryAsync<Users>(sqlQuery);
            return Ok(users);
        }

        [HttpGet("{userEmail}")]
        public async Task<ActionResult<Users>> Users(string userEmail)
        {
            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            var user = await connection.QueryFirstAsync<Users>("Select * FROM Users WHERE Email = @Email", new { Email = userEmail });
            return Ok(user);
        }
    }
}

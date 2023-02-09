using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Hosting;

namespace PostApp.Backend.Models
{
    [Table("Users")]
    public class Users
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public List<Post> Posts { get; set; }
    }
}

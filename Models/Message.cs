using Dapper.Contrib.Extensions;

namespace PostApp.Backend.Models
{
    [Table("Message")]
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

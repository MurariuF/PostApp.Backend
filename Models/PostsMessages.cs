using Dapper.Contrib.Extensions;

namespace PostApp.Backend.Models
{
    [Table("PostsMessages")]
    public class PostsMessages
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid MessageId { get; set; }

    }
}

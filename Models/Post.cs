using Dapper.Contrib.Extensions;

namespace PostApp.Backend.Models
{
    [Table("Post")]
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<PostsMessages> PostsMessages { get; set; }
    }
}

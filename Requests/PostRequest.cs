namespace PostApp.Backend.Requests
{
    public class PostRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}

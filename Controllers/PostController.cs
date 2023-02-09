using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostApp.Backend.Models;
using PostApp.Backend.Requests;
using System.Data.SqlClient;

namespace PostApp.Backend.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration config;

        public PostController(IConfiguration config)
        {
            this.config = config;
        }

        private async Task<List<Post>> GetAllPosts()
        {
            var sqlQuery = @"Select * 
                            FROM Post";
            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            var posts = await connection.QueryAsync<Post>(sqlQuery);

            return posts.ToList();
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Posts()
        {
            return Ok(await GetAllPosts());
        }

        [HttpGet("{userEmail}")]
        public async Task<ActionResult<Post>> Posts(string userEmail)
        {
            var sql = @"SELECT *
                FROM Post P 
                INNER JOIN Users U ON P.UserId = U.Id
                INNER JOIN PostsMessages PM on P.Id = PM.PostId
                INNER JOIN Message M ON PM.MessageId = M.Id
                WHERE U.Email = @Email";

            var parameters = new DynamicParameters();
            parameters.Add("Email", userEmail);

            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));

            var queryResult = await connection.QueryAsync<Post, Users, List<PostsMessages>, Message, Post>(sql, (post, user, postsmessages, message) =>
            {
                post.User = user;
                post.PostsMessages = postsmessages;
                return post;
            },
            parameters
            );
            return Ok(queryResult);
        }

        [HttpPost]
        public async Task<ActionResult<List<Post>>> Posts(PostRequest post)
        {
            Post newPost = new Post
            {
                UserId = post.UserId,
                Content = post.Content
            };

            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            object parameters = new { UserId = newPost.UserId, Content = newPost.Content };
            await connection.ExecuteAsync("INSERT INTO Post (UserId, Content) VALUES (@UserId, @Content)", parameters);

            var result = await GetAllPosts();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<List<Post>>> UpdatePosts([FromBody] PostRequest post)
        {
            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            object parameters = new { Id = post.Id, UserId = post.UserId, Content = post.Content };
            await connection.ExecuteAsync("UPDATE Post SET Content = @Content WHERE Id = @Id AND UserId = @UserId", parameters);

            var result = await GetAllPosts();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<List<Post>>> DeletePost([FromBody] PostRequest post)
        {
            using var connection = new SqlConnection(config.GetConnectionString("myDb1"));
            object parameters = new { Id = post.Id };
            await connection.ExecuteAsync("DELETE FROM Post Where Id = @Id", parameters);

            var result = await GetAllPosts();

            return Ok(result);
        }
    }
}

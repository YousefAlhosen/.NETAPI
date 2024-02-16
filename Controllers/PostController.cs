using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// this class is to create a post controller

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);

        }

        [HttpGet("PostsSingle/{postId}")]
        public IEnumerable<Post> GetPosts(int postId)
        {
            string sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitel],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                    WHERE PostId = " + postId.ToString();

            return _dapper.LoadData<Post>(sql);
        }

    }
}
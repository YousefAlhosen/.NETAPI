using Dapper;
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
    public class PostController: ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);

        }


        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId=0, int userId=0, string searchParam="None")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";
            string stringParameters = "";
            DynamicParameters sqlParameters = new();

            if(postId != 0)
            {
                stringParameters += ", @PostId=@PostIdParameter";
                sqlParameters.Add("@PostIdParameter", postId, System.Data.DbType.Int32);

            }
            if(userId != 0)
            {
                stringParameters += ", @PostId=@UserIdParameter";
                sqlParameters.Add("@UserIdParameter", userId, System.Data.DbType.Int32);
                
            }
            if(searchParam.ToLower() != "none")
            {
                stringParameters += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", postId, System.Data.DbType.String);
                
            }

            if (stringParameters.Length > 0)
            {
                sql += stringParameters.Substring(1);

            }

            return _dapper.LoadDataWithParameters<Post>(sql, sqlParameters);
        }


        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@UserIdParameter",this.User.FindFirst("userId")?.Value,System.Data.DbType.Int32);

            return _dapper.LoadDataWithParameters<Post>(sql, sqlParameters);
        }


        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(Post postToUpsert)
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
                @UserId=@UserIdParameter,
                @PostTitel=@PostTitleParameter,
                @PostContent=@PostContentParameter";
            
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value,System.Data.DbType.Int32);
            sqlParameters.Add("@PostTitleParameter", postToUpsert.PostTitel, System.Data.DbType.String);
            sqlParameters.Add("@PostContentParameter", postToUpsert.PostContent, System.Data.DbType.String);

            if(postToUpsert.PostId > 0)
            {
                sql += ",@PostId=@PostIdParameter";
                sqlParameters.Add("@PostIdParameter", postToUpsert.PostId, System.Data.DbType.Int32);

            }
            
            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            } 
            throw new Exception ("Failed to upsert  Post ");

        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @" EXEC TutorialAppSchema.spPost_Delete
                 @UserId=@UserIdParameter,
                 @PostId=@PostIdParameter";
            
            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value,System.Data.DbType.Int32);
            sqlParameters.Add("@PostIdParameter", postId, System.Data.DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql,sqlParameters))
            {
                return Ok();
            } 
            throw new Exception ("Failed to Delete Post ");
            
        }

    }
}
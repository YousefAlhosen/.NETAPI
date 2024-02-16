using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper =   new DataContextDapper(config);
    }
    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";


        IEnumerable<User> users = _dapper.LoadData<User>(sql);

        return users;
    }

    
    
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users
                    WHERE UserId = " + userId.ToString();


        User user = _dapper.LoadDataSingle<User>(sql);

        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET  [FirstName] = '" + user.FirstName +
                "', [LastName] = '"+ user.LastName +
                "', [Email] = '" + user.Email + 
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active+
            "' WHERE UserId = " + user.UserId;

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception(" Failed to update user");

    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (" + 
                "'" + user.FirstName +
                "', '"+ user.LastName +
                "', '" + user.Email + 
                "', '" + user.Gender +
                "', '" + user.Active+
                
            "')";

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception(" Failed to add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userID)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE UserId = " + userID.ToString();

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception(" Failed to delete user");
    }

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalaries(int userId)
    {
        return _dapper.LoadData<UserSalary>(@"
            SELECT UserSalary.UserId
                    , UserSalary.Salary
            FROM TutorialAppSchema.UserSalary
                WHERE UserId = "+ userId);
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql = @"
                INSERT INTO TutorialAppSchema.UserSalary (
                    UserId,
                    Salary
                ) VALUES (" + userSalaryForInsert.UserId
                    + ", " + userSalaryForInsert.Salary
                    +")";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userSalaryForInsert);
        }

        throw new Exception("Adding userSalary failed on save ");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserSalary SET Salary="
            + userSalaryForUpdate.Salary
            + " WHERE UserId= " + userSalaryForUpdate.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userSalaryForUpdate);
        }

        throw new Exception("Updating UserSalary failed on save");
    }

    [HttpDelete("UserSalary/{userID}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary WHERE UserId=" + userId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Deleting User Salary failed on save");

    }

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"
                SELECT UserJobInfo.UserId
                     , UserJobInfo.JobTitel
                     , UserJobInfo.Department
                FROM TutorialAppSchema.UserJobInfo
                    WHERE UserId = " + userId);
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfo(UserJobInfo userJobInfoForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo (
                userId,
                Department,
                JobTitel
            ) VALUES (" + userJobInfoForInsert.UserId
                + ", '" + userJobInfoForInsert.Department
                + "', '"+ userJobInfoForInsert.JobTitel
                +"')";


        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForInsert);
        }

        throw new Exception("Adding User Job Info failes on save");
    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutUSerJobInfo(UserJobInfo userJobInfoForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department= '"
            + userJobInfoForUpdate.Department
            + "', JobTitel='"
            + userJobInfoForUpdate.JobTitel
            + "' WHERE UserId= " + userJobInfoForUpdate.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForUpdate);
        }

        throw new Exception("Updating User Info failed on save");

    }

    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = " DELETE FROM TutorialAppSchema.UserJobInfo Where UserId=" + userId;
        if( _dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Deleting User Job Info failed on save");
    }







    
    
}

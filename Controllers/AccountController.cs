using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAPI.Dto;
using BookAPI.Entities;
using BookAPI.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private IAccount _account;
        public AccountController(IAccount account)
        {
            _account = account;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto registerUser)
        {
            ApplicationUser user = new ApplicationUser();

            user.FirstName = registerUser.FirstName;
            user.LastName = registerUser.LastName;
            user.UserName = registerUser.Username;
            user.Email = registerUser.Email;


            var newUser = await _account.CreateUser(user, registerUser.Password);
            if (newUser)
                return Ok(new { message = "User Created" });

            return BadRequest(new { message = "User not created" });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto userDto)
        {
            var user = await _account.SignIn(userDto);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        //[AllowAnonymous]
        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _account.GetAll();
            return Ok(users);
        }

        //GetByUserId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _account.GetByUserId(id);
            return Ok(user);
        }

        //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ApplicationUser appuser)
        {
            appuser.Id = id;
            var updateuser = await _account.UpdateUser(appuser);

            if (updateuser)
            {
                return Ok("Account Updated");
            }
            else
            {
                return BadRequest(new { message = "Unable to update Account details" });
            }
        }

        //// DELETE 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteUser = await _account.Delete(id);
            if (deleteUser)
            {
                return Ok("Account Deleted");
            }
            else
            {
                return BadRequest(new { message = "Unable to delete Account details" });
            }
        }
    }
}

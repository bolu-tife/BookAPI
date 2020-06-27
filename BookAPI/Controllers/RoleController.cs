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
    public class RoleController : ControllerBase
    {
        private IRole _role;
        public RoleController(IRole role)
        {
            _role = role;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleModels registerRole)
        {
            ApplicationRole role = new ApplicationRole();

            role.Name = registerRole.Name;
            


            var newRole = await _role.CreateRole(role);
            if (newRole)
                return Ok(new { message = "Role Created" });

            return BadRequest(new { message = "Role not created" });
        }

        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] LoginDto userDto)
        //{
        //    var user = await _account.SignIn(userDto);

        //    if (user == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });

        //    return Ok(user);
        //}

        //[AllowAnonymous]
        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _role.GetAll();
            return Ok(roles);
        }
        //GetByUserId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByRoleId(string id)
        {
            var role = await _role.GetByRoleId(id);
            return Ok(role);
        }

        //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ApplicationRole approle)
        {
            approle.Id = id;
            var updaterole = await _role.UpdateUser(approle);

            if (updaterole)
            {
                return Ok("Role Updated");
            }
            else
            {
                return BadRequest(new { message = "Unable to update Role details" });
            }
        }

        //// DELETE 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteRole = await _role.Delete(id);
            if (deleteRole)
            {
                return Ok("Role Deleted");
            }
            else
            {
                return BadRequest(new { message = "Unable to delete ROle details" });
            }
        }
    }
}

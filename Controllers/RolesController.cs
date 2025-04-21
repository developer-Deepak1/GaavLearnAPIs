using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaavLearnAPIs.Dtos;
using GaavLearnAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaavLearnAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class RolesController(RoleManager<IdentityRole> roleManager
    ,UserManager<AppUser> userManager):ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager=roleManager;
        private readonly UserManager<AppUser> _userManager=userManager;
 
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto){

           if(string.IsNullOrEmpty(createRoleDto.RoleName)) {
            return BadRequest("Role Name Is Required");
           }

            var roleExist= await _roleManager.RoleExistsAsync(createRoleDto.RoleName);
            if(roleExist){
                return BadRequest("Role Already Exists");
            }

            var roleResult=await _roleManager.CreateAsync(new IdentityRole(createRoleDto.RoleName));

            if(roleResult.Succeeded){
               return Ok(new {message="Role Created Successfully"});
            }

            return Ok("Role Creation Failed");
        }

        [HttpGet]
        [Route("GetRole")]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetRole(){
            var roles=await _roleManager.Roles.Select(u=>new RoleResponseDto{
              Id=u.Id,
              Name=u.Name,
              TotalUsers=_userManager.GetUsersInRoleAsync(u.Name!).Result.Count
            }).ToListAsync();

            return Ok(roles);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteRole(string id){
            var role=await _roleManager.FindByIdAsync(id);
            if(role is null){
                return NotFound("Role Not Found");
            }
            var result= await _roleManager.DeleteAsync(role);
            if(result.Succeeded){
                return Ok(new {message="Role deleted Successfully."});
            }
            return BadRequest("Role deleted Failed");
        }
        
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDto roleAssignDto){
          var user= await _userManager.FindByIdAsync(roleAssignDto.UserId);

          if(user is null){
           return NotFound("User Not Found");
          }
          var role =await _roleManager.FindByIdAsync(roleAssignDto.RoleId);

          if(role is null){
            return NotFound("Role Not Found");
          }
          var result= await _userManager.AddToRoleAsync(user,role.Name!);
          if(result.Succeeded){
            return Ok(new {message="Role Assign Successfully"});
          }

          var error=result.Errors.FirstOrDefault();
          
          return BadRequest(error!.Description);
        }
    }
}
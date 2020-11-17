using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.DTOs.Role;
using WebApp.API.Services.Interfaces;

namespace WebApp.API.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)   
        {
            _adminService = adminService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _adminService.GetUsersWithRoles();
            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _adminService.GetRoles();
            return Ok(roles);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var result = await _adminService.EditRoles(userName, roleEditDTO.RoleNames);
            if (result.Failure) 
            {
                return BadRequest(result.Error);
            }
            
            return Ok();
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpGet("adsForModeration")]
        public async Task<IActionResult> GetAdsForModeration()
        {
            var ads = await _adminService.GetAdsForModeration();
            return Ok(ads);
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpPost("approveAd/{id}")]
        public async Task<IActionResult> ApproveAd(int id) 
        {
            await _adminService.ApproveAd(id);
            return Ok();
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpDelete("rejectAd/{id}")]
        public async Task<IActionResult> RejectAd(int id) 
        {
            await _adminService.RejectAd(id);
            return Ok();
        }
    }
}
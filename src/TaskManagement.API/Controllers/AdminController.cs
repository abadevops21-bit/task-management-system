using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;
using TaskManagementSystem.Application.Common.Constants;
using TaskManagementSystem.Application.Interfaces;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = Roles.SuperUser+","+Roles.Admin)] 
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPut("change-role")]
    public async Task<IActionResult> ChangeRole(Guid userId, string role)
    {
        await _adminService.ChangeUserRole(userId, role);
        return Ok("Role updated successfully");
    }
}
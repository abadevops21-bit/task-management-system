using TaskManagement.Application.Interfaces;
using TaskManagementSystem.Infrastructure.Data;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ChangeUserRole(Guid userId, string newRole)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.Role = newRole;

        await _context.SaveChangesAsync();

        return true;
    }
}
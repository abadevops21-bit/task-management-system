using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Interfaces
{
    public interface IAdminService
    {
        Task<bool> ChangeUserRole(Guid userId, string newRole);
    }
}

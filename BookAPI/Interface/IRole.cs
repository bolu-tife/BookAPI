using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPI.Entities;

namespace BookAPI.Interface
{
    public interface IRole
    {
        Task<bool> CreateRole(ApplicationRole role);
        Task<IEnumerable<ApplicationRole>> GetAll();
        Task<ApplicationRole> GetByRoleId(string id);
        Task<bool> Delete(string id);
        Task<bool> UpdateUser(ApplicationRole role);

    }
}

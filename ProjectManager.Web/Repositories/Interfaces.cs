using ProjectManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Repositories
{
    public interface IProject
    {
        Task<List<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(string id);
        Task InsertAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task DeleteByIdAsync(string id);
    }

    public interface IApplicationUser
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task InsertAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(ApplicationUser user);
        Task DeleteByIdAsync(string id);
    }


}

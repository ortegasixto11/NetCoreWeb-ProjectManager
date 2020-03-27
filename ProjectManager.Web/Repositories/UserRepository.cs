using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Repositories
{
    public class UserRepository : IApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _userManager.DeleteAsync(await GetByIdAsync(id));
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.AsNoTracking().OrderBy(x => x.Email).ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task InsertAsync(ApplicationUser user)
        {
            user.IsActive = true;
            user.NormalizedEmail = user.Email;
            user.UserName = $"{user.FirstName}{user.LastName}";
            user.NormalizedUserName = $"{user.FirstName}{user.LastName}";
            await _userManager.CreateAsync(user, user.PasswordNotMapped);
        }

        public async Task UpdateAsync(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.DNI = model.DNI;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Role = model.Role;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email;
            user.UserName = $"{model.FirstName}{model.LastName}";
            user.NormalizedUserName = $"{model.FirstName}{model.LastName}";
            user.IsActive = model.IsActive;
            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.PasswordNotMapped);
            await _userManager.UpdateAsync(user);
        }

        public async Task<SelectList> GetSelectListUsersAsync()
        {
            return new SelectList(await GetAllAsync(), "Id", "Email");
        }

        public async Task<List<ApplicationUser>> GetProjectUsersByProjectIdAsync(string projectId)
        {
            return await _userManager.Users.Include(x => x.ProjectUsers).Where(x => x.ProjectUsers.Any(y => y.ProjectId == projectId)).ToListAsync();
        }

    }
}

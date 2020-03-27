using Microsoft.EntityFrameworkCore;
using ProjectManager.Web.Data;
using ProjectManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Repositories
{
    public class ProjectRepository : IProject
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            _context.Projects.Remove(await GetByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects.Include(x => x.ProjectUsers).Include(x => x.ProjectWorkflows).ToListAsync();
        }

        public async Task<Project> GetByIdAsync(string id)
        {
            return await _context.Projects.Where(x => x.Id == id)
                .Include(x => x.ProjectUsers).ThenInclude(y => y.User)
                .Include(x => x.ProjectWorkflows).ThenInclude(y => y.User)
                .FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Project project)
        {
            project.IsActive = true;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(ProjectUser projectUser)
        {
            if (!await UserExistsInProjectAsync(projectUser.ProjectId, projectUser.UserId))
            {
                projectUser.Id = Guid.NewGuid().ToString();
                _context.ProjectUsers.Add(projectUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserAsync(string projectId, string userId)
        {
            var projectUser = await _context.ProjectUsers.Where(x => x.ProjectId == projectId && x.UserId == userId).FirstOrDefaultAsync();
            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetProjectNameByIdAsync(string id)
        {
            return await _context.Projects.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
        }

        public async Task<bool> UserExistsInProjectAsync(string projectId, string userId)
        {
            return await _context.ProjectUsers.AnyAsync(x => x.ProjectId == projectId && x.UserId == userId);
        }

        public async Task<List<Project>> GetUserProjectsByUserIdAsync(string userId)
        {
            return await _context.Projects
                .Include(x => x.ProjectUsers)
                .Include(x => x.ProjectWorkflows)
                .Where(x => x.ProjectUsers.Any(y => y.UserId == userId) && x.IsActive == true).ToListAsync();
        }

        public async Task AddHoursToProjectWorkflowAsync(string projectId, string userId, double reportedHours)
        {
            var model = new ProjectWorkflow { 
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectId,
                UserId = userId,
                ReportedHours = reportedHours,
                Timestamp = DateTime.Now
            };
            _context.ProjectWorkflows.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectWorkflow>> GetUserProjectWorkflowByUserIdAsync(string userId, string projectId)
        {
            return await _context.ProjectWorkflows.Where(x => x.UserId == userId && x.ProjectId == projectId).ToListAsync();
        }

        public async Task RemoveHoursFromProjectWorkflowAsync(string projectWorkflowId)
        {
            var projectWorkflow = await _context.ProjectWorkflows.Where(x => x.Id == projectWorkflowId).FirstOrDefaultAsync();
            _context.ProjectWorkflows.Remove(projectWorkflow);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectWorkflow>> GetProjectWorkflowUsersByProjectIdAsync(string projectId)
        {
            return await _context.ProjectWorkflows.Where(x => x.ProjectId == projectId).ToListAsync();
        }

    }
}

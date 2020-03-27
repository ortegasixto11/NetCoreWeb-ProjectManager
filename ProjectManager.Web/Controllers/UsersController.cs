using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Web.Data;
using ProjectManager.Web.Models;
using ProjectManager.Web.Repositories;

namespace ProjectManager.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserRepository _repo;
        private readonly ProjectRepository _projectsRepo;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repo = new UserRepository(userManager);
            _projectsRepo = new ProjectRepository(context);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var user = await _repo.GetByIdAsync(id.Value.ToString());
            if (user == null) return NotFound();
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                user.Role = user.Role.ToUpper();
                await _repo.InsertAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var user = await _repo.GetByIdAsync(id.Value.ToString());
            if (user == null) return NotFound();
            return View(new ViewModels.UserEditViewModel { 
                Id = user.Id,
                DNI = user.DNI,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Email = user.Email,
                IsActive = user.IsActive
            });
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationUser user)
        {
            if (id != Guid.Parse(user.Id)) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var user = await _repo.GetByIdAsync(id.Value.ToString());
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repo.DeleteByIdAsync(id.ToString());
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/ChangePassword/5
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(new ViewModels.UserChangePasswordViewModel
            {
                Id = user.Id,
                Email = user.Email
            });
        }

        // POST: Users/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ViewModels.UserChangePasswordViewModel changePassword)
        {
            if (string.IsNullOrEmpty(changePassword.Id)) return NotFound();
            await _repo.ChangePasswordAsync(new ApplicationUser { Id = changePassword.Id, PasswordNotMapped = changePassword.Password });
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Projects
        public async Task<IActionResult> Projects(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest();
            var projects = await _projectsRepo.GetUserProjectsByUserIdAsync(userId);
            ViewData["UserId"] = userId;
            return View(projects);
        }

        // GET: Users/ReportHours
        public async Task<IActionResult> ReportHours(string projectId, string userId)
        {
            if (string.IsNullOrEmpty(projectId)) return BadRequest();
            ViewData["ProjectId"] = projectId;
            ViewData["UserId"] = userId;
            ViewData["ProjectName"] = await _projectsRepo.GetProjectNameByIdAsync(projectId);
            return View();
        }

        // POST: Users/ReportHours
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportHours(ViewModels.UserReportHoursViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProjectId)) return BadRequest();
            if (string.IsNullOrEmpty(viewModel.UserId)) return BadRequest();
            await _projectsRepo.AddHoursToProjectWorkflowAsync(viewModel.ProjectId, viewModel.UserId, viewModel.ReportedHours);
            return RedirectToAction(nameof(Projects), new { userId = viewModel.UserId });
        }

        // GET: Users/ProjectDetails
        public async Task<IActionResult> ProjectDetails(string projectId, string userId)
        {
            if (string.IsNullOrEmpty(projectId)) return BadRequest();
            if (string.IsNullOrEmpty(userId)) return BadRequest();
            var projectWorkflow = await _projectsRepo.GetUserProjectWorkflowByUserIdAsync(userId, projectId);
            ViewData["UserId"] = userId;
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = await _projectsRepo.GetProjectNameByIdAsync(projectId);
            return View(projectWorkflow);
        }

        // GET: Users/RemoveReportedHour
        public async Task<IActionResult> RemoveReportedHour(string projectWorkflowId, string projectId, string userId)
        {
            if (string.IsNullOrEmpty(projectWorkflowId)) return BadRequest();
            if (string.IsNullOrEmpty(projectId)) return BadRequest();
            if (string.IsNullOrEmpty(userId)) return BadRequest();
            await _projectsRepo.RemoveHoursFromProjectWorkflowAsync(projectWorkflowId);
            return RedirectToAction(nameof(ProjectDetails), new { userId, projectId });
        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Web.Data;
using ProjectManager.Web.Models;
using ProjectManager.Web.Repositories;

namespace ProjectManager.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectRepository _repo;
        private readonly UserRepository _usersRepo;

        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repo = new ProjectRepository(context);
            _usersRepo = new UserRepository(userManager);
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var project = await _repo.GetByIdAsync(id.Value.ToString());
            if (project == null) return NotFound();
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid().ToString();
                await _repo.InsertAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var project = await _repo.GetByIdAsync(id.Value.ToString());
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Project project)
        {
            if (id != Guid.Parse(project.Id)) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateAsync(project);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var project = await _repo.GetByIdAsync(id.Value.ToString());
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repo.DeleteByIdAsync(id.ToString());
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/ManageUsers/5
        public async Task<IActionResult> ManageUsers(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            ViewData["Users"] = await _usersRepo.GetSelectListUsersAsync();
            ViewData["ProjectId"] = id;
            ViewData["ProjectName"] = await _repo.GetProjectNameByIdAsync(id);
            var users = await _usersRepo.GetProjectUsersByProjectIdAsync(id);
            return View(users);
        }

        // POST: Projects/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser([FromForm] ViewModels.ProjectManageUsersViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProjectId)) return NotFound();
            if (string.IsNullOrEmpty(viewModel.UserId)) return NotFound();
            await _repo.AddUserAsync(new ProjectUser { ProjectId = viewModel.ProjectId, UserId = viewModel.UserId });
            return RedirectToAction(nameof(ManageUsers), new { id = viewModel.ProjectId });
        }

        // GET: Projects/RemoveUser/5/5
        public async Task<IActionResult> RemoveUser(string projectId, string userId)
        {
            if (string.IsNullOrEmpty(projectId)) return NotFound();
            if (string.IsNullOrEmpty(userId)) return NotFound();
            await _repo.RemoveUserAsync(projectId, userId);
            return RedirectToAction(nameof(ManageUsers), new { id = projectId });
        }



    }
}

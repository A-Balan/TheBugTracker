using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Models.ViewModels;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : BTBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTFileService _bTFileService;
        private readonly IBTTicketService _btTicketService;
        private readonly IBTProjectService _projectService;
        private readonly IBTRolesService _rolesService;

        public ProjectsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTFileService bTFileService, IBTTicketService btTicketService, IBTProjectService projectService, IBTRolesService rolesService)
        {
            _context = context;
            _userManager = userManager;
            _bTFileService = bTFileService;
            _btTicketService = btTicketService;
            _projectService = projectService;
            _rolesService = rolesService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            IEnumerable<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(_companyId);
            return View(projects);
            
        }

		// GET: Projects/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Remember that the _context should not be used directly in the controller so....     

			// Edit the following code to use the service layer. 
			// Your goal is to return the 'project' from the databse
			// with the Id equal to the parameter passed in.               
			// This is the only modification necessary for this method/action.     

			var project = await _context.Projects
				.Include(p => p.Company)
                .Include(p=> p.Members)
				.Include(p => p.ProjectPriority)
				.FirstOrDefaultAsync(m => m.Id == id);

            ViewData["Tickets"] = await _btTicketService.GetTicketsByProjectIdAsync(project?.Id);


			if (project == null)
			{
				return NotFound();
			}

			return View(project);
		}

		// GET: Projects/Create
		public IActionResult Create()
        {

            int companyId = User.Identity!.GetCompanyId();
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ProjectPriorityId,ImageFormFile,CompanyId")] Project project)
        {
            if (ModelState.IsValid)
            {
                if (project.ImageFormFile != null)
                {
                    // Create the Image Service
                    // 1. Convert file to byte array and assign to ImageDate
                    project.ImageFileData = await _bTFileService.ConvertFileToByteArrayAsync(project.ImageFormFile);
                    // 2. Assign the ImageType based on the choosen file
                    project.ImageFileType = project.ImageFormFile.ContentType;
                }

                project.Created = DateTime.Now;

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Id", project.ProjectPriorityId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,Name,Description,Created,StartDate,EndDate,ProjectPriorityId,ImageFileData,ImageFileType,ImageFormFile,Archived")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Set the Image data if one has been choosen
                if (project.ImageFormFile != null)
                {
                    // Create the Image Service
                    // 1. Convert file to byte array and assign to ImageDate
                    project.ImageFileData = await _bTFileService.ConvertFileToByteArrayAsync(project.ImageFormFile);
                    // 2. Assign the ImageType based on the choosen file
                    project.ImageFileType = project.ImageFormFile.ContentType;
                }

                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Id", project.ProjectPriorityId);
            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> AssignPM(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Project? project = await _projectService.GetProjectByIdAsync(id, _companyId);

            if(project == null)
            {
                return NotFound();
            }

            //get list of PMs

            IEnumerable<BTUser> projectManagers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _companyId);

            //gets us PM user if assigned

            BTUser? currentPM = await _projectService.GetProjectManagerAsync(id);

            AssignPMViewModel viewModel = new()
            {
                ProjectId = project.Id,
                    ProjectName = project.Name,
                    PMList = new SelectList(projectManagers, "Id", "FullName", currentPM?.Id),
                    PMId = currentPM?.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPM(AssignPMViewModel viewModel)
        {
            if(!string.IsNullOrEmpty(viewModel.PMId))
            {
                if (await _projectService.AddProjectManagerAsync(viewModel.PMId, viewModel.ProjectId))
                {
                    return RedirectToAction(nameof(Details), new { id = viewModel.ProjectId });
                }
                else
                {

                    ModelState.AddModelError("PMId", "Error assigning PM.");
                }
                //ModelState.AddModelError("PMId", "Error, No PM chosen, please choose a PM");
            }


            ModelState.AddModelError("PMId", "No Project Manager chosen. Please select a Project Manager.");
            IEnumerable<BTUser> projectManagers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _companyId);

            //gets us PM user if assigned

            BTUser? currentPM = await _projectService.GetProjectManagerAsync(viewModel.ProjectId);
            viewModel.PMList = new SelectList(projectManagers, "Id", "FullName", currentPM.Id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AssignProjectMembers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //getting a project back?
            Project? project = await _projectService.GetProjectByIdAsync(id, _companyId);

            if (project == null)
            {
                return NotFound();
            }
            List<BTUser> submitters = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _companyId);
            List<BTUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
            List<BTUser> usersList = submitters.Concat(developers).ToList();
            IEnumerable<string> currentMembers = project.Members.Select(u => u.Id);
            ProjectMembersViewModel viewModel = new()
            {
                Project = project,
                UsersList = new MultiSelectList(usersList, "Id", "FullName", currentMembers)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectMembers(ProjectMembersViewModel viewModel)
        {
            if (viewModel.SelectedMembers != null)
            {
                //it knows Id is there bc it came in hidden from Form-AssignProjMem view
                await _projectService.RemoveMembersFromProjectAsync(viewModel.Project?.Id, _companyId);
                await _projectService.AddMembersToProjectAsync(viewModel.SelectedMembers, viewModel.Project?.Id, _companyId);

                return RedirectToAction(nameof(Details), new { id = viewModel.Project?.Id });
            }
            //to handle an error
            ModelState.AddModelError("SelectedMembers", "No Users chosen. Please select Users.");

            viewModel.Project = await _projectService.GetProjectByIdAsync(viewModel.Project?.Id, _companyId);

            List<BTUser> submitters = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _companyId);
            List<BTUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
            List<BTUser> usersList = submitters.Concat(developers).ToList();
            IEnumerable<string> currentMembers = viewModel.Project!.Members.Select(u => u.Id);
            //repopulate for view to make sense again
            viewModel.UsersList = new MultiSelectList(usersList, "Id", "FullName", currentMembers);

            return View(viewModel);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Archived = true;

                IEnumerable<Ticket> tickets = _context.Tickets.Where(t => t.ProjectId == id);

                foreach (Ticket ticket in tickets)
                {
                    ticket.ArchivedByProject = true;
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Projects/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Archived = false;

                IEnumerable<Ticket> tickets = _context.Tickets.Where(t => t.ProjectId == id);

                foreach (Ticket ticket in tickets)
                {
                    ticket.ArchivedByProject = false;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

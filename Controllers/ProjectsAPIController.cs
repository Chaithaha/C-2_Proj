using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CreativeColab.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsAPIController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            if (id != project.ProjectId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Update(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/projects/5/users
        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<ProjectUser>>> GetProjectUsers(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project.ProjectUsers);
        }

        // POST: api/projects/5/users
        [HttpPost("{id}/users")]
        public async Task<ActionResult<ProjectUser>> AddProjectUser(int id, [FromBody] ProjectUser projectUser)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            projectUser.ProjectId = id;
            _context.ProjectUsers.Add(projectUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectUsers), new { id = id }, projectUser);
        }

        // DELETE: api/projects/5/users/3
        [HttpDelete("{projectId}/users/{userId}")]
        public async Task<IActionResult> DeleteProjectUser(int projectId, int userId)
        {
            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);

            if (projectUser == null)
            {
                return NotFound("Project user not found");
            }

            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/projects/5/deadlines
        [HttpGet("{id}/deadlines")]
        public async Task<ActionResult<IEnumerable<ProjectDeadline>>> GetProjectDeadlines(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectDeadlines)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project.ProjectDeadlines);
        }

        // POST: api/projects/5/deadlines
        [HttpPost("{id}/deadlines")]
        public async Task<ActionResult<ProjectDeadline>> AddProjectDeadline(int id, [FromBody] ProjectDeadline deadline)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            deadline.ProjectId = id;
            _context.ProjectDeadlines.Add(deadline);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectDeadlines), new { id = id }, deadline);
        }

        // DELETE: api/projects/5/deadlines/3
        [HttpDelete("{projectId}/deadlines/{deadlineId}")]
        public async Task<IActionResult> DeleteProjectDeadline(int projectId, int deadlineId)
        {
            var deadline = await _context.ProjectDeadlines
                .FirstOrDefaultAsync(d => d.DeadlineId == deadlineId && d.ProjectId == projectId);

            if (deadline == null)
            {
                return NotFound("Deadline not found");
            }

            _context.ProjectDeadlines.Remove(deadline);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
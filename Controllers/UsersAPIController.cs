using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CreativeColab.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersAPIController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/users/5/projects
        [HttpGet("{id}/projects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetUserProjects(int id)
        {
            var user = await _context.Users
                .Include(u => u.OwnedProjects)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.OwnedProjects);
        }

        // GET: api/users/5/bookmarks
        [HttpGet("{id}/bookmarks")]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetUserBookmarks(int id)
        {
            var user = await _context.Users
                .Include(u => u.Bookmarks)
                .ThenInclude(b => b.Game)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Bookmarks);
        }

        // POST: api/users/5/bookmarks
        [HttpPost("{id}/bookmarks")]
        public async Task<ActionResult<Bookmark>> AddUserBookmark(int id, [FromBody] Bookmark bookmark)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            bookmark.UserId = id;
            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBookmarks), new { id = id }, bookmark);
        }

        // DELETE: api/users/5/bookmarks/3
        [HttpDelete("{userId}/bookmarks/{bookmarkId}")]
        public async Task<IActionResult> DeleteUserBookmark(int userId, int bookmarkId)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.UserId == userId);

            if (bookmark == null)
            {
                return NotFound("Bookmark not found");
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/users/5/payments
        [HttpGet("{id}/payments")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetUserPayments(int id)
        {
            var user = await _context.Users
                .Include(u => u.Payments)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Payments);
        }

        // GET: api/users/5/reminders
        [HttpGet("{id}/reminders")]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetUserReminders(int id)
        {
            var user = await _context.Users
                .Include(u => u.Reminders)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Reminders);
        }

        // POST: api/users/5/reminders
        [HttpPost("{id}/reminders")]
        public async Task<ActionResult<Reminder>> AddUserReminder(int id, [FromBody] Reminder reminder)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            reminder.UserId = id;
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserReminders), new { id = id }, reminder);
        }

        // DELETE: api/users/5/reminders/3
        [HttpDelete("{userId}/reminders/{reminderId}")]
        public async Task<IActionResult> DeleteUserReminder(int userId, int reminderId)
        {
            var reminder = await _context.Reminders
                .FirstOrDefaultAsync(r => r.ReminderId == reminderId && r.UserId == userId);

            if (reminder == null)
            {
                return NotFound("Reminder not found");
            }

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
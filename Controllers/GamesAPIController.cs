using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CreativeColab.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesAPIController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.Games.ToListAsync();
            return Ok(games);
        }

        // GET: api/games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        // POST: api/games
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
        }

        // PUT: api/games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] Game game)
        {
            if (id != game.GameId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Update(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/games/5/bookmarks
        [HttpGet("{id}/bookmarks")]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetGameBookmarks(int id)
        {
            var game = await _context.Games
                .Include(g => g.Bookmarks)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game.Bookmarks);
        }

        // POST: api/games/5/bookmarks
        [HttpPost("{id}/bookmarks")]
        public async Task<ActionResult<Bookmark>> AddGameBookmark(int id, [FromBody] Bookmark bookmark)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound("Game not found");
            }

            bookmark.GameId = id;
            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGameBookmarks), new { id = id }, bookmark);
        }

        // DELETE: api/games/5/bookmarks/3
        [HttpDelete("{gameId}/bookmarks/{bookmarkId}")]
        public async Task<IActionResult> DeleteGameBookmark(int gameId, int bookmarkId)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.GameId == gameId);

            if (bookmark == null)
            {
                return NotFound("Bookmark not found");
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
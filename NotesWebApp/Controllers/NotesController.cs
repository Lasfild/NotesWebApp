using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesWebApp.Contracts;
using NotesWebApp.DataAccess;
using NotesWebApp.Models;
using System.Linq.Expressions;

namespace NotesWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public NotesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, CancellationToken ct)
        {
            var note = new Note(request.Title, request.Description);

            await _dbContext.Notes.AddAsync(note, ct);
            await _dbContext.SaveChangesAsync(ct);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetNotesRequest request, CancellationToken ct)
        {
            var notesQuerry = _dbContext.Notes
                .Where(n => !string.IsNullOrWhiteSpace(request.Search) &&
                            n.Title.ToLower().Contains(request.Search.ToLower()));

            Expression<Func<Note, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "date" => note => note.CreatedAt,
                "title" => note => note.Title,
                _ => Note => Note.Id
            };

            notesQuerry = request.SortOrder == "desc"
                ? notesQuerry.OrderByDescending(selectorKey)
                : notesQuerry.OrderBy(selectorKey);

            var noteDtos = await notesQuerry
                .Select(n => new NoteDto(n.Id, n.Title, n.Description, n.CreatedAt))
                .ToListAsync(cancellationToken: ct);

            return Ok(new GetNotesResponse(noteDtos));
        }
    }
}

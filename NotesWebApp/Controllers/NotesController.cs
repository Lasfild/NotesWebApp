using Microsoft.AspNetCore.Mvc;
using NotesWebApp.Contracts;
using NotesWebApp.DataAccess;
using NotesWebApp.Models;

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
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}

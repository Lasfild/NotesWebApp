using Microsoft.AspNetCore.Mvc;

namespace NotesWebApp.Contracts
{
    public record CreateNoteRequest(string Title, string Description)
    {

    }
}

namespace NotesWebApp.Contracts
{
    public record NoteDto(Guid id, string Title, string Description, DateTime CreatedAt);
}

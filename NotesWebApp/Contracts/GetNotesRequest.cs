﻿namespace NotesWebApp.Contracts
{
    public record GetNotesRequest(string? Search, string? SortItem, string? SortOrder)
    {
    }
}

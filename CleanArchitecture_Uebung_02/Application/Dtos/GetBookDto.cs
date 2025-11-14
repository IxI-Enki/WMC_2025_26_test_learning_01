namespace Application.Dtos;

public record GetBookDto(int Id, string ISBN, string Title, int AuthorId, string AuthorName, 
    int PublicationYear, int AvailableCopies);


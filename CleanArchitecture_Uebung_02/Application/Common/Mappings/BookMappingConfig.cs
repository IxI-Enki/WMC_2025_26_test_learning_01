using Application.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mappings;

/// <summary>
/// Mapster-Konfiguration für Book-Mappings.
/// </summary>
public static class BookMappingConfig
{
    public static void ConfigureBookMappings()
    {
        TypeAdapterConfig<Book, GetBookDto>.NewConfig()
            .Map(dest => dest.AuthorName, src => src.Author != null 
                ? $"{src.Author.FirstName} {src.Author.LastName}" 
                : null);
    }
}

using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Authors.Queries.GetAllAuthors;

public readonly record struct GetAllAuthorsQuery : IRequest<Result<IReadOnlyCollection<GetAuthorDto>>>;
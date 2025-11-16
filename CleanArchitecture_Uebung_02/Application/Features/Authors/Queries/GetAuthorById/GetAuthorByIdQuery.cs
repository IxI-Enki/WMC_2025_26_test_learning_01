using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Authors.Queries.GetAuthorById;

public readonly record struct GetAuthorByIdQuery(int Id) : IRequest<Result<GetAuthorDto>>;

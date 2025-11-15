using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public readonly record struct CreateBookCommand( string Location, string Name, DateTime Timestamp, double Value )
: IRequest<Result<GetBookDto>>;

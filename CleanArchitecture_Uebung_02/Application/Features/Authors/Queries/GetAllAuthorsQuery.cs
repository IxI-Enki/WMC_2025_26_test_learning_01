using Application.Common.Models;
using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authors.Queries;
public record GetAllAuthorsQuery : IRequest<Result<IReadOnlyCollection<GetAuthorDto>>>;
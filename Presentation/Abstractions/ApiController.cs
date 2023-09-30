using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions;

[ApiController]
public class ApiController : ControllerBase
{
    protected IMediator Mediator { get; }

    protected ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}
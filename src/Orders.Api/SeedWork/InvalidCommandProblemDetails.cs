using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Configuration.Validation;

namespace Orders.Api.SeedWork;

public class InvalidCommandProblemDetails : ProblemDetails
{
    public InvalidCommandProblemDetails(InvalidCommandException exception)
    {
        Title = exception.Message;
        Status = StatusCodes.Status400BadRequest;
        Detail = exception.Details;
        Type = "https://somedomain/validation-error";
    }
}
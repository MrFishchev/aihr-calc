using System.Net.Mime;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Microsoft.AspNetCore.Mvc;

namespace Aihr.Calculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICoursesProvider _coursesProvider;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICoursesProvider coursesProvider, ILogger<CoursesController> logger)
    {
        _coursesProvider = coursesProvider;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _coursesProvider.GetAllCoursesAsync(cancellationToken));
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to fetch courses");
            return Problem();
        }
    }
}
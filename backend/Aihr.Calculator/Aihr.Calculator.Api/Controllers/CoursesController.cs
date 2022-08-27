using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aihr.Calculator.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class CoursesController : ControllerBase
{
    private readonly ICoursesProvider _coursesProvider;

    public CoursesController(ICoursesProvider coursesProvider)
    {
        _coursesProvider = coursesProvider;
    }

    [HttpGet]
    public async Task<List<Course>> GetAll(CancellationToken cancellationToken)
    {
        return await _coursesProvider.GetAllCoursesAsync(cancellationToken);
    }
}
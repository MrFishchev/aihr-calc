using System.Net.Mime;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Api.Services;
using Aihr.Calculator.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aihr.Calculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudiesController : ControllerBase
{
    private readonly IStudiesProvider _studiesProvider;
    private readonly IStudyEstimationService _studyEstimationService;
    private readonly ILogger<StudiesController> _logger;

    public StudiesController(
        ILogger<StudiesController> logger,
        IStudiesProvider studiesProvider,
        IStudyEstimationService studyEstimationService)
    {
        _logger = logger;
        _studiesProvider = studiesProvider;
        _studyEstimationService = studyEstimationService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<List<Study>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _studiesProvider.GetAllStudiesAsync(cancellationToken);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalculateStudyTime(
        [FromBody] Study study, 
        CancellationToken cancellationToken = default)
    {
        if (!study.Courses.Any())
        {
            _logger.LogInformation("Study does not contain any courses in {Request} request",
                HttpContext.TraceIdentifier);
            return BadRequest();
        }

        if (study.StartDate > study.EndDate)
        {
            _logger.LogInformation("Study's start date is bigger than end date in {Request}",
                HttpContext.TraceIdentifier);
            return BadRequest();
        }

        try
        {
            var estimateStudyTime = _studyEstimationService.EstimateHoursPerWeek(study);
            study.HoursPerWeek = estimateStudyTime.HoursPerWeek;
            await _studiesProvider.AddStudyAsync(study, cancellationToken);
            return Ok(estimateStudyTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add study {StudyId} in {Request}", study.Id, HttpContext.TraceIdentifier);
            return Problem();
        }
    }
}
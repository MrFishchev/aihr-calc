using System.Net.Mime;
using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Api.Services;
using Aihr.Calculator.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aihr.Calculator.Api.Controllers;

[ApiController]
[Route("{controller}")]
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
    public async Task<List<Study>> GetAll(CancellationToken cancellationToken)
    {
        return await _studiesProvider.GetAllStudiesAsync(cancellationToken);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddStudy(Study study, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(study.Id))
        {
            _logger.LogInformation("Study's id is null or empty in {Request} request",
                HttpContext.TraceIdentifier);
            return BadRequest();
        }

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
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add study {StudyId} in {Request}", study.Id, HttpContext.TraceIdentifier);
            return Problem();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aihr.Calculator.Api.Controllers;
using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Api.Services;
using Aihr.Calculator.Common.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Aihr.Calculator.Api.UnitTests.Controllers;

public class StudiesControllerTests
{
    private readonly Mock<IStudiesProvider> _studiesProvider = new();
    private readonly Mock<IStudyEstimationService> _studyEstimationService = new();
    private readonly StudiesController _sut;
    private readonly Study _study;
    
    public StudiesControllerTests()
    {
        _sut = new StudiesController(
            NullLogger<StudiesController>.Instance,
            _studiesProvider.Object,
            _studyEstimationService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        _study = new Study
        {
            Courses = new List<Course>
            {
                new() { Id = "Id", Duration = 1, Name = "Name" }
            },
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
        };
    }

    [Fact]
    public async Task CalculateStudyTime_CoursesAreEmpty_ReturnsBadRequest()
    {
        _study.Courses = Enumerable.Empty<Course>().ToList();

        var result = await _sut.CalculateStudyTime(_study);

        result.Should().BeOfType<BadRequestResult>();
    }
    
    [Fact]
    public async Task CalculateStudyTime_StartDateBiggerThanEndDate_ReturnsBadRequest()
    {
        _study.StartDate = DateTime.UtcNow.AddDays(15);

        var result = await _sut.CalculateStudyTime(_study);

        result.Should().BeOfType<BadRequestResult>();
    }
    
    [Fact]
    public async Task CalculateStudyTime_StudyEstimationServiceThrowsException_ReturnsProblem()
    {
        _studyEstimationService.Setup(x => x.EstimateHoursPerWeek(It.IsAny<Study>()))
            .Throws(new Exception());
        
        var result = await _sut.CalculateStudyTime(_study);

        Validate500Response(result);
    }
    
    [Fact]
    public async Task CalculateStudyTime_StudiesProviderThrowsException_ReturnsProblem()
    {
        _studiesProvider.Setup(x => x.AddStudyAsync(It.IsAny<Study>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception());
        
        var result = await _sut.CalculateStudyTime(_study);

       Validate500Response(result);
    }
    
    [Fact]
    public async Task CalculateStudyTime_WhenCalled_ReturnsEstimatedStudyTime()
    {
        var expected = new EstimatedStudyTime(1, 100, 1000);
        _studyEstimationService.Setup(x => x.EstimateHoursPerWeek(It.IsAny<Study>()))
            .Returns(() => expected);
        
        var result = await _sut.CalculateStudyTime(_study);

        (result as OkObjectResult)!.Value.Should().BeEquivalentTo(expected);
    }

    private static void Validate500Response(IActionResult result)
    {
        result.Should().BeOfType<ObjectResult>();
        (result as ObjectResult)!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
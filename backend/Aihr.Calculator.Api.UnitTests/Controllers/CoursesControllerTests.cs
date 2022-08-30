using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aihr.Calculator.Api.Controllers;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Common.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Aihr.Calculator.Api.UnitTests.Controllers;

public class CoursesControllerTests
{
    private readonly Mock<ICoursesProvider> _coursesProvider = new();
    private readonly CoursesController _sut;
    
    public CoursesControllerTests()
    {
        _sut = new CoursesController(
            _coursesProvider.Object,
            NullLogger<CoursesController>.Instance)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task GetAll_CoursesProviderThrowsException_ReturnsProblem()
    {
        _coursesProvider.Setup(x => x.GetAllCoursesAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception());
        
        var result = await _sut.GetAll();

       Validate500Response(result);
    }
    
    [Fact]
    public async Task GetAll_WhenCalled_ReturnsCoursesList()
    {
        var expected = new List<Course>
        {
            new() { Id = "1", Duration = 1, Name = "1" },
            new() { Id = "2", Duration = 2, Name = "2" },
        };
        _coursesProvider.Setup(x => x.GetAllCoursesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => expected);
        
        var result = await _sut.GetAll();

        (result as OkObjectResult)!.Value.Should().BeEquivalentTo(expected);
    }

    private static void Validate500Response(IActionResult result)
    {
        result.Should().BeOfType<ObjectResult>();
        (result as ObjectResult)!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
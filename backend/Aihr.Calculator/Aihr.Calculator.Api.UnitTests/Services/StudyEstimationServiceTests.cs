using System;
using System.Collections.Generic;
using System.Linq;
using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Api.Services;
using Aihr.Calculator.Common.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using FluentAssertions;

namespace Aihr.Calculator.Api.UnitTests.Services;

public class StudyEstimationServiceTests
{
    private readonly StudyEstimationService _sut;
    private readonly Study _study;

    public StudyEstimationServiceTests()
    {
        _sut = new StudyEstimationService(NullLogger<StudyEstimationService>.Instance);
        _study = new Study
        {
            Id = Guid.NewGuid().ToString(),
            Courses = new List<Course>(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1)
        };
    }

    [Fact]
    public void EstimateHoursPerWeek_StartDateBiggerThanEndDate_ThrowsArgumentException()
    {
        _study.StartDate = _study.EndDate.AddMonths(1);
        var act = () => _sut.EstimateHoursPerWeek(_study);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void EstimateHoursPerWeek_WhenCalled_ReturnsExpectedResult()
    {
        _study.Courses = new List<Course>
        {
            new() { Id = "1", Duration = 1, Name = "1" },
            new() { Id = "2", Duration = 10, Name = "1" },
            new() { Id = "3", Duration = 20, Name = "1" },
            new() { Id = "4", Duration = 30, Name = "1" },
            new() { Id = "5", Duration = 40, Name = "1" },
        };
        var totalDuration = GetSelectedCoursesDuration(_study);

        var result = _sut.EstimateHoursPerWeek(_study);
        
        ValidateHoursPerWeek(result, totalDuration);
    }
    
    [Fact]
    public void EstimateHoursPerWeek_SelectedRangeShorterThanWeek_ReturnsExpectedResult()
    {
        _study.EndDate = DateTime.UtcNow.AddDays(4);
        _study.Courses = new List<Course>
        {
            new() { Id = "1", Duration = 1, Name = "1" },
            new() { Id = "2", Duration = 10, Name = "1" },
            new() { Id = "3", Duration = 20, Name = "1" },
            new() { Id = "4", Duration = 30, Name = "1" },
            new() { Id = "5", Duration = 40, Name = "1" },
        };
        var totalDuration = GetSelectedCoursesDuration(_study);

        var result = _sut.EstimateHoursPerWeek(_study);
        
       ValidateHoursPerWeek(result, totalDuration);
    }
    
    [Fact]
    public void EstimateHoursPerWeek_CoursesDurationMuchShorterThanSelectedRange_ReturnsExpectedResult()
    {
        _study.EndDate = DateTime.UtcNow.AddDays(4);
        _study.Courses = new List<Course>
        {
            new() { Id = "1", Duration = 2, Name = "1" },
        };
        var totalDuration = GetSelectedCoursesDuration(_study);

        var result = _sut.EstimateHoursPerWeek(_study);
        
        ValidateHoursPerWeek(result, totalDuration);
    }

    #region Private Methods

    private static void ValidateHoursPerWeek(
        EstimatedStudyTime estimatedStudyTime,
        int totalDuration)
    {
        var totalTimeToStudy = estimatedStudyTime.Weeks * estimatedStudyTime.HoursPerWeek;
        var deltaTime = totalTimeToStudy - totalDuration;
        deltaTime.Should().Be(0);
    }

    private static int GetSelectedCoursesDuration(Study study)
    {
        return study.Courses.Select(x => x.Duration).Sum();
    }

    #endregion
}
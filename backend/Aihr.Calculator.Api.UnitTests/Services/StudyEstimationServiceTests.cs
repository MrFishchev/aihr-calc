using System;
using System.Collections.Generic;
using System.Linq;
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
    public void EstimateHoursPerWeek_SelectedRangeShorterThanWeek_ReturnsCoursesDuration()
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

        var selectedRange = _study.EndDate - _study.StartDate;
        result.Weeks.Should().Be(1);
        result.HoursPerWeek.Should().Be(totalDuration);
        result.HoursPerDay.Should().BeCloseTo((int)(totalDuration / selectedRange.TotalDays), 1);
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
        
        result.Weeks.Should().Be(1);
        result.HoursPerWeek.Should().Be(totalDuration);
        result.HoursPerDay.Should().Be(1);
    }
    
    [Fact]
    public void EstimateHoursPerWeek_OneDayRangeSelected_ReturnsTotalDurationPerWeekAndHour()
    {
        _study.EndDate = DateTime.UtcNow;
        _study.Courses = new List<Course>
        {
            new() { Id = "1", Duration = 2, Name = "1" },
            new() { Id = "2", Duration = 3, Name = "2" },
        };
        
        var result = _sut.EstimateHoursPerWeek(_study);
        
        result.Weeks.Should().Be(1);
        result.HoursPerWeek.Should().Be(5);
        result.HoursPerDay.Should().Be(5);
    }

    #region Private Methods

    private static int GetSelectedCoursesDuration(Study study)
    {
        return study.Courses.Select(x => x.Duration).Sum();
    }

    #endregion
}
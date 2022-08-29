using System.Security.Principal;
using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Services;

public class StudyEstimationService : IStudyEstimationService
{
    private readonly ILogger<StudyEstimationService> _logger;
    private const int DaysPerWeek = 7;
    private const int MaxHoursPerDay = 24;
    private const int MaxHoursPerWeek = MaxHoursPerDay * DaysPerWeek;

    public StudyEstimationService(ILogger<StudyEstimationService> logger)
    {
        _logger = logger;
    }

    public EstimatedStudyTime EstimateHoursPerWeek(Study study)
    {
        if (study.StartDate > study.EndDate)
        {
            throw new ArgumentException("Start date cannot be bigger than end date");
        }
        
        var totalDuration = study.Courses.Select(x => x.Duration).Sum();
        var availableTime = study.EndDate - study.StartDate;
        var hoursPerDay = totalDuration / Math.Max(availableTime.TotalDays, 1);

        // selected study range and total duration are less than one week
        if (totalDuration <= MaxHoursPerWeek && availableTime.TotalDays / DaysPerWeek <= 1)
        {
            return EstimatedStudyTime.SingleWeek(hoursPerWeek: totalDuration, (int)Math.Ceiling(hoursPerDay));
        }

        var hoursPerWeek = (int)Math.Ceiling(hoursPerDay * DaysPerWeek); // 2.3 hours will be 3 hours at the end
        
        var weeks = (int)Math.Ceiling((double)totalDuration / MaxHoursPerWeek);

        if (hoursPerWeek > MaxHoursPerWeek)
        {
            _logger.LogWarning("Estimated time per week is bigger than {MaxHoursPerWeek}", MaxHoursPerWeek);
        }

        _logger.LogDebug("Estimated time for {CoursesCount} with {Duration}: {EstimatedTime}",
            study.Courses.Count, totalDuration, hoursPerWeek);
        
        return new EstimatedStudyTime(weeks, hoursPerWeek, (int)Math.Ceiling(hoursPerDay));
    }
}
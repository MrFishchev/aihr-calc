using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Services;

/// <summary>
/// Estimation service for <see cref="Study"/>
/// </summary>
public interface IStudyEstimationService
{
    /// <summary>
    /// Estimates how many hours (per week, per day) is needed to be spent to finish selected courses
    /// <see cref="EstimatedStudyTime"/>
    /// </summary>
    /// <param name="study">Study to be estimated</param>
    /// <returns></returns>
    EstimatedStudyTime EstimateHoursPerWeek(Study study);
}
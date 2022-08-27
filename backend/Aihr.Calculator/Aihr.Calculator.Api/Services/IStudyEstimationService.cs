using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Services;

public interface IStudyEstimationService
{
    EstimatedStudyTime EstimateHoursPerWeek(Study study);
}
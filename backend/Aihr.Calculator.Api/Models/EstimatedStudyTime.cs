namespace Aihr.Calculator.Api.Models;

/// <summary>
/// Handles response to a client with estimated study time
/// </summary>
public record EstimatedStudyTime
{
    /// <summary>
    /// Indicates how many weeks is needed minimum (e.g. if total duration is longer than week time)
    /// </summary>
    public int Weeks { get; }

    /// <summary>
    /// Indicated how many hours per week (number of days, not a calendar week) client should study 
    /// </summary>
    public int HoursPerWeek { get; }

    /// <summary>
    /// Useful in case of not-full weeks selected ranges
    /// </summary>
    public int HoursPerDay { get; }

    public EstimatedStudyTime(int weeks, int hoursPerWeek, int hoursPerDay)
    {
        if (weeks < 1)
        {
            throw new ArgumentException("Number of weeks cannot be less than 1", nameof(weeks));
        }

        Weeks = weeks;
        HoursPerWeek = hoursPerWeek;
        HoursPerDay = hoursPerDay;
    }
    
    public static EstimatedStudyTime SingleWeek(int hoursPerWeek, int hoursPerDay)
    {
        return new EstimatedStudyTime(1, hoursPerWeek, hoursPerDay);
    }
}
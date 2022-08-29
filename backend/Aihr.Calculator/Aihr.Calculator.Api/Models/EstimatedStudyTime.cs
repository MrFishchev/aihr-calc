namespace Aihr.Calculator.Api.Models;

public record EstimatedStudyTime
{
    /// <summary>
    /// Indicates how many weeks is needed minimum (e.g. if total duration is longer than week time)
    /// </summary>
    public int Weeks { get; }

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
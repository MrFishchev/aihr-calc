namespace Aihr.Calculator.Api.Models;

public record EstimatedStudyTime
{
    public int Weeks { get; }

    public int HoursPerWeek { get; }

    public EstimatedStudyTime(int weeks, int hoursPerWeek)
    {
        if (weeks < 1)
        {
            throw new ArgumentException("Number of weeks cannot be less than 1", nameof(weeks));
        }

        Weeks = weeks;
        HoursPerWeek = hoursPerWeek;
    }
    
    public static EstimatedStudyTime SingleWeek(int hoursPerWeek)
    {
        return new EstimatedStudyTime(1, hoursPerWeek);
    }
}
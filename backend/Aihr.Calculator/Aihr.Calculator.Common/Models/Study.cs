using System.Text.Json.Serialization;
using Aihr.Calculator.Common.Converters;
using Amazon.DynamoDBv2.DataModel;

namespace Aihr.Calculator.Common.Models;

[DynamoDBTable("studies")]
[Serializable]
public class Study
{
    [DynamoDBHashKey("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [DynamoDBProperty("courses")]
    [JsonPropertyName("courses")]
    public List<Course> Courses { get; set; } = null!;
    
    [DynamoDBProperty("start_date", typeof(DateTimeUtcConverter))]
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    
    [DynamoDBProperty("end_date", typeof(DateTimeUtcConverter))]
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
    
    [DynamoDBProperty("hours_per_week")]
    [JsonPropertyName("hoursPerWeek")]
    public int? HoursPerWeek { get; set; }
}
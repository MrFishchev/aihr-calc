using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;

namespace Aihr.Calculator.Common.Models;

[DynamoDBTable("courses")]
[Serializable]
public class Course
{
    [DynamoDBHashKey("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [DynamoDBProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [DynamoDBProperty("duration")]
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}
using Aihr.Calculator.Common.Models;
using Amazon.DynamoDBv2.DataModel;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

public class CoursesProvider : ICoursesProvider
{
    private readonly IDynamoDBContext _dynamoDb;

    public CoursesProvider(IDynamoDBContext dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }
    
    public async Task<List<Course>> GetAllCoursesAsync(CancellationToken cancellationToken)
    {
        return await _dynamoDb.ScanAsync<Course>(default).GetRemainingAsync(cancellationToken);
    }
}
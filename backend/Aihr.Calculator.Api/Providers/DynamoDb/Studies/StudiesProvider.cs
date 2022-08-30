using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;
using Amazon.DynamoDBv2.DataModel;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

public class StudiesProvider : IStudiesProvider
{
    private readonly IDynamoDBContext _dynamoDb;

    public StudiesProvider(IDynamoDBContext dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<List<Study>> GetAllStudiesAsync(CancellationToken cancellationToken)
    {
        return await _dynamoDb.ScanAsync<Study>(default).GetRemainingAsync(cancellationToken);
    }

    public async Task AddStudyAsync(Study study, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(study.Id))
        {
            study.Id = Guid.NewGuid().ToString();
        }
        await _dynamoDb.SaveAsync(study, cancellationToken);
    }
}
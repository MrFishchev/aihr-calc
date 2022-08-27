using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

public interface IStudiesProvider
{
    Task<List<Study>> GetAllStudiesAsync(CancellationToken cancellationToken);

    Task AddStudyAsync(Study study, CancellationToken cancellationToken);
}
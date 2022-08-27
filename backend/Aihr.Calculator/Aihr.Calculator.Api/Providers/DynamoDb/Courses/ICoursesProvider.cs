using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

public interface ICoursesProvider
{
    Task<List<Course>> GetAllCoursesAsync(CancellationToken cancellationToken);
}
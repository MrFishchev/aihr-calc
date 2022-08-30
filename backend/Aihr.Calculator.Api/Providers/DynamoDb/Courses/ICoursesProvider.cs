using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

/// <summary>
/// Provides CRUD operations for the <see cref="Course"/> entity
/// </summary>
public interface ICoursesProvider
{
    /// <summary>
    /// Get the list of all available courses
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Course>> GetAllCoursesAsync(CancellationToken cancellationToken);
}
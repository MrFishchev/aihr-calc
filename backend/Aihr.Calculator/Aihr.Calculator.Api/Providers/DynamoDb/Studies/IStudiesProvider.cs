using Aihr.Calculator.Api.Models;
using Aihr.Calculator.Common.Models;

namespace Aihr.Calculator.Api.Providers.DynamoDb;

/// <summary>
/// Provides CRUD operations for the <see cref="Study"/> entity
/// </summary>
public interface IStudiesProvider
{
    /// <summary>
    /// Get the history of all estimated studies
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Study>> GetAllStudiesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Saves estimated study to the DB
    /// </summary>
    /// <param name="study">Estimated study instance</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddStudyAsync(Study study, CancellationToken cancellationToken);
}
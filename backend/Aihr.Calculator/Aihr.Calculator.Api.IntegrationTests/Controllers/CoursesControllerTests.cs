using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Aihr.Calculator.Common.Models;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aihr.Calculator.Api.IntegrationTests.Controllers;

public class CoursesControllerTests : IClassFixture<MockedApp>, IAsyncDisposable
{
    private readonly MockedApp _app = new();
    private readonly HttpClient _httpClient;
    private readonly IDynamoDBContext _dynamoDbContext;
    private const string CoursesController = "/courses"; 

    private readonly Course _course = new()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "TestCaseController",
        Duration = 2
    };

    public CoursesControllerTests()
    {
        _httpClient = _app.CreateClient();
        _dynamoDbContext = _app.Services.GetRequiredService<IDynamoDBContext>();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _dynamoDbContext.DeleteAsync(_course);
    }
    
    [Fact]
    public async Task Get_CourseExists_ReturnsListWithTheCourse()
    {
        await _dynamoDbContext.SaveAsync(_course);
        
        var response = await _httpClient.GetAsync(CoursesController);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var courses = await GetCoursesListFromResponse(response);
        courses.Should().ContainEquivalentOf(_course);
    }
    
    [Fact]
    public async Task Get_CourseDoesNotExist_ReturnsListWithTheCourse()
    {
        var response = await _httpClient.GetAsync(CoursesController);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var courses = await GetCoursesListFromResponse(response);
        courses.Should().NotContainEquivalentOf(_course);
    }

    private static async Task<List<Course>?> GetCoursesListFromResponse(HttpResponseMessage response)
    {
        var rawResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Course>>(rawResponse);
    }
}
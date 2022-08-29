using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aihr.Calculator.Common.Models;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aihr.Calculator.Api.IntegrationTests.Controllers;

public class StudiesControllerTests : IClassFixture<MockedApp>, IAsyncDisposable
{
    private readonly MockedApp _app = new();
    private readonly HttpClient _httpClient;
    private readonly IDynamoDBContext _dynamoDbContext;
    private const string StudiesController = "/studies"; 

    private readonly Course _course = new()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "TestStudiesController",
        Duration = 2
    };

    private readonly Study _study;

    public StudiesControllerTests()
    {
        _httpClient = _app.CreateClient();
        _dynamoDbContext = _app.Services.GetRequiredService<IDynamoDBContext>();

        _study = new Study
        {
            Id = Guid.NewGuid().ToString(),
            Courses = new List<Course> { _course },
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };
    }
    
    public async ValueTask DisposeAsync()
    {
        await _dynamoDbContext.DeleteAsync(_study);
        await _dynamoDbContext.DeleteAsync(_course);
    }

    #region GetAll

    [Fact]
    public async Task GetAll_StudyExists_ReturnsListWithTheStudies()
    {
        await _dynamoDbContext.SaveAsync(_course);
        await _dynamoDbContext.SaveAsync(_study);
        
        var response = await _httpClient.GetAsync(StudiesController);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var studies = await GetObjectFromResponse<List<Study>>(response);
        studies.Should().Contain(x => x.Id == _study.Id && x.Courses.First().Id == _study.Courses.First().Id);
    }
    
    [Fact]
    public async Task GetAll_CourseDoesNotExist_ReturnsListWithTheCourse()
    {
        var response = await _httpClient.GetAsync(StudiesController);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var studies = await GetObjectFromResponse<List<Study>>(response);
        studies.Should().NotContain(x => x.Id == _study.Id);
    }
    
    #endregion

    #region AddStudy

    [Fact]
    public async Task AddStudy_CoursesAreEmpty_ReturnsBadRequest()
    {
        var study = new Study
        {
            Id = Guid.NewGuid().ToString(),
            Courses = Enumerable.Empty<Course>().ToList(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _httpClient.PostAsync(StudiesController, 
            new StringContent(JsonSerializer.Serialize(study), Encoding.UTF8, MediaTypeNames.Application.Json));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task AddStudy_StartDateBiggerThanEndDate_ReturnsBadRequest()
    {
        var study = new Study
        {
            Id = Guid.NewGuid().ToString(),
            Courses = new List<Course>{_course},
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow
        };

        var response = await _httpClient.PostAsync(StudiesController, 
            new StringContent(JsonSerializer.Serialize(study), Encoding.UTF8, MediaTypeNames.Application.Json));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddStudy_WhenCalled_AddsValidEntityToDb()
    {
        await _dynamoDbContext.DeleteAsync<Study>(_study.Id);
        var response = await _httpClient.PostAsync(StudiesController, 
            new StringContent(JsonSerializer.Serialize(_study), Encoding.UTF8, MediaTypeNames.Application.Json));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdStudy = await _dynamoDbContext.LoadAsync<Study>(_study.Id);
        createdStudy.Should().NotBeNull();
        createdStudy.Courses.Should().BeEquivalentTo(_study.Courses);
        createdStudy.Id.Should().BeEquivalentTo(_study.Id);
        createdStudy.EndDate.Should().BeCloseTo(_study.EndDate, TimeSpan.FromMilliseconds(100));
        createdStudy.StartDate.Should().BeCloseTo(_study.StartDate, TimeSpan.FromMilliseconds(100));
        createdStudy.HoursPerWeek.Should().Be(2);
    }

    #endregion

    #region Private Methods

    private static async Task<T?> GetObjectFromResponse<T>(HttpResponseMessage response)
    {
        var rawResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(rawResponse);
    }
    
    #endregion
}
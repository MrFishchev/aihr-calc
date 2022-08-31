using System.Text.Json.Serialization;
using Aihr.Calculator.Api.Providers.DynamoDb;
using Aihr.Calculator.Api.Services;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LocalStack.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalStack(builder.Configuration);
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAwsService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

// Register DynamoDb Providers
builder.Services.AddScoped<IStudiesProvider, StudiesProvider>();
builder.Services.AddScoped<ICoursesProvider, CoursesProvider>();

// Register Services
builder.Services.AddScoped<IStudyEstimationService, StudyEstimationService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}
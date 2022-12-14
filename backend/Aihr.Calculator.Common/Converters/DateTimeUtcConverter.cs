using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Aihr.Calculator.Common.Converters;

/// <summary>
/// Converts DateTime representation of .NET to DynamoDB representation
/// </summary>
public class DateTimeUtcConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value) => (DateTime)value;

    public object FromEntry(DynamoDBEntry entry)
    {
        var dateTime = entry.AsDateTime();
        return dateTime.ToUniversalTime();
    }
}
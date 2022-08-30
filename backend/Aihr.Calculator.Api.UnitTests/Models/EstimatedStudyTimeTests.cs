using System;
using Aihr.Calculator.Api.Models;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace Aihr.Calculator.Api.UnitTests.Models;

public class EstimatedStudyTimeTests
{
    [Fact]
    public void EstimatedStudyTime_WeeksLessThanOne_ThrowsArgumentException()
    {
        Action act = () => new EstimatedStudyTime(0, 1, 1);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void EstimatedStudyTime_SingleWeekCalled_ReturnsExpected()
    {
        var expected = new EstimatedStudyTime(1, 3, 5);

        EstimatedStudyTime.SingleWeek(3, 5).Should().BeEquivalentTo(expected);
    }
}
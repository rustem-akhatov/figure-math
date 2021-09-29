using System;
using AutoFixture;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class CircleInfoTests
    {
        private readonly Fixture _fixture;
        
        public CircleInfoTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Area_ShouldUseRightFormula_WhenCalled()
        {
            // Arrange
            var circle = _fixture.Create<CircleInfo>();
            
            double expectedArea = Math.PI * Math.Pow(circle.Radius, 2);

            // Act
            double actualArea = circle.Area;

            // Assert
            Assert.Equal(expectedArea, actualArea);
        }

        [Fact]
        public void Radius_ShouldAcceptValue_WhenPositiveNumber()
        {
            // Arrange
            var radius = _fixture.Create<double>();

            // Act
            var circle = new CircleInfo
            {
                Radius = radius
            };

            // Assert
            Assert.Equal(radius, circle.Radius);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Radius_ShouldThrowException_WhenZeroOrNegativeNumber(int sign)
        {
            // Arrange
            var radius = _fixture.Create<double>() * sign;

            // Act
            var circle = new CircleInfo();

            Exception exception = Record.Exception(() => circle.Radius = radius);

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
    }
}
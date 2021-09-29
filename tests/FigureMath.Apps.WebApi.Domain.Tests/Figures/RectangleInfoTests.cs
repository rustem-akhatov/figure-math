using System;
using AutoFixture;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class RectangleInfoTests
    {
        private readonly Fixture _fixture;
        
        public RectangleInfoTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Area_ShouldUseRightFormula_WhenCalled()
        {
            // Arrange
            var rectangle = _fixture.Create<RectangleInfo>();

            double expectedArea = rectangle.Width * rectangle.Length;

            // Act
            double actualArea = rectangle.Area;

            // Assert
            Assert.Equal(expectedArea, actualArea);
        }
        
        [Fact]
        public void Length_ShouldAcceptValue_WhenPositiveNumber()
        {
            // Arrange
            var length = _fixture.Create<double>();

            // Act
            var rectangle = new RectangleInfo
            {
                Length = length
            };

            // Assert
            Assert.Equal(length, rectangle.Length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Length_ShouldThrowException_WhenZeroOrNegativeNumber(int sign)
        {
            // Arrange
            var length = _fixture.Create<double>() * sign;

            // Act
            var rectangle = new RectangleInfo();

            Exception exception = Record.Exception(() => rectangle.Length = length);

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
        
        [Fact]
        public void Width_ShouldAcceptValue_WhenPositiveNumber()
        {
            // Arrange
            var width = _fixture.Create<double>();

            // Act
            var rectangle = new RectangleInfo
            {
                Width = width
            };

            // Assert
            Assert.Equal(width, rectangle.Width);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Width_ShouldThrowException_WhenZeroOrNegativeNumber(int sign)
        {
            // Arrange
            var width = _fixture.Create<double>() * sign;

            // Act
            var rectangle = new RectangleInfo();

            Exception exception = Record.Exception(() => rectangle.Width = width);

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
    }
}
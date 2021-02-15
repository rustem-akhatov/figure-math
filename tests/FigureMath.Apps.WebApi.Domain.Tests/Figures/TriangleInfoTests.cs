using System;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures
{
    public class TriangleInfoTests
    {
        private readonly Fixture _fixture;
        
        public TriangleInfoTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Area_ShouldUseRightFormula_WhenCalled()
        {
            // Arrange
            var triangle = _fixture.Create<TriangleInfo>();

            double expectedArea = triangle.Height * triangle.Base / 2;

            // Act
            double actualArea = triangle.Area;

            // Assert
            Assert.Equal(expectedArea, actualArea);
        }
        
        [Fact]
        public void Base_ShouldAcceptValue_WhenPositiveNumber()
        {
            // Arrange
            var baseValue = _fixture.Create<double>();

            // Act
            var triangle = new TriangleInfo
            {
                Base = baseValue
            };

            // Assert
            Assert.Equal(baseValue, triangle.Base);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Base_ShouldThrowException_WhenZeroOrNegativeNumber(int sign)
        {
            // Arrange
            var baseValue = _fixture.Create<double>() * sign;

            // Act
            var triangle = new TriangleInfo();

            Exception exception = Record.Exception(() => triangle.Base = baseValue);

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
        
        [Fact]
        public void Height_ShouldAcceptValue_WhenPositiveNumber()
        {
            // Arrange
            var height = _fixture.Create<double>();

            // Act
            var triangle = new TriangleInfo
            {
                Height = height
            };

            // Assert
            Assert.Equal(height, triangle.Height);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Height_ShouldThrowException_WhenZeroOrNegativeNumber(int sign)
        {
            // Arrange
            var height = _fixture.Create<double>() * sign;

            // Act
            var triangle = new TriangleInfo();

            Exception exception = Record.Exception(() => triangle.Height = height);

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
    }
}
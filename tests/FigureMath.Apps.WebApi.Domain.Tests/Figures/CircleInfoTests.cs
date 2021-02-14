using System;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures
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
    }
}
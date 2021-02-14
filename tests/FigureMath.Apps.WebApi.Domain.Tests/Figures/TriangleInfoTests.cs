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
    }
}
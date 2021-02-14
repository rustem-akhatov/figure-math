using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures
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
    }
}
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;
using FluentAssertions;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures.Descriptors
{
    public class RectangleDescriptorTests
    {
        private readonly RectangleDescriptor _rectangleDescriptor;
        
        public RectangleDescriptorTests()
        {
            _rectangleDescriptor = new RectangleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnRectangle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _rectangleDescriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Rectangle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnRectangleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _rectangleDescriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(RectangleInfo.PropNames.All);
        }
    }
}
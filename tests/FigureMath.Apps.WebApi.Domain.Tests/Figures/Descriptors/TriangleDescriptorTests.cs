using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;
using FluentAssertions;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures.Descriptors
{
    public class TriangleDescriptorTests
    {
        private readonly TriangleDescriptor _triangleDescriptor;
        
        public TriangleDescriptorTests()
        {
            _triangleDescriptor = new TriangleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnTriangle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _triangleDescriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Triangle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnTriangleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _triangleDescriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(TriangleInfo.PropNames.All);
        }
    }
}
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;
using FluentAssertions;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures.Descriptors
{
    public class CircleDescriptorTests
    {
        private readonly CircleDescriptor _circleDescriptor;
        
        public CircleDescriptorTests()
        {
            _circleDescriptor = new CircleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnCircle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _circleDescriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Circle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnCircleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _circleDescriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(CircleInfo.PropNames.All);
        }
    }
}
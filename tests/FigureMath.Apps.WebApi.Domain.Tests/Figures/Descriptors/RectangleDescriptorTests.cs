using System.Collections.Generic;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures.Descriptors
{
    public class RectangleDescriptorTests
    {
        private readonly Fixture _fixture;
        
        private readonly RectangleDescriptor _descriptor;
        
        public RectangleDescriptorTests()
        {
            _fixture = new Fixture();
            
            _descriptor = new RectangleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnRectangle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _descriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Rectangle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnRectangleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _descriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(RectangleInfo.PropNames.All);
        }

        [Fact]
        public void ValidateProps_ShouldReturnValid_WhenPropsValid()
        {
            // Arrange
            double width = _fixture.Create<double>();
            double length = _fixture.Create<double>();

            var props = new Dictionary<string, double>
            {
                { RectangleInfo.PropNames.Width, width },
                { RectangleInfo.PropNames.Length, length }
            };
            
            // Act
            ValidationResult validationResult = _descriptor.ValidateProps(props);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData(0, 0, 2)]
        [InlineData(1, -1, 1)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, -1, 2)]
        public void ValidateProps_ShouldReturnInvalid_WhenPropsInvalid(int widthSign, int lengthSign, int errorsCount)
        {
            // Arrange
            double width = _fixture.Create<double>() * widthSign;
            double length = _fixture.Create<double>() * lengthSign;

            var props = new Dictionary<string, double>
            {
                { RectangleInfo.PropNames.Width, width },
                { RectangleInfo.PropNames.Length, length }
            };

            // Act
            ValidationResult validationResult = _descriptor.ValidateProps(props);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            
            Assert.Equal(errorsCount, validationResult.Errors.Count);
        }
    }
}
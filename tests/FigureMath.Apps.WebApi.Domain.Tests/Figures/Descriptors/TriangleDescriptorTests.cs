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
    public class TriangleDescriptorTests
    {
        private readonly Fixture _fixture;
        
        private readonly TriangleDescriptor _descriptor;
        
        public TriangleDescriptorTests()
        {
            _fixture = new Fixture();
            
            _descriptor = new TriangleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnTriangle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _descriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Triangle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnTriangleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _descriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(TriangleInfo.PropNames.All);
        }

        [Fact]
        public void ValidateProps_ShouldReturnValid_WhenPropsValid()
        {
            // Arrange
            double baseValue = _fixture.Create<double>();
            double height = _fixture.Create<double>();

            var props = new Dictionary<string, double>
            {
                { TriangleInfo.PropNames.Base, baseValue },
                { TriangleInfo.PropNames.Height, height }
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
        public void ValidateProps_ShouldReturnInvalid_WhenPropsInvalid(int baseSign, int heightSign, int errorsCount)
        {
            // Arrange
            double baseValue = _fixture.Create<double>() * baseSign;
            double height = _fixture.Create<double>() * heightSign;

            var props = new Dictionary<string, double>
            {
                { TriangleInfo.PropNames.Base, baseValue },
                { TriangleInfo.PropNames.Height, height }
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
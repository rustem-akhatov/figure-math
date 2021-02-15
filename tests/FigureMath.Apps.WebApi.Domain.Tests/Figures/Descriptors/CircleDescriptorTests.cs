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
    public class CircleDescriptorTests
    {
        private readonly Fixture _fixture;
        
        private readonly CircleDescriptor _descriptor;
        
        public CircleDescriptorTests()
        {
            _fixture = new Fixture();
            
            _descriptor = new CircleDescriptor();
        }
        
        [Fact]
        public void FigureType_ShouldReturnCircle_WhenCalled()
        {
            // Arrange
            
            // Act
            FigureType actualFigureType = _descriptor.FigureType;

            // Assert
            Assert.Equal(FigureType.Circle, actualFigureType);
        }

        [Fact]
        public void RequiredProps_ShouldReturnCircleInfoPropNamesAll_WhenCalled()
        {
            // Arrange
            
            // Act
            string[] requiredProps = _descriptor.RequiredProps;

            // Assert
            requiredProps.Should().Equal(CircleInfo.PropNames.All);
        }

        [Fact]
        public void ValidateProps_ShouldReturnValid_WhenPropsValid()
        {
            // Arrange
            double radius = _fixture.Create<double>();

            var props = new Dictionary<string, double>
            {
                { CircleInfo.PropNames.Radius, radius }
            };
            
            // Act
            ValidationResult validationResult = _descriptor.ValidateProps(props);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidateProps_ShouldReturnInvalid_WhenPropsInvalid(int radiusSign)
        {
            // Arrange
            double radius = _fixture.Create<double>() * radiusSign;

            var props = new Dictionary<string, double>
            {
                { CircleInfo.PropNames.Radius, radius }
            };

            // Act
            ValidationResult validationResult = _descriptor.ValidateProps(props);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
        }
    }
}
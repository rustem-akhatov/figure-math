using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;
using FluentValidation.Results;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures.Descriptors
{
    public class FigureDescriptorBaseTests
    {
        private readonly Fixture _fixture;

        public FigureDescriptorBaseTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ValidateProps_ShouldReturnValid_WhenAllPropsSpecified()
        {
            // Arrange
            string[] requiredProps = _fixture.CreateMany<string>().ToArray();

            Dictionary<string, double> figureProps = requiredProps.ToDictionary(prop => prop, _ => _fixture.Create<double>());
            
            TestFigureDescriptor descriptor = CreateDescriptor(requiredProps);
            
            // Act
            ValidationResult validationResult = descriptor.ValidateProps(figureProps);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
            
            Assert.Equal(1, descriptor.ValidatePropsValuesCalledCount);
        }

        [Fact]
        public void ValidateProps_ShouldReturnInvalid_WhenSomePropMissed()
        {
            // Arrange
            int[] propsCounts = _fixture.CreateMany<int>(2).ToArray();

            int requiredPropsCount = propsCounts.Max();
            int presentedPropsCount = propsCounts.Min();

            string[] requiredProps = _fixture.CreateMany<string>(requiredPropsCount).ToArray();

            Dictionary<string, double> figureProps = requiredProps
                .Take(presentedPropsCount)
                .ToDictionary(prop => prop, _ => _fixture.Create<double>());

            TestFigureDescriptor descriptor = CreateDescriptor(requiredProps);

            // Act
            ValidationResult validationResult = descriptor.ValidateProps(figureProps);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);

            Assert.Equal(requiredPropsCount - presentedPropsCount, validationResult.Errors.Count);

            Assert.All(validationResult.Errors, failure => Assert.Contains(failure.PropertyName, requiredProps));
            Assert.All(validationResult.Errors, failure => Assert.Contains("is not specified", failure.ErrorMessage));

            Assert.Equal(0, descriptor.ValidatePropsValuesCalledCount);
        }

        [Fact]
        public void ValidateProps_ShouldReturnInvalid_WhenTooManyPropsSpecified()
        {
            // Arrange
            int[] propsCounts = _fixture.CreateMany<int>(2).ToArray();

            int presentedPropsCount = propsCounts.Max();
            int requiredPropsCount = propsCounts.Min();

            string[] presentedProps = _fixture.CreateMany<string>(presentedPropsCount).ToArray();
            
            string[] requiredProps = presentedProps.Take(requiredPropsCount).ToArray();

            Dictionary<string, double> figureProps = presentedProps.ToDictionary(prop => prop, _ => _fixture.Create<double>());

            TestFigureDescriptor descriptor = CreateDescriptor(requiredProps);

            // Act
            ValidationResult validationResult = descriptor.ValidateProps(figureProps);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);

            Assert.Equal(presentedPropsCount - requiredPropsCount, validationResult.Errors.Count);

            Assert.All(validationResult.Errors, failure => Assert.Contains(failure.PropertyName, presentedProps));
            Assert.All(validationResult.Errors, failure => Assert.Contains("is redundant", failure.ErrorMessage));

            Assert.Equal(0, descriptor.ValidatePropsValuesCalledCount);
        }

        private TestFigureDescriptor CreateDescriptor(string[] requiredProps)
        {
            return new TestFigureDescriptor(_fixture.Create<FigureType>(), requiredProps);
        }
        
        private class TestFigureDescriptor : FigureDescriptorBase
        {
            public TestFigureDescriptor(FigureType figureType, string[] requiredProps)
                : base(figureType, requiredProps)
            { }

            public int ValidatePropsValuesCalledCount { get; private set; }

            protected override ValidationResult ValidatePropsValues(IDictionary<string, double> figureProps)
            {
                ValidatePropsValuesCalledCount++;
                
                return new ValidationResult();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Apps.WebApi.Models.Figures;
using FigureMath.Data.Enums;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Tests.Models.Figures
{
    public class PostFigureModelValidatorTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFigureDescriptor> _figureDescriptorMock;
        
        private readonly PostFigureModelValidator _validator;

        public PostFigureModelValidatorTests()
        {
            _fixture = new Fixture();

            _figureDescriptorMock = new Mock<IFigureDescriptor>();

            var figureDescriptorProviderMock = new Mock<IFigureDescriptorProvider>();
            figureDescriptorProviderMock
                .Setup(provider => provider.GetDescriptorFor(It.IsAny<FigureType>()))
                .Returns(_figureDescriptorMock.Object);

            _validator = new PostFigureModelValidator(figureDescriptorProviderMock.Object);
        }
        
        [Fact]
        public void Validate_ShouldReturnValid_WhenAllPropsFilledCorrectly()
        {
            // Arrange
            var model = _fixture.Create<PostFigureModel>();

            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(model.FigureProps.Keys.ToArray());

            _figureDescriptorMock
                .Setup(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string, double>>()))
                .Returns(new ValidationResult());

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
            
            _figureDescriptorMock.Verify(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string,double>>()), Times.Once);
        }
        
        [Fact]
        public void Validate_ShouldReturnInvalid_WhenAllPropsFilledButIncorrectly()
        {
            // Arrange
            var model = _fixture.Create<PostFigureModel>();

            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(model.FigureProps.Keys.ToArray());

            _figureDescriptorMock
                .Setup(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string, double>>()))
                .Returns(new ValidationResult(_fixture.CreateMany<ValidationFailure>()));

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            
            _figureDescriptorMock.Verify(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string,double>>()), Times.Once);
        }

        [Fact]
        public void Validate_ShouldReturnCorrectPropertyName_WhenInvalid()
        {
            // Arrange
            var model = _fixture
                .Build<PostFigureModel>()
                .With(obj => obj.FigureProps, new Dictionary<string, double>())
                .Create();
            
            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(_fixture.CreateMany<string>().ToArray());

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            
            ValidationFailure failure = validationResult.Errors[0];
            
            Assert.Equal(nameof(PostFigureModel.FigureProps), failure.PropertyName);
        }

        [Fact]
        public void Validate_ShouldReturnErrorWithAllProps_WhenNoPropsFilled()
        {
            // Arrange
            var model = _fixture
                .Build<PostFigureModel>()
                .With(obj => obj.FigureProps, new Dictionary<string, double>())
                .Create();

            string[] requiredProps = _fixture.CreateMany<string>().ToArray();

            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(requiredProps);

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);

            ValidationFailure failure = validationResult.Errors[0];
            
            Assert.All(requiredProps, prop => Assert.Contains(prop, failure.ErrorMessage));
            
            _figureDescriptorMock.Verify(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string,double>>()), Times.Never);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenFilledPropsAreGreaterThanRequiredProps()
        {
            // Arrange
            var model = _fixture.Create<PostFigureModel>();
            
            string[] requiredProps = _fixture.CreateMany<string>(1).ToArray();
            
            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(requiredProps);

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);

            ValidationFailure failure = validationResult.Errors[0];
            
            Assert.All(requiredProps, prop => Assert.Contains(prop, failure.ErrorMessage));
            
            _figureDescriptorMock.Verify(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string,double>>()), Times.Never);
        }

        [Fact]
        public void Validate_ShouldReturnErrorWithNotFilledProps_WhenOnlySomePropsFilled()
        {
            // Arrange
            const int takePropsCount = 1;
            
            string[] requiredProps = _fixture.CreateMany<string>().ToArray();

            var model = _fixture
                .Build<PostFigureModel>()
                .With(obj => obj.FigureProps, requiredProps.Take(takePropsCount).ToDictionary(prop => prop, _ => _fixture.Create<double>()))
                .Create();
            
            _figureDescriptorMock
                .SetupGet(descriptor => descriptor.RequiredProps)
                .Returns(requiredProps);
            
            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);

            ValidationFailure failure = validationResult.Errors[0];
            
            Assert.All(requiredProps.Skip(takePropsCount), prop => Assert.Contains(prop, failure.ErrorMessage));
            
            _figureDescriptorMock.Verify(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string,double>>()), Times.Never);
        }
    }
}
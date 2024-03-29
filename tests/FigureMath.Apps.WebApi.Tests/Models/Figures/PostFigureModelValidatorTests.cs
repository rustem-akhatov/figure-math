using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain;
using FigureMath.Data;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Tests.Figures
{
    public class PostFigureModelValidatorTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFigureDescriptor> _figureDescriptorMock;
        
        private readonly AddFigureModelValidator _validator;

        public PostFigureModelValidatorTests()
        {
            _fixture = new Fixture();

            _figureDescriptorMock = new Mock<IFigureDescriptor>();

            var figureDescriptorProviderMock = new Mock<IFigureDescriptorProvider>();
            
            figureDescriptorProviderMock
                .Setup(provider => provider.GetDescriptorFor(It.IsAny<FigureType>()))
                .Returns(_figureDescriptorMock.Object);

            _validator = new AddFigureModelValidator(figureDescriptorProviderMock.Object);
        }
        
        [Fact]
        public void Validate_ShouldReturnValid_WhenAllPropsSpecifiedCorrectly()
        {
            // Arrange
            var model = _fixture.Create<AddFigureModel>();

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
        public void Validate_ShouldReturnInvalid_WhenSomethingWrong()
        {
            // Arrange
            var model = _fixture.Create<AddFigureModel>();

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
        public void Validate_ShouldReturnOnlyContextPropertyName_WhenFailurePropertyNameIsNull()
        {
            // Arrange
            var model = _fixture.Create<AddFigureModel>();

            _figureDescriptorMock
                .Setup(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string, double>>()))
                .Returns(new ValidationResult(new[] { new ValidationFailure(null, _fixture.Create<string>()) }));

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            
            Assert.All(validationResult.Errors, error => Assert.Equal(nameof(AddFigureModel.FigureProps), error.PropertyName));
        }
        
        [Fact]
        public void Validate_ShouldReturnCombinedPropertyName_WhenFailurePropertyNameIsNotNull()
        {
            // Arrange
            var model = _fixture.Create<AddFigureModel>();

            ValidationFailure[] failures = _fixture.CreateMany<ValidationFailure>().ToArray();
            
            _figureDescriptorMock
                .Setup(descriptor => descriptor.ValidateProps(It.IsAny<IDictionary<string, double>>()))
                .Returns(new ValidationResult(failures));

            // Act
            ValidationResult validationResult = _validator.Validate(model);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            
            Assert.All(validationResult.Errors, 
                error => Assert.Contains(failures, failure => error.PropertyName == $"{nameof(AddFigureModel.FigureProps)}.{failure.PropertyName}"));
        }
    }
}
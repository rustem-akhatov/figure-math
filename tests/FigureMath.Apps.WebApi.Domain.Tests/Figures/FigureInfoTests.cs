using System;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Data.Enums;
using FigureMath.Data.Testing.AutoFixture.Extensions;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures
{
    public class FigureInfoTests
    {
        private readonly Fixture _fixture;

        public FigureInfoTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeInstance_WhenFigureTypeMatch()
        {
            // Arrange
            var figure = _fixture.CreateFigure();

            // Act
            var figureInfo = new Mock<FigureInfo>(figure.FigureType, figure).Object;

            // Assert
            Assert.NotNull(figureInfo);
        }
        
        [Fact]
        public void Constructor_ShouldThrowException_WhenFigureTypeMismatch()
        {
            // Arrange
            var figure = _fixture.CreateFigure(FigureType.Circle);

            // Act
            Exception exception = Record.Exception(() => new Mock<FigureInfo>(FigureType.Rectangle, figure).Object);

            // Assert
            Assert.NotNull(exception);
            
            Assert.NotNull(exception.InnerException);
            
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }
    }
}
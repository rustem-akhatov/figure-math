using System;
using AutoFixture;
using FigureMath.Data;
using FigureMath.Data.Testing;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class FigureInfoFactoryTests
    {
        private const FigureType _testFigureType = FigureType.Circle;

        private readonly Mock<IFigureInfoTypeProvider> _figureInfoTypeProviderMock;
        private readonly FigureInfoFactory _figureInfoFactory;

        private readonly Figure _testFigure;
        
        public FigureInfoFactoryTests()
        {
            var fixture = new Fixture();

            _figureInfoTypeProviderMock = new Mock<IFigureInfoTypeProvider>();
            
            _figureInfoFactory = new FigureInfoFactory(_figureInfoTypeProviderMock.Object);
            
            _testFigure = fixture.CreateFigure(_testFigureType);
        }
        
        [Fact]
        public void Create_ShouldCreateSpecificFigureInfo_WhenTypeHasNecessaryConstructor()
        {
            // Arrange
            _figureInfoTypeProviderMock
                .Setup(provider => provider.GetTypeFor(It.IsAny<FigureType>()))
                .Returns(typeof(WithNecessaryConstructorFigureInfo));

            // Act
            FigureInfo figureInfo = _figureInfoFactory.Create(_testFigure);

            // Assert
            Assert.NotNull(figureInfo);
        }
        
        [Fact]
        public void Create_ShouldThrowException_WhenTypeDoesNotHaveNecessaryConstructor()
        {
            // Arrange
            _figureInfoTypeProviderMock
                .Setup(provider => provider.GetTypeFor(It.IsAny<FigureType>()))
                .Returns(typeof(WithoutNecessaryConstructorFigureInfo));

            // Act
            Exception exception = Record.Exception(() => _figureInfoFactory.Create(_testFigure));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
        
        private class WithNecessaryConstructorFigureInfo : FigureInfo
        {
            public WithNecessaryConstructorFigureInfo(Figure figure)
                : base(FigureType.Circle, figure)
            {
                // For example
                Area = int.MinValue;
            }

            public override double Area { get; }
        }
        
        private class WithoutNecessaryConstructorFigureInfo : FigureInfo
        {
            public WithoutNecessaryConstructorFigureInfo(Figure figure, double area)
                : base(FigureType.Circle, figure)
            {
                // For example
                Area = area;
            }

            public override double Area { get; }
        }
    }
}
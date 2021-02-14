using System;
using System.Reflection;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Data.Enums;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests.Figures
{
    public class FigureInfoTypeProviderTests
    {
        private readonly Fixture _fixture;
        
        private readonly FigureInfoTypeProvider _figureInfoTypeProvider;
        
        public FigureInfoTypeProviderTests()
        {
            _fixture = new Fixture();
            
            _figureInfoTypeProvider = new FigureInfoTypeProvider();
        }
        
        [Fact]
        public void GetTypeFor_ShouldReturnType_WhenKnownFigureType()
        {
            // Arrange
            var figureType = _fixture.Create<FigureType>();

            // Act
            Type type = _figureInfoTypeProvider.GetTypeFor(figureType);
            
            var attribute = type?.GetCustomAttribute<FigureImplementationAttribute>();

            // Assert
            Assert.NotNull(type);
            Assert.NotNull(attribute);
            Assert.Equal(figureType, attribute.FigureType);
        }

        [Fact]
        public void GetTypeFor_ShouldThrowException_WhenUnknownFigureType()
        {
            // Arrange
            const FigureType figureType = (FigureType)10000000;

            // Act
            Exception exception = Record.Exception(() => _figureInfoTypeProvider.GetTypeFor(figureType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
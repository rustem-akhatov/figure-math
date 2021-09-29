using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FigureMath.Common.Data;
using FigureMath.Data;
using FigureMath.Data.Testing;
using MediatR;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class GetFigureInfoHandlerTests
    {
        private readonly Fixture _fixture;

        private readonly FigureMathDbContext _dbContext;

        private readonly Mock<IFigureInfoFactory> _figureInfoFactoryMock;
        
        private readonly IRequestHandler<GetFigureInfoRequest, FigureInfo> _handler;

        public GetFigureInfoHandlerTests()
        {
            _fixture = new Fixture();

            _dbContext = _fixture.CreateFigureMathDbContext();

            _figureInfoFactoryMock = new Mock<IFigureInfoFactory>();
            
            _figureInfoFactoryMock
                .Setup(factory => factory.Create(It.IsAny<Figure>()))
                .Returns<Figure>(figureArg => new Mock<FigureInfo>(figureArg.FigureType, figureArg).Object);

            _handler = new GetFigureInfoHandler(_dbContext, _figureInfoFactoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFigureInfo_WhenFigureExists()
        {
            // Arrange
            Figure figure = _fixture.CreateFigure();
            
            _dbContext.Figures.Add(figure);
            
            await _dbContext.SaveChangesAsync();

            var request = _fixture.Build<GetFigureInfoRequest>()
                .FromFactory(() => new GetFigureInfoRequest(figure.Id))
                .Create();
            
            // Act
            FigureInfo figureInfo = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(figureInfo);
            
            _figureInfoFactoryMock.Verify(factory => factory.Create(It.IsAny<Figure>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenFigureNotExists()
        {
            // Arrange
            var request = _fixture.Create<GetFigureInfoRequest>();
            
            // Act
            Exception exception = await Record.ExceptionAsync(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<EntityNotFoundException>(exception);
            
            _figureInfoFactoryMock.Verify(factory => factory.Create(It.IsAny<Figure>()), Times.Never);
        }
    }
}
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FigureMath.Apps.WebApi.Domain;
using FigureMath.Data;
using FigureMath.Data.Testing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Tests
{
    public class FiguresControllerTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMediator> _mediatorMock;

        private readonly FiguresController _controller;
        
        public FiguresControllerTests()
        {
            _fixture = new Fixture();

            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            
            _controller = new FiguresController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetFigure_ShouldReturnOkOfFigureInfo_WhenCalled()
        {
            // Arrange
            var figureInfoMock = new Mock<FigureInfo>(_fixture.Create<FigureType>());

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetFigureInfoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(figureInfoMock.Object);

            // Act
            IActionResult actionResult = await _controller.GetFigure(_fixture.Create<long>());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);

            var objectResult = (OkObjectResult)actionResult;
            Assert.Equal(figureInfoMock.Object, objectResult.Value);
            
            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetFigureInfoRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFigureArea_ShouldReturnContentOfArea_WhenCalled()
        {
            // Arrange
            var area = _fixture.Create<double>();
            
            var figureInfoMock = new Mock<FigureInfo>(_fixture.Create<FigureType>());
            figureInfoMock
                .SetupGet(figure => figure.Area)
                .Returns(area);
            
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetFigureInfoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(figureInfoMock.Object);

            // Act
            IActionResult actionResult = await _controller.GetFigureArea(_fixture.Create<long>());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ContentResult>(actionResult);

            var contentResult = (ContentResult)actionResult;
            Assert.Equal(area.ToString(CultureInfo.InvariantCulture), contentResult.Content);
            
            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetFigureInfoRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PostFigure_ShouldReturnCreatedAtAction_WhenCalled()
        {
            // Arrange
            Figure figure = _fixture.CreateFigure();
            
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<AddFigureRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(figure);

            // Act
            IActionResult actionResult = await _controller.AddFigure(_fixture.Create<AddFigureModel>());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<CreatedAtActionResult>(actionResult);

            var createdResult = (CreatedAtActionResult)actionResult;
            Assert.Equal(nameof(FiguresController.GetFigure), createdResult.ActionName);
            Assert.Null(createdResult.ControllerName);
            Assert.Equal(figure, createdResult.Value);

            Assert.Single(createdResult.RouteValues);
            Assert.Contains(createdResult.RouteValues, pair => pair.Key == "id");
            Assert.Equal(figure.Id, createdResult.RouteValues["id"]);
            
            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<AddFigureRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
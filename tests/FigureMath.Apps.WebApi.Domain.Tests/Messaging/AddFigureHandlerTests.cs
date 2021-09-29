using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FigureMath.Data;
using FigureMath.Data.Testing;
using MediatR;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class AddFigureHandlerTests
    {
        private readonly Fixture _fixture;
        
        private readonly FigureMathDbContext _dbContext;
        
        private readonly IRequestHandler<AddFigureRequest, Figure> _handler;

        public AddFigureHandlerTests()
        {
            _fixture = new Fixture();

            _dbContext = _fixture.CreateFigureMathDbContext();

            _handler = new AddFigureHandler(_dbContext);
        }

        [Fact]
        public async Task Handle_ShouldAddFigure_WhenCalled()
        {
            // Arrange
            var request = _fixture.Create<AddFigureRequest>();

            // Act
            Figure figure = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(figure);
            Assert.Single(_dbContext.Figures);
        }
    }
}
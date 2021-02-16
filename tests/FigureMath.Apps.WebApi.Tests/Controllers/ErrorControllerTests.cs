using System;
using AutoFixture;
using FigureMath.Apps.Hosting;
using FigureMath.Apps.WebApi.Controllers;
using FigureMath.Common.Data.Exceptions;
using FigureMath.Testing.Moq.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Tests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly Fixture _fixture;
        
        private readonly Mock<IHostEnvironment> _hostEnvironmentMock;
        private readonly Mock<ILogger<ErrorController>> _loggerMock;
        
        private readonly Mock<IExceptionHandlerPathFeature> _exceptionHandlerPathFeatureMock;
        
        private readonly ErrorController _controller;

        public ErrorControllerTests()
        {
            _fixture = new Fixture();
            
            _hostEnvironmentMock = new Mock<IHostEnvironment>();
            _loggerMock = new Mock<ILogger<ErrorController>>();

            _exceptionHandlerPathFeatureMock = new Mock<IExceptionHandlerPathFeature>();

            var featureCollection = new FeatureCollection();
            featureCollection.Set(_exceptionHandlerPathFeatureMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            
            httpContextMock
                .SetupGet(context => context.Features)
                .Returns(featureCollection);

            httpContextMock
                .SetupGet(context => context.Request)
                .Returns(Mock.Of<HttpRequest>());
            
            _controller = new ErrorController(_hostEnvironmentMock.Object, _loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
        }

        [Fact]
        public void Unknown_ShouldReturnNotFoundWithoutProblem_WhenNoError()
        {
            // Arrange
            
            // Act
            IActionResult actionResult = _controller.Unknown();

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundResult>(actionResult);
            
            _loggerMock.VerifyLogged(LogLevel.Error, Times.Never);
        }

        [Fact]
        public void Unknown_ShouldReturnNotFoundWithProblem_WhenEntityNotFoundExceptionOccured()
        {
            // Arrange
            _exceptionHandlerPathFeatureMock
                .SetupGet(feature => feature.Error)
                .Returns(new EntityNotFoundException());

            // Act
            IActionResult actionResult = _controller.Unknown();

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            
            _loggerMock.VerifyLogged(LogLevel.Error, Times.Never);
        }
        
        [Fact]
        public void Unknown_ShouldReturnInternalServerError_WhenUnknownExceptionOccured()
        {
            // Arrange
            _exceptionHandlerPathFeatureMock
                .SetupGet(feature => feature.Error)
                .Returns(_fixture.Create<Exception>());

            // Act
            IActionResult actionResult = _controller.Unknown();

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            
            _loggerMock.VerifyLogged(LogLevel.Error, Times.Once);
        }

        [Theory]
        [InlineData("Development")]
        [InlineData(HostEnvironments.DockerDesktop)]
        public void Unknown_ShouldReturnDetail_WhenEnvironmentIs(string environment)
        {
            // Arrange
            Exception exception = CreateExceptionWithStackTrace();
            
            _exceptionHandlerPathFeatureMock
                .SetupGet(feature => feature.Error)
                .Returns(exception);

            _hostEnvironmentMock
                .SetupGet(host => host.EnvironmentName)
                .Returns(environment);

            // Act
            IActionResult actionResult = _controller.Unknown();

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = (ObjectResult)actionResult;
            Assert.NotNull(objectResult.Value);

            var problemDetails = (ProblemDetails)objectResult.Value;
            
            Assert.Equal(exception.Message, problemDetails.Title);
            Assert.Equal(exception.StackTrace, problemDetails.Detail);
        }

        [Theory]
        [InlineData("Production")]
        public void Unknown_ShouldNotReturnDetail_WhenEnvironmentIs(string environment)
        {
            // Arrange
            Exception exception = CreateExceptionWithStackTrace();

            _exceptionHandlerPathFeatureMock
                .SetupGet(feature => feature.Error)
                .Returns(exception);

            _hostEnvironmentMock
                .SetupGet(host => host.EnvironmentName)
                .Returns(environment);
            
            // Act
            IActionResult actionResult = _controller.Unknown();

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = (ObjectResult)actionResult;
            Assert.NotNull(objectResult.Value);

            var problemDetails = (ProblemDetails)objectResult.Value;
            
            Assert.Null(problemDetails.Title);
            Assert.Null(problemDetails.Detail);
        }

        private Exception CreateExceptionWithStackTrace()
        {
            try
            {
                throw _fixture.Create<Exception>();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
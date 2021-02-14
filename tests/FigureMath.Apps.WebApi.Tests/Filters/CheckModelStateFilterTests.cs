using System.Collections.Generic;
using AutoFixture;
using FigureMath.Apps.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Tests.Filters
{
    public class CheckModelStateFilterTests
    {
        private readonly Fixture _fixture;
        
        private readonly ModelStateDictionary _modelState;
        
        private readonly ActionExecutingContext _actionExecutingContext;
        
        private readonly CheckModelStateFilter _filter;

        public CheckModelStateFilterTests()
        {
            _fixture = new Fixture();
            
            _modelState = new ModelStateDictionary();
            
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                _modelState
            );

            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
            
            _filter = new CheckModelStateFilter();
        }
        
        [Fact]
        public void OnActionExecuting_ShouldDoNothing_WhenModelStateIsValid()
        {
            // Arrange
            
            // Act
            _filter.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_ShouldAssignBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _modelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());

            // Act
            _filter.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.NotNull(_actionExecutingContext.Result);
            Assert.IsType<BadRequestObjectResult>(_actionExecutingContext.Result);
        }
    }
}
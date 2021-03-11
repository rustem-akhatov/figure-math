using System.Globalization;
using System.Threading.Tasks;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Messaging;
using FigureMath.Apps.WebApi.Models.Figures;
using FigureMath.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FigureMath.Apps.WebApi.Controllers
{
    /// <summary>
    /// Controller to work with figures.
    /// </summary>
    [Route("figures")]
    public class FiguresController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiguresController"/> class.
        /// </summary>
        /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
        public FiguresController(IMediator mediator)
        {
            _mediator = EnsureArg.IsNotNull(mediator, nameof(mediator));
        }

        /// <summary>
        /// Returns an entire information about the figure includes its area.
        /// </summary>
        /// <param name="id">Identifier of the figure.</param>
        /// <returns>Entire information about the figure.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFigure(long id)
        {
            var request = new GetFigureInfoRequest(id);

            FigureInfo figureInfo = await _mediator.Send(request);
            
            return Ok(figureInfo);
        }

        /// <summary>
        /// Calculates area of the specified figure and returns it.
        /// </summary>
        /// <param name="id">Identifier of the figure.</param>
        /// <returns>Calculated area of the figure.</returns>
        [HttpGet("{id}/area")]
        public async Task<IActionResult> GetFigureArea(long id)
        {
            var request = new GetFigureInfoRequest(id);
            
            FigureInfo figureInfo = await _mediator.Send(request);
            
            return Content(figureInfo.Area.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds figure to persistent store.
        /// </summary>
        /// <param name="model">The model of the input parameters.</param>
        /// <returns>Identifier of the created figure and figure itself.</returns>
        [HttpPost]
        public async Task<IActionResult> PostFigure([FromBody] PostFigureModel model)
        {
            var request = new AddFigureRequest(model.FigureType, model.FigureProps);

            Figure figure = await _mediator.Send(request);
            
            return CreatedAtAction(nameof(GetFigure), new { id = figure.Id }, figure);
        }
    }
}
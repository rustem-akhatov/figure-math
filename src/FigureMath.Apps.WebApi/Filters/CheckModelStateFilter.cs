using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FigureMath.Apps.WebApi
{
    /// <summary>
    /// Ensures model is valid.
    /// </summary>
    public class CheckModelStateFilter : IActionFilter
    {
        /// <summary>
        /// Checks <see cref="ModelStateDictionary.IsValid"/>.
        /// If ModelStat is invalid then assigns <see cref="BadRequestObjectResult"/> to result of the current context.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
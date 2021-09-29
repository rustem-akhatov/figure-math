using FigureMath.Common.AspNet.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;

namespace FigureMath.Common.AspNet
{
    public abstract class CommonControllerBase : ControllerBase
    {
        protected ObjectResult Problem(IProblemInfo problem)
        {
            return Problem(
                type: problem.ProblemType,
                title: problem.Title,
                statusCode: problem.StatusCode);
        }
    }
}
using Microsoft.AspNetCore.Http;

namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public class DefaultProblemInfo : IProblemInfo
    {
        public string ProblemType => "InternalServerError";

        public int StatusCode => StatusCodes.Status500InternalServerError;

        public string Title => null;
    }
}
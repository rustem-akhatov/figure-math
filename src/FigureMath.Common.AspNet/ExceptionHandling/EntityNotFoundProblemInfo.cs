using EnsureThat;
using FigureMath.Common.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public class EntityNotFoundProblemInfo : IProblemInfo
    {
        private readonly EntityNotFoundException _error;
        private readonly string _requestMethod;

        [UsedImplicitly]
        public EntityNotFoundProblemInfo(EntityNotFoundException error)
            : this(error, HttpMethods.Get)
        { }
        
        public EntityNotFoundProblemInfo(EntityNotFoundException error, string requestMethod)
        {
            _error = EnsureArg.IsNotNull(error, nameof(error));
            _requestMethod = EnsureArg.IsNotNullOrEmpty(requestMethod, nameof(requestMethod));
        }

        public string ProblemType => $"{_error.EntityType.Name}NotFound";

        public int StatusCode => IsGet ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;

        public string Title => _error.Message;

        private bool IsGet => HttpMethods.IsGet(_requestMethod);
    }
}
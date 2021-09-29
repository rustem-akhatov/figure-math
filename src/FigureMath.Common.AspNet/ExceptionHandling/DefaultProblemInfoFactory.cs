using EnsureThat;
using FigureMath.Common.Data;

namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public class DefaultProblemInfoFactory : IProblemInfoFactory
    {
        public virtual IProblemInfo Create(ProblemContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            
            return context.Error switch
            {
                EntityNotFoundException entityNotFoundException => new EntityNotFoundProblemInfo(entityNotFoundException, context.RequestMethod),
                _ => new DefaultProblemInfo()
            };
        }
    }
}
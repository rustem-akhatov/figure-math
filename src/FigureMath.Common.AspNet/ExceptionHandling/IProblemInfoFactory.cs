namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public interface IProblemInfoFactory
    {
        IProblemInfo Create(ProblemContext context);
    }
}
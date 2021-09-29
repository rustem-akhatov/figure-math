namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public interface IProblemInfo
    {
        string ProblemType { get; }
        
        int StatusCode { get; }
        
        string Title { get; }
    }
}
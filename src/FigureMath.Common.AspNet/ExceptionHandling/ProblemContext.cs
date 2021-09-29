using System;

namespace FigureMath.Common.AspNet.ExceptionHandling
{
    public class ProblemContext : ProblemContext<Exception>
    { }

    public class ProblemContext<TException>
        where TException : Exception
    {
        public string RequestMethod { get; init; }
        
        public TException Error { get; init; }
    }
}
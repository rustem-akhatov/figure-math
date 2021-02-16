// ReSharper disable UnusedMember.Global

using System;
using System.Linq.Expressions;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Moq;

namespace FigureMath.Testing.Moq.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Mock{ILogger}"/> of <see cref="ILogger"/>.
    /// </summary>
    public static class MockExtensions
    {
        /// <summary>
        /// Verifies that a specific log method was performed on the mock.
        /// </summary>
        /// <param name="loggerMock">An instance of <see cref="Mock{ILogger}"/>.</param>
        /// <param name="level">Log level that have to be logged.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <typeparam name="T">The type who's name is used for the logger category name.</typeparam>
        public static void VerifyLogged<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Func<Times> times)
        {
            EnsureArg.IsNotNull(times, nameof(times));
            
            VerifyLogged(loggerMock, level, times());
        }
        
        /// <summary>
        /// Verifies that a specific log method was performed on the mock.
        /// </summary>
        /// <param name="loggerMock">An instance of <see cref="Mock{ILogger}"/>.</param>
        /// <param name="level">Log level that have to be logged.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        public static void VerifyLogged(this Mock<ILogger> loggerMock, LogLevel level, Func<Times> times)
        {
            EnsureArg.IsNotNull(times, nameof(times));
            
            VerifyLogged(loggerMock, level, times());
        }

        /// <summary>
        /// Verifies that a specific log method was performed on the mock.
        /// </summary>
        /// <param name="loggerMock">An instance of <see cref="Mock{ILogger}"/>.</param>
        /// <param name="level">Log level that have to be logged.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <typeparam name="T">The type who's name is used for the logger category name.</typeparam>
        public static void VerifyLogged<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Times times)
        {
            EnsureArg.IsNotNull(loggerMock, nameof(loggerMock));
            
            loggerMock.Verify(VerifyLogged<T>(level), times);
        }
        
        /// <summary>
        /// Verifies that a specific log method was performed on the mock.
        /// </summary>
        /// <param name="loggerMock">An instance of <see cref="Mock{ILogger}"/>.</param>
        /// <param name="level">Log level that have to be logged.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        public static void VerifyLogged(this Mock<ILogger> loggerMock, LogLevel level, Times times)
        {
            EnsureArg.IsNotNull(loggerMock, nameof(loggerMock));
            
            loggerMock.Verify(VerifyLogged(level), times);
        }
        
        private static Expression<Action<ILogger<T>>> VerifyLogged<T>(LogLevel level)
        {
            return logger => logger.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>) It.IsAny<object>());
        }

        private static Expression<Action<ILogger>> VerifyLogged(LogLevel level)
        {
            return logger => logger.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>) It.IsAny<object>());
        }
    }
}
using System;
using System.Diagnostics;
using EnsureThat;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FigureMath.Common.AspNet.Infrastructure
{
    /// <summary>
    /// Represents decorator of the <see cref="ProblemDetailsFactory"/> class that replace traceId extension
    /// using <see cref="CorrelationManager.ActivityId"/> of the current <see cref="Trace.CorrelationManager"/>. 
    /// </summary>
    [UsedImplicitly]
    public class ActivityIdProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ProblemDetailsFactory _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityIdProblemDetailsFactory"/> class using decorated <see cref="ProblemDetailsFactory"/>.
        /// </summary>
        /// <param name="inner">Decorated instance of the <see cref="ProblemDetailsFactory"/> class.</param>
        public ActivityIdProblemDetailsFactory(ProblemDetailsFactory inner)
        {
            _inner = EnsureArg.IsNotNull(inner, nameof(inner));
        }

        /// <summary>
        /// Creates a <see cref="ProblemDetails" /> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions" />.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext" />.</param>
        /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
        /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
        /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
        /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
        /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
        /// <returns>The <see cref="ProblemDetails" /> instance.</returns>
        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            return ReplaceTraceId(_inner.CreateProblemDetails(httpContext,
                statusCode,
                title,
                type,
                detail,
                instance));
        }

        /// <summary>
        /// Creates a <see cref="ValidationProblemDetails" /> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions" />.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext" />.</param>
        /// <param name="modelStateDictionary">The <see cref="ModelStateDictionary" />.</param>
        /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
        /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
        /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
        /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
        /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
        /// <returns>The <see cref="ValidationProblemDetails" /> instance.</returns>
        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            return ReplaceTraceId(_inner.CreateValidationProblemDetails(httpContext,
                modelStateDictionary,
                statusCode,
                title,
                type,
                detail,
                instance
            ));
        }

        private static T ReplaceTraceId<T>(T problemDetails)
            where T : ProblemDetails
        {
            if (problemDetails == null)
                return null;
            
            Guid activityId = Trace.CorrelationManager.ActivityId;
            if (activityId != Guid.Empty)
            {
                problemDetails.Extensions["traceId"] = activityId;
            }

            return problemDetails;
        }
    }
}
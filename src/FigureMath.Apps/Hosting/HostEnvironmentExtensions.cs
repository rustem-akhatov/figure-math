using EnsureThat;
using Microsoft.Extensions.Hosting;

namespace FigureMath.Apps.Hosting
{
    /// <summary>
    /// Extension methods for <see cref="IHostEnvironment"/>.
    /// </summary>
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.DockerDesktop"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.DockerDesktop"/>, otherwise false.</returns>
        public static bool IsDockerDesktop(this IHostEnvironment hostEnvironment)
        {
            EnsureArg.IsNotNull(hostEnvironment, nameof(hostEnvironment));

            return hostEnvironment.IsEnvironment(HostEnvironments.DockerDesktop);
        }
    }
}
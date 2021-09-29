using System;
using System.Reflection;
using EnsureThat;

namespace FigureMath.Common
{
    /// <summary>
    /// Helper methods to work with instances of the <see cref="Assembly"/> class.
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// Returns version of the entry assembly.
        /// </summary>
        /// <returns>Version of the entry assembly.</returns>
        /// <exception cref="InvalidOperationException">Entry assembly is null.</exception>
        public static string GetVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            
            if (assembly == null)
                throw new InvalidOperationException("Entry assembly is null.");

            return GetVersion(assembly);
        }

        /// <summary>
        /// Returns version of the passed assembly.
        /// </summary>
        /// <param name="assembly">Instance of the <see cref="Assembly"/> class.</param>
        /// <returns>Version of <paramref name="assembly"/>.</returns>
        public static string GetVersion(Assembly assembly)
        {
            EnsureArg.IsNotNull(assembly, nameof(assembly));
            
            return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }
    }
}
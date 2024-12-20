using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppAngular.Service.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetResourceAsString(this Assembly assembly, string path)
        {
            var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith(path, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new InvalidOperationException($"Resource '{path}' not found in assembly '{assembly.FullName}'.");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new InvalidOperationException($"Failed to get a stream for the resource '{path}'.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}

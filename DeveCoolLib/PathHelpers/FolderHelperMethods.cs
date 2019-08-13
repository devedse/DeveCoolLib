using System;
using System.IO;
using System.Reflection;

namespace DeveCoolLib.PathHelpers
{
    public static class FolderHelperMethods
    {
        public static Lazy<string> AssemblyDirectory { get; } = new Lazy<string>(() => CreateLocationOfImageProcessorAssemblyDirectory());

        private static string CreateLocationOfImageProcessorAssemblyDirectory()
        {
            var assembly = typeof(FolderHelperMethods).GetTypeInfo().Assembly;
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            return assemblyDir;
        }
    }
}

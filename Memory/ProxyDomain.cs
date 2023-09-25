using System;
using System.Reflection;

namespace EngineLibrary.Memory
{
    internal class ProxyDomain : MarshalByRefObject
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Scripting> {ex.Message}");
                return null;
            }
        }
    }
}

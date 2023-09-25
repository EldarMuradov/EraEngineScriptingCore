using EngineLibrary.ECS;
using EngineLibrary.FileSystem;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EngineLibrary.Scripting
{
    public interface IAssemblyLoader
    {
        Assembly Load(byte[] bytes);
    }

    public class AssemblyLoader : MarshalByRefObject, IAssemblyLoader
    {
        public Assembly Load(byte[] bytes)
        {
            return AppDomain.CurrentDomain.Load(bytes);
        }
    }

    public sealed class UserScripting
    {
        private AppDomain _domain;

        private Assembly _assembly;

        public UserScripting() => CreateDomain();

        internal void ReloadDll() 
        {
            ReleaseDomain();

            CreateDomain();

            LoadDll();
        }

        internal void LoadDll() 
        {
            if (!Directory.Exists(Project.TempDllPath))
                Directory.CreateDirectory(Project.Path + "\\" + Project.TempDllPath);

            IFileCloner fileCloner = new FileCloner();
            fileCloner.Clone(Path.Combine(Project.Path, "Assets\\Scripts", Project.Name, Project.Name, "bin\\Debug"), Project.Path + "\\" + Project.TempDllPath);

            var bytes = GenerateAssemblyAndGetRawBytes();

            var assemblyLoader = new AssemblyLoader();

            _assembly = assemblyLoader.Load(bytes);

            var types = _assembly.GetTypes();
            string[] names = new string[types.Length];
            int i = 0, length = types.Length;
            for (; i < length; i++)
            {
                names[i] = types[i].Name;
                SendTypes_Internal(names[i]);
            }
        }

        internal void SyncScripting() 
        {
            if (_assembly != null)
                LoadAllData(_assembly);
        }

        internal Component GetComponent(string name) 
        {
            var type = _assembly.GetType(Project.Name + "." + name);
            if (type != null)
            {
                var component = (Component)Activator.CreateInstance(type);

                return component;
            }

            return null;
        }

        private void LoadAllData(Assembly assembly) 
        {

        }

        private static byte[] GenerateAssemblyAndGetRawBytes()
        {
            return File.ReadAllBytes(Project.Path + "\\" + Project.TempDllPath + $"\\{Project.Name}.dll");
        }

        internal void ReleaseDomain() => AppDomain.Unload(_domain);

        private void CreateDomain()
        {
            _domain = AppDomain.CreateDomain("UserAppDomain" + Guid.NewGuid(), null, new AppDomainSetup
            {
                ShadowCopyFiles = "true",
                LoaderOptimization = LoaderOptimization.MultiDomainHost
            });
            _domain.UnhandledException += (o, e) => { Debug.LogError("Scripting> " + ((Exception)e.ExceptionObject).Message); };
        }

        internal static void ReloadScripting_External() 
        {
            UserScripting scripting = Game.Scripting;
            AppDomain.Unload(scripting._domain);
            scripting.CreateDomain();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            scripting.LoadDll();
            Debug.Log("Reloading");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SendTypes_Internal(string types);
    }
}
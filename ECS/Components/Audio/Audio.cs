using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EngineLibrary.ECS.Components
{
    [Serializable]
    public struct Audio
    {
        public string Path;

        public Audio(string path)
        {
            Path = path;
            Task.Run(() => LoadAudio_Internal(path));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern static void LoadAudio_Internal(string path);
    }
}

using EngineLibrary.Memory;
using System.Runtime.CompilerServices;

namespace EngineLibrary.SceneManagement
{
    public static class SceneManager
    {
        public static void LoadScene(string name) 
        {
            LoadScene_Internal(name);
            Game.Entities = Serializer.DeserializeSceneAsync(name).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void LoadScene_Internal(string name);
    }
}

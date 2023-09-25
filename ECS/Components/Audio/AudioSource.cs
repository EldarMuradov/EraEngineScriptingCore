using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS.Components
{
    public class AudioSource : Component
    {
        private uint _id = 0;

        public override void Start()
        {
            _id = GameEntity.Id;
        }

        public override void Update()
        {

        }

        public void PlayOneShoot(Audio audio) 
        {
            PlayOneShoot_Internal(_id, audio.Path);
        }

        public void Stop() 
        {
            Stop_Internal(_id);
        }

        public void Pause() 
        {
            Pause_Internal(_id);
        }

        public void Resume() 
        {
            Resume_Internal(_id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void PlayOneShoot_Internal(uint id, string path);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Stop_Internal(uint id);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Pause_Internal(uint id);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Resume_Internal(uint id);
    }
}

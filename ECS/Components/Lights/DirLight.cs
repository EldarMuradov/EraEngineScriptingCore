using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS.Components
{
    public sealed class DirLight : Light
    {
        public override void Start()
        {

        }

        public override void Update()
        {

        }

        internal override void Init(params object[] args)
        {
            this.LightData = (ILightData)args[0];
            DirLightData data = (DirLightData)LightData;
            Init_Internal(GameEntity.Id, data.Color.R, data.Color.G, data.Color.B, data.Ambient.X, data.Ambient.Y, data.Ambient.Z,
                data.Direction.X, data.Direction.Y, data.Direction.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Init_Internal(uint id, float r, float g, float b, float r2, float g2, float b2, float x, float y, float z);
    }
}

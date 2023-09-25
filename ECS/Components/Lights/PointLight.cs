using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS.Components
{
    public sealed class PointLight : Light
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

            PointLightData data = (PointLightData)LightData;
            Init_Internal(GameEntity.Id, data.Color.R, data.Color.G, data.Color.B, data.Attenuation.X, data.Attenuation.Y, data.Attenuation.Z,
                data.Position.X, data.Position.Y, data.Position.Z, data.Range);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Init_Internal(uint id, float r, float g, float b, float r2, float g2, float b2, float x, float y, float z, float range);
    }
}

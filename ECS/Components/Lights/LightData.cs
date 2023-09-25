using EngineLibrary.Math;

namespace EngineLibrary.ECS.Components
{
    public interface ILightData 
    {
        Color Color { get; }
    }

    public struct PointLightData : ILightData
    {
        public Color Color { get;}

        public readonly float Range;

        public Vector3D Position { get; }
        public Vector3D Attenuation { get; }

        public PointLightData(Color color, Vector3D pos, Vector3D att, float range)
        {
            Color = color;
            Position = pos;
            Attenuation = att;
            Range = range;
        }
    }

    public struct SpotLightData : ILightData
    {
        public Color Color { get; }

        public Vector3D Direction { get; }
        public Vector3D Position { get; }
        public Vector3D Attenuation { get; }

        public readonly float Range;
        public readonly float InnerConeAngle;
        public readonly float OuterConeAngle;

        public SpotLightData(Color color, Vector3D dir, Vector3D pos, Vector3D att, float range, float inner, float outter)
        {
            Color = color;
            Direction = dir;
            Position = pos;
            Attenuation = att;
            Range = range;
            InnerConeAngle = inner;
            OuterConeAngle = outter;
        }
    }

    public struct DirLightData : ILightData
    {
        public Color Color { get; }
        public Vector3D Direction { get; }
        public Vector3D Ambient { get; }

        public DirLightData(Color color, Vector3D dir, Vector3D amb)
        {
            Color = color;
            Direction = dir;
            Ambient = amb;
        }
    }
}

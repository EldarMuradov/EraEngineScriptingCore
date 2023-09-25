using EngineLibrary.Math;
using System.Runtime.CompilerServices;

namespace EngineLibrary
{
    public class Physics
    {
        internal static bool Raycast(uint id, Vector3D start, Vector3D dest) 
        {
            return Raycast_Internal(id, start.X, start.Y, start.Z, dest.X, dest.Y, dest.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static bool Raycast_Internal(uint id, float x1, float y1, float z1, float x2, float y2, float z2);
    }
}

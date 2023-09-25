using EngineLibrary.ECS;
using EngineLibrary.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary
{
    public sealed class TransformHandler
    {
        public static void HandlePosition(uint[] ids, float[] xs, float[] ys, float[] zs) 
        {
            int i = 0, count = ids.Length;
            for (; i < count; i++)
            {
                Game.Entities[ids[i]]?.GetComponent<Transform>()?.SetPosition(new Math.Vector3D(xs[i], ys[i], zs[i]), false);
            }
        }

        public static void HandleRotation(uint[] ids, float[] xs, float[] ys, float[] zs, float[] ws)
        {
            int i = 0, count = ids.Length;
            for (; i < count; i++)
            {
                Game.Entities[ids[i]]?.GetComponent<Transform>()?.SetRotation(new Math.Quaternion(xs[i], ys[i], zs[i], ws[i]), false);
            }
        }

        public static void HandleScale(uint[] ids, float[] xs, float[] ys, float[] zs)
        {
            int i = 0, count = ids.Length;
            for (; i < count; i++)
            {
                Game.Entities[ids[i]]?.GetComponent<Transform>()?.SetScale(new Math.Vector3D(xs[i], ys[i], zs[i]), false);
            }
        }
    }
}

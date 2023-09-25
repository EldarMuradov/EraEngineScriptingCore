using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary.ECS.Components.Colliders
{
    public sealed class BoxCollider : Collider
    {
        internal override void Init(params object[] args)
        {
            V1 = (float)args[0];
            V2 = (float)args[1];
            V3 = (float)args[2];
        }
    }
}

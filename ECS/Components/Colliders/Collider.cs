using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary.ECS.Components
{
    public enum ColliderType 
    {
        Box = 0,
        Terrain = 1,
        Sphere = 2,
        Capsule = 3,
        TriangleMesh = 4,
        ConvexMesh = 5
    }

    public abstract class Collider : Component, ICollidable
    {
        public float V1 = 0, V2 = 0, V3 = 0;

        public void OnCollisionEnter(Entity collision)
        {

        }

        public override void Start()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}

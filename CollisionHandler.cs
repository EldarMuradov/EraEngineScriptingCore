using EngineLibrary.ECS;
using EngineLibrary.ECS.Components;

namespace EngineLibrary
{
    public sealed class CollisionHandler
    {
        public static void HandleCollision(uint[] ids1, uint[] ids2)
        {
            int i = 0, count = ids1.Length;
            for (; i < count; i++)
            {
                foreach (var comp in Game.Entities[ids1[i]].Components)
                {
                    if (comp.Value is ICollidable collidable)
                        collidable.OnCollisionEnter(Game.Entities[ids2[i]]);
                }

                foreach (var comp in Game.Entities[ids2[i]].Components)
                {
                    if (comp.Value is ICollidable collidable)
                        collidable.OnCollisionEnter(Game.Entities[ids2[i]]);
                }
            }
        }
    }
}

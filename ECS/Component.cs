using EngineLibrary.Math;

namespace EngineLibrary.ECS
{
    public abstract class Component
    {
        public Entity GameEntity;

        public abstract void Start();

        public abstract void Update();

        internal virtual void Init(params object[] args) { }

        public static Entity Find(string name) 
        {
            foreach (var obj in Game.Entities) 
            {
                if (obj.Value.Name == name)
                    return obj.Value;
            }

            return null;
        }
        
        public Entity Instantiate(Entity original, Entity parent = null) => 
            GameEntity.Instantiate(original, parent);

        public Entity Instantiate(Entity original, Vector3D pos, Quaternion rot, Entity parent = null) => 
            GameEntity.Instantiate(original, pos, rot, parent);
    }
}
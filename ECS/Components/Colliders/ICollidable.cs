namespace EngineLibrary.ECS.Components
{
    public interface ICollidable
    {
        void OnCollisionEnter(Entity collision);
    }
}
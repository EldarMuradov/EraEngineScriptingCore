namespace EngineLibrary
{
    public interface IGame
    {
        bool start(string path);
        bool update(float dt);
    }
}

using EngineLibrary.ECS;
using EngineLibrary.Scripting;
using System.Collections.Generic;

namespace EngineLibrary
{
    public sealed class Game : IGame
    {
        public static Dictionary<uint, Entity> Entities = new Dictionary<uint, Entity>(200);
        public static Dictionary<uint, Entity> Additional = new Dictionary<uint, Entity>(10);

        public static float DeltaTime = 0.0f;

        internal delegate void UpdateDelegates();
        internal UpdateDelegates UpdateDelegate;

        internal static UserScripting Scripting;

        public bool start(string path)
        {
            string[] expath = path.Split('\\');
            string name = expath[expath.Length - 1];

            Project.Path = path;
            Project.Name = name;

            Scripting = new UserScripting();

            Scripting.LoadDll();

            SceneManagement.SceneManager.LoadScene("DefaultScene");

            Debug.Log("Scripting> Started Successfuly");

            UpdateDelegate += Start;

            Scripting.SyncScripting();

            if (Additional.Count > 0)
            {
                foreach (var obj in Additional)
                    Entities.Add(obj.Key, obj.Value);
                Additional.Clear();
            }

            return true;
        }

        internal void Start() 
        {
            foreach (var entity in Entities)
                entity.Value.Start();

            UpdateDelegate -= Start;
        }

        public bool update(float dt)
        {
            UpdateDelegate?.Invoke();

            foreach (var entity in Entities)
                entity.Value.Update();

            if (Additional.Count > 0)
            {
                foreach (var obj in Additional)
                    Entities.Add(obj.Key, obj.Value);
                Additional.Clear();
            }

            DeltaTime = dt;

            return true;
        }
    }
}

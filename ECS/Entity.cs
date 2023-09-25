using EngineLibrary.ECS.Components;
using EngineLibrary.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS
{
    [Serializable]
    public sealed class Entity
    {
        public string Name = "Entity";
        public string Tag = "Default";

        public Entity Parent;

        public List<Entity> Childs = new List<Entity>();

        public Dictionary<string, Component> Components = new Dictionary<string, Component>();

        public readonly uint Id;

        public bool ActiveSelf
        {
            get
            {
                return _activeSelf;
            }
            set
            {
                _activeSelf = value;
                SetActive_Internal(Id, _activeSelf);
            }
        }

        private bool _activeSelf = true;

        public Entity(uint id)
        {
            Id = id;
        }

        public void Start()
        {
            foreach (var comp in Components)
            {
                comp.Value.Start();
            }
        }

        public void Update() 
        {
            foreach (var comp in Components)
                comp.Value.Update();
        }

        public T GetComponent<T>() where T : Component, new() 
        {
            if (Components.TryGetValue(typeof(T).Name, out var comp))
                return (T)comp;
            return default;
        }

        public T CreateComponent<T>(params object[] args) where T : Component, new()
        {
            var comp = GetComponent<T>();

            if (comp != null)
                return comp;

            comp = new T();
            comp.Init(args);

            if (!typeof(T).IsSubclassOf(typeof(Script)))
                CreateComponent_Internal(Id, typeof(T).Name);
            else
                CreateScriptComponent_Internal(Id, typeof(T).Name);

            Components.Add(typeof(T).Name, comp);
            comp.GameEntity = this;

            comp.Start();

            return comp;
        }

        internal void AddComponentFromInstance(Component comp, string name) 
        {
            if (comp != null)
            {
                Components.Add(name, comp);
                comp.GameEntity = this;
                comp.Start();
                CreateScriptComponent_Internal(Id, name);
            }
        }

        public T CreateComponentInternal<T>() where T : Component, new()
        {
            var comp = GetComponent<T>();

            if (comp != null)
                return comp;

            comp = new T();

            Components.Add(typeof(T).Name, comp);
            comp.GameEntity = this;

            return comp;
        }

        public Component CopyComponent(Component component)
        {
            var type = component.GetType();

            foreach (var c in Components)
            {
                if (c.Value.GetType() == type)
                    return c.Value;
            }

            var comp = component;

            Components.Add(comp.GetType().Name, comp);
            comp.GameEntity = this;

            return comp;
        }

        public void AddChild(Entity entity) 
        {
            Childs.Add(entity);
        }

        public void RemoveChild(Entity entity) 
        {
            Childs.Remove(entity);
        }

        public void Release() => Release_Internal(Id);

        public Entity Instantiate(Entity original, Vector3D position, Quaternion rotation, Entity parent = null)
        {
            uint newId = (uint)Game.Entities.Count + 130;

            if (parent != null)
                Instantiate_Internal(original.Id, newId, (int)parent.Id);
            else
                Instantiate_Internal(original.Id, newId, -1);

            Entity instance = new Entity(newId);

            instance.Name = original.Name + newId.ToString();
            instance.Tag = original.Tag;

            if (parent != null)
                instance.Parent = parent;

            foreach (var comp in original.Components)
                instance.CopyComponent(comp.Value);

            var transform = instance.GetComponent<Transform>();
            transform.SetPosition(position);
            transform.SetRotation(rotation);

            Game.Additional.Add(newId, instance);

            return instance;
        }

        public Entity Instantiate(Entity original, Entity parent = null)
        {
            uint newId = (uint)Game.Entities.Count + 3;

            if (parent != null)
                Instantiate_Internal(original.Id, newId, (int)parent.Id);
            else
                Instantiate_Internal(original.Id, newId, -1);

            Entity instance = new Entity(newId);

            instance.Name = original.Name + newId.ToString();
            instance.Tag = original.Tag;

            if (parent != null)
                instance.Parent = parent;

            foreach (var comp in original.Components)
                instance.CopyComponent(comp.Value);

            Game.Additional.Add(newId, instance);

            return instance;
        }

        public void RemoveComponent<T>(bool fromEngine = false) where T : Component, new()
        {
            var compname = GetComponent<T>().GetType().Name;
            Components.Remove(compname);
            if (fromEngine)
                RemoveComponent_Internal(Id, compname);
        }

        public void RemoveComponent(string name, bool fromEngine = true)
        {
            Components.Remove(name);
            if (fromEngine)
                RemoveComponent_Internal(Id, name);
        }

        internal static void RemoveComponentFromEngine(uint id, string comp) 
        {
            Debug.LogError(comp);
            Game.Entities[id].RemoveComponent(comp, false);
        }

        internal static void AddComponentFromEngine(uint id, string comp)
        {
            var component = Game.Scripting.GetComponent(comp);
            Game.Entities[id].AddComponentFromInstance(component, comp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void RemoveComponent_Internal(uint id, string name);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Release_Internal(uint id);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SetActive_Internal(uint id, bool state);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void CreateComponent_Internal(uint id, string name);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void CreateScriptComponent_Internal(uint id, string name);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Instantiate_Internal(uint id, uint newId, int parentId);
    }
}
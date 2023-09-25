using EngineLibrary.ECS;
using EngineLibrary.Math;
using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS.Components
{
    public sealed class Transform : Component
    {
        private Vector3D _eulerRotation = new Vector3D(new float[3] { 0, 0, 0 });
        private Vector3D _position = new Vector3D(new float[3] { 0, 0, 0 });
        private Vector3D _scale = new Vector3D(new float[3] { 0, 0, 0 });

        private Quaternion _rotation = new Quaternion(new float[4] { 0, 0, 0, 1 });

        public Transform() 
        {
        
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        public Quaternion GetRotation() 
        {
            return _rotation;
        }

        public Vector3D GetEulerRotation()
        {
            return _eulerRotation;
        }

        public void SetRotation(Quaternion rotation, bool from = true) 
        {
            _rotation = rotation;
            _eulerRotation = Vector3D.FromQuaternion(_rotation);
            if (from)
                SetRotation_Internal(GameEntity.Id, _rotation.X, _rotation.Y, _rotation.Z, _rotation.W);
        }

        public void SetRotation(Vector3D rotation, bool from = true)
        {
            _rotation = Quaternion.Euler(rotation);
            _eulerRotation = rotation;
            if (from)
                SetRotation_Internal(GameEntity.Id, _rotation.X, _rotation.Y, _rotation.Z, _rotation.W);
        }

        public Vector3D GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector3D position, bool from = true)
        {
            _position = position;
            if (from)
                SetPosition_Internal(GameEntity.Id, _position.X, _position.Y, _position.Z);
        }

        public Vector3D GetScale()
        {
            return _scale;
        }

        public void SetScale(Vector3D scale, bool from = true)
        {
            _scale = scale;
            if (from)
                SetScale_Internal(GameEntity.Id, _scale.X, _scale.Y, _scale.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SetPosition_Internal(uint id, float x, float y, float z);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SetRotation_Internal(uint id, float x, float y, float z, float w);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SetScale_Internal(uint id, float x, float y, float z);
    }
}

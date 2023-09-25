using EngineLibrary.Math;
using System.Runtime.CompilerServices;

namespace EngineLibrary.ECS.Components
{
    public enum RigidbodyType 
    {
        Static = 0,
        Dynamic = 1,
        Kinematic = 2
    }

    public enum ForceMode 
    {
        Force = 0,
        Impulse = 1
    }

    public sealed class Rigidbody : Component
    {
        public RigidbodyType RigidbodyType;

        public uint Mass
        {
            get 
            {
                return _mass;
            }
            set 
            {
                if (_mass != value)
                {
                    _mass = value;
                    SetMass_Internal(GameEntity.Id, value);
                }
            }
        }

        private uint _mass = 1;

        public Rigidbody()
        {
            RigidbodyType = RigidbodyType.Static;
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public void AddForce(Vector3D force, ForceMode mode) 
        {
            AddForce_Internal(GameEntity.Id, (int)mode, force.X, force.Y, force.Z);
        }

        internal override void Init(params object[] args)
        {
            this.RigidbodyType = (RigidbodyType)args[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void SetMass_Internal(uint id, uint mass);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void AddForce_Internal(uint id, int mode, float x, float y, float z);
    }
}

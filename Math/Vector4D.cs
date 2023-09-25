using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EngineLibrary.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4D : IEquatable<Vector4D>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4D(float x, float y, float z, float w)
        {
            this.X = x;
            this.Z = z;
            this.Y = y;
            this.W = w;
        }

        public Vector4D(Quaternion q)
        {
            this.X = q.X;
            this.Z = q.Z;
            this.Y = q.Y;
            this.W = q.W;
        }

        public Vector4D(ReadOnlySpan<float> values)
        {
            if (values.Length < 4)
            {
                Debug.LogError("Scripting> Wrong Parameters for Vector4D!");
            }

            this = Unsafe.ReadUnaligned<Vector4D>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
        }

        public Vector4D(Vector4D vector)
        {
            this.X = vector.X;
            this.Z = vector.Z;
            this.Y = vector.Y;
            this.W = vector.W;
        }

        public static Vector4D operator +(Vector4D rhs, Vector4D lhs)
        {
            return new Vector4D(rhs.X + lhs.X, rhs.Y + lhs.Y, rhs.Z + lhs.Z, rhs.W + lhs.W);
        }

        public static Vector4D operator -(Vector4D rhs, Vector4D lhs)
        {
            return new Vector4D(rhs.X - lhs.X, rhs.Y - lhs.Y, rhs.Z - lhs.Z, rhs.W - lhs.W);
        }

        public static Vector4D operator *(Vector4D rhs, Vector4D lhs)
        {
            return new Vector4D(rhs.X * lhs.X, rhs.Y * lhs.Y, rhs.Z * lhs.Z, rhs.W * lhs.W);
        }

        public static Vector4D operator *(Vector4D rhs, float value)
        {
            return new Vector4D(rhs.X * value, rhs.Y * value, rhs.Z * value, rhs.W * value);
        }

        public static Vector4D operator /(Vector4D rhs, Vector4D lhs)
        {
            return new Vector4D(rhs.X / lhs.X, rhs.Y / lhs.Y, rhs.Z / lhs.Z, rhs.W / lhs.W);
        }

        public static Vector4D operator /(Vector4D rhs, float value)
        {
            return new Vector4D(rhs.X / value, rhs.Y / value, rhs.Z / value, rhs.W / value);
        }

        public static bool operator !=(Vector4D lhs, Vector4D rhs)
        {
            return (lhs.X != rhs.X) && (lhs.Y != rhs.Y) && (lhs.Z != rhs.Z) && (lhs.W != rhs.W);
        }

        public static bool operator ==(Vector4D lhs, Vector4D rhs)
        {
            return (lhs.X == rhs.X) && (lhs.Y == rhs.Y) && (lhs.Z == rhs.Z) && (lhs.W == rhs.W);
        }

        public override bool Equals(object obj)
        {
            return Equals((Vector4D)obj);
        }

        public bool Equals(Vector4D vec)
        {
            return X == vec.X && Y == vec.Y && Z == vec.Z && W == vec.W;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode() | W.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(X.ToString(), " ", Y.ToString(), " ", Z.ToString(), " ", W.ToString());
        }

        public void Cross(Vector4D v1, Vector4D v2, Vector4D v3)
        {
            this.X = v1.Y * (v2.Z * v3.W - v3.Z * v2.W) - v1.Z * (v2.Y * v3.W - v3.Y * v2.W) + v1.W * (v2.Y * v3.Z - v2.Z * v3.Y);
            this.Y = -(v1.X * (v2.Z * v3.W - v3.Z * v2.W) - v1.Z * (v2.X * v3.W - v3.X * v2.W) + v1.W * (v2.X * v3.Z - v3.X * v2.Z));
            this.Z = v1.X * (v2.Y * v3.W - v3.Y * v2.W) - v1.Y * (v2.X * v3.W - v3.X * v2.W) + v1.W * (v2.X * v3.Y - v3.X * v2.Y);
            this.W = -(v1.X * (v2.Y * v3.Z - v3.Y * v2.Z - v1.Y * (v2.X * v3.Z - v3.X * v2.Z) + v1.Z * (v2.X * v3.Y - v3.X * v2.Y)));
        }
    }
}

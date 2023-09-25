using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EngineLibrary.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2D : IEquatable<Vector2D>
    {
        public float X;
        public float Y;

        public Vector2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2D(Vector2D vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }

        public Vector2D(ReadOnlySpan<float> values)
        {
            if (values.Length < 2)
            {
                Debug.LogError("Scripting> Wrong Parameters for Vector2D!");
            }

            this = Unsafe.ReadUnaligned<Vector2D>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
        }

        public static Vector2D operator +(Vector2D rhs, Vector2D lhs)
        {
            return new Vector2D(rhs.X + lhs.X, rhs.Y + lhs.Y);
        }

        public static Vector2D operator -(Vector2D rhs, Vector2D lhs)
        {
            return new Vector2D(rhs.X - lhs.X, rhs.Y - lhs.Y);
        }

        public override bool Equals(object obj)
        {
            return Equals((Vector2D)obj);
        }

        public bool Equals(Vector2D vec)
        {
            return X == vec.X && Y == vec.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat(X.ToString(), " ", Y.ToString());
        }

        public static Vector2D operator *(Vector2D rhs, Vector2D lhs)
        {
            return new Vector2D(rhs.X * lhs.X, rhs.Y * lhs.Y);
        }

        public static Vector2D operator *(Vector2D rhs, float value)
        {
            return new Vector2D(rhs.X * value, rhs.Y * value);
        }
    }
}

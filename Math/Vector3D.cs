using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace EngineLibrary.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3D : IEquatable<Vector3D>
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(float x, float y, float z)
        {
            this.X = x;
            this.Z = z;
            this.Y = y;
        }

        public Vector3D(Vector3D vector)
        {
            this.X = vector.X;
            this.Z = vector.Z;
            this.Y = vector.Y;
        }

        public Vector3D(ReadOnlySpan<float> values)
        {
            if (values.Length < 3)
            {
                Debug.LogError("Scripting> Wrong Parameters for Vector3D!");
            }

            this = Unsafe.ReadUnaligned<Vector3D>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
        }

        public static Vector3D NormalizeAngles(Vector3D angles)
        {
            angles.X = Math.NormalizeAngle(angles.X * Math.Rad2Deg);
            angles.Y = Math.NormalizeAngle(angles.Y * Math.Rad2Deg);
            angles.Z = Math.NormalizeAngle(angles.Z * Math.Rad2Deg);

            return angles;
        }

        public static Vector3D Max(Vector3D lhs, Vector3D rhs)
        {
            return new Vector3D(
                (lhs.X > rhs.X) ? lhs.X : rhs.X,
                (lhs.Y > rhs.Y) ? lhs.Y : rhs.Y,
                (lhs.Z > rhs.Z) ? lhs.Z : rhs.Z
            );
        }

        public static Vector3D Min(Vector3D lhs, Vector3D rhs)
        {
            return new Vector3D(
                (lhs.X < rhs.X) ? lhs.X : rhs.X,
                (lhs.Y < rhs.Y) ? lhs.Y : rhs.Y,
                (lhs.Z < rhs.Z) ? lhs.Z : rhs.Z
            );
        }

        public static Vector3D Clamp(Vector3D value, Vector3D min, Vector3D max)
        {
            return Min(Max(value, min), max);
        }

        public void CopyTo(float[] array)
        {
            if (array.Length < 3)
            {
                Debug.LogError("Scripting> Wrong parameters with Vector3D!");
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref array[0]), this);
        }

        public static Vector3D MakePositive(Vector3D euler)
        {
            float negativeFlip = -0.0001f * 57.29578f;
            float positiveFlip = 360.0f + negativeFlip;

            if (euler.X < negativeFlip)
                euler.X += 360.0f;
            else if (euler.X > positiveFlip)
                euler.X -= 360.0f;

            if (euler.Y < negativeFlip)
                euler.Y += 360.0f;
            else if (euler.Y > positiveFlip)
                euler.Y -= 360.0f;

            if (euler.Z < negativeFlip)
                euler.Z += 360.0f;
            else if (euler.Z > positiveFlip)
                euler.Z -= 360.0f;

            return euler;
        }

        public static Vector3D operator +(Vector3D rhs, Vector3D lhs)
        {
            return new Vector3D(rhs.X + lhs.X, rhs.Y + lhs.Y, rhs.Z + lhs.Z);
        }

        public static Vector3D operator -(Vector3D rhs, Vector3D lhs)
        {
            return new Vector3D(rhs.X - lhs.X, rhs.Y - lhs.Y, rhs.Z - lhs.Z);
        }

        public static Vector3D operator *(Vector3D rhs, Vector3D lhs)
        {
            return new Vector3D(rhs.X * lhs.X, rhs.Y * lhs.Y, rhs.Z * lhs.Z);
        }

        public static Vector3D operator *(Vector3D rhs, float value)
        {
            return new Vector3D(rhs.X * value, rhs.Y * value, rhs.Z * value);
        }

        public static Vector3D operator /(Vector3D rhs, Vector3D lhs)
        {
            return new Vector3D(rhs.X / lhs.X, rhs.Y / lhs.Y, rhs.Z / lhs.Z);
        }

        public static Vector3D operator /(Vector3D rhs, float value)
        {
            return new Vector3D(rhs.X / value, rhs.Y / value, rhs.Z / value);
        }

        public static bool operator !=(Vector3D lhs, Vector3D rhs)
	    {
		    return (lhs.X != rhs.X) && (lhs.Y != rhs.Y) && (lhs.Z != rhs.Z);
	    }

        public static bool operator ==(Vector3D lhs, Vector3D rhs)
	    {
		    return (lhs.X == rhs.X) && (lhs.Y == rhs.Y) && (lhs.Z == rhs.Z);
	    }

        public static Vector3D Lerp(Vector3D start, Vector3D end, float delta)
        {
            Vector3D v;
            v.X = start.X * (1.0f - delta) + end.X * (delta);
            v.Y = start.Y * (1.0f - delta) + end.Y * (delta);
            v.Z = start.Z * (1.0f - delta) + end.Z * (delta);
            return v;
        }

        public Vector3D Cross(Vector3D rhs)
        {
            return new Vector3D(
                Y * rhs.Z - Z * rhs.Y,
                Z * rhs.X - X * rhs.Z,
                X * rhs.Y - Y * rhs.X);
        }

        public static Vector3D Cross(Vector3D v1, Vector3D v2)
        {
            Vector3D res;
            res.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            res.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            res.Z = (v1.X * v2.Y) - (v1.Y * v2.X);

            return res;
        }

        public static Vector3D FromQuaternion(Quaternion q)
        {
            Vector3D v;
            float t0 = 2.0f * (q.W * q.X + q.Y * q.Z);
            float t1 = 1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
            v.X = (float)System.Math.Atan2(t0, t1) * Math.Rad2Deg;

            float t2 = 2 * (q.Y * q.W - q.Z * q.X);
            t2 = t2 > 1.0f ? 1.0f : t2;

            v.Y = (float)System.Math.Asin(t2) * Math.Rad2Deg;

            float t3 = 2.0f * (q.W * q.Z + q.Y * q.X);
            float t4 = 1.0f - 2.0f * (q.Z * q.Z + q.Y * q.Y);

            v.Z = (float)System.Math.Atan2(t3, t4) * Math.Rad2Deg;

            return v;
        }

        public static Vector3D FromQuaternion(float x, float y, float z, float w)
        {
            Vector3D v;
            float t0 = 2.0f * (w * x + y * z);
            float t1 = 1.0f - 2.0f * (x * x + y * y);
            v.X = (float)System.Math.Atan2(t0, t1) * Math.Rad2Deg;

            float t2 = 2 * (y * w - z * x);
            t2 = t2 > 1.0f ? 1.0f : t2;

            v.Y = (float)System.Math.Asin(t2) * Math.Rad2Deg;

            float t3 = 2.0f * (w * z + y * x);
            float t4 = 1.0f - 2.0f * (z * z + y * y);

            v.Z = (float)System.Math.Atan2(t3, t4) * Math.Rad2Deg;

            return v;
        }
        public float Angle(Vector3D vector)
        {
            float d = Dot(vector);

            return (float)System.Math.Acos(d) * Math.Rad2Deg;
        }

        void Rotate(Vector3D ang)
        {
            float angleDeg = Angle(ang);

            Vector3D axis = Cross(this, ang);

            float halfsin = (float)System.Math.Sin(angleDeg / 2);
            float halfcos = (float)System.Math.Cos(angleDeg / 2);

            Quaternion q = new Quaternion(axis.X * halfsin, axis.Y * halfsin, axis.Z * halfsin, halfcos);

            this = FromQuaternion(q);
        }

        public override bool Equals(object obj)
        {
            return Equals((Vector3D)obj);
        }

        public bool Equals(Vector3D vec)
        {
            return X == vec.X && Y == vec.Y && Z == vec.Z;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat(X.ToString(), " ", Y.ToString(), " ", Z.ToString());
        }

        public static Vector3D Up()
	    {
		    return new Vector3D(0, 1, 0);
        }

        public static Vector3D Right()
        {
            return new Vector3D(1, 0, 0);
        }

        public static Vector3D Forward()
        {
            return new Vector3D(0, 0, 1);
        }

        public float Dot(Vector3D vector)
	    {
		    return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        public float Magnitude()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3D GetConjugate()
	    {
		    return new Vector3D(-X, -Y, -Z);
        }

        public void Normalize()
        {
            float mag = Magnitude();
            if (mag > Math.Epsilon)
            {
                X /= mag;
                Y /= mag;
                Z /= mag;
            }
            else
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
        }

        public static Vector3D Normalize(Vector3D value)
        {
            float mag = value.Magnitude();
            if (mag > Math.Epsilon)
            {
                value.X /= mag;
                value.Y /= mag;
                value.Z /= mag;
            }
            else
            {
                value.X = 0;
                value.Y = 0;
                value.Z = 0;
            }
            return new Vector3D(value.X, value.Y, value.Z);
        }
    }
}
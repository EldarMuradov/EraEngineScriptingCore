using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EngineLibrary.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Quaternion(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public Quaternion(Vector4D vec)
		{
			this.X = vec.X;
			this.Y = vec.Y;
			this.Z = vec.Z;
			this.W = vec.W;
		}

		public Quaternion(ReadOnlySpan<float> values)
		{
			if (values.Length < 4)
			{
				Debug.LogError("Scripting> Wrong Parameters for Vector3D!");
			}

			this = Unsafe.ReadUnaligned<Quaternion>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
		}

		public void Set(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public static Quaternion Identity()
		{
			return new Quaternion(0, 0, 0, 1);
		}

		public float Angle(Quaternion q)
		{
			float d = System.Math.Min(System.Math.Abs(Dot(q)), 1.0f);
			return EqualUsingDot(d) ? 0.0f : (float)System.Math.Acos(d) * 2.0f * Math.Rad2Deg;
		}

		public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
		{
			return new Quaternion(
				lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y,
				lhs.W * rhs.Y + lhs.Y * rhs.W + lhs.Z * rhs.X - lhs.X * rhs.Z,
				lhs.W * rhs.Z + lhs.Z * rhs.W + lhs.X * rhs.Y - lhs.Y * rhs.X,
				lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z);
		}

		public static bool operator ==(Quaternion lhs, Quaternion rhs)
		{
			return EqualUsingDot(lhs.Dot(rhs));
		}

		public static bool operator !=(Quaternion lhs, Quaternion rhs)
		{
			return EqualUsingDot(lhs.Dot(rhs));
		}

		public static Quaternion operator *(Quaternion lhs, float num)
		{
			return new Quaternion(lhs.X * num, lhs.Y * num, lhs.Z * num, lhs.W * num);
		}

		public static Quaternion operator /(Quaternion lhs, float num)
		{
			return new Quaternion(lhs.X / num, lhs.Y / num, lhs.Z / num, lhs.W / num);
		}

		public override bool Equals(object obj)
		{
			return Equals((Quaternion)obj);
		}

		public bool Equals(Quaternion q)
		{
			return X == q.X && Y == q.Y && Z == q.Z && W == q.W;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode() | W.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(X.ToString(), " ", Y.ToString(), " ", Z.ToString(), " ", W.ToString());
		}

		public static Vector3D operator *(Quaternion lhs, Vector3D point)
		{
			float x = lhs.X * 2;
			float y = lhs.Y * 2;
			float z = lhs.Z * 2;
			float xx = x * x;
			float yy = y * y;
			float zz = z * z;
			float xy = x * y;
			float xz = x * z;
			float yz = y * z;
			float wx = lhs.W * x;
			float wy = lhs.W * y;
			float wz = lhs.W * z;

			Vector3D res;
			res.X = (1 - (yy + zz)) * point.X + (xy - wz) * point.Y + (xz + wy) * point.Z;
			res.Y = (xy + wz) * point.X + (1 - (xx + zz)) * point.Y + (yz - wx) * point.Z;
			res.Z = (xz - wy) * point.X + (yz + wx) * point.Y + (1 - (xx + yy)) * point.Z;
			return res;
		}

		public float Dot(Quaternion q)
		{
			return q.X * X + q.Y * Y + q.Z * Z + q.W * W;
		}

		public Quaternion GetConjugate()
		{
			return new Quaternion(-X, -Y, -Z, W);
		}

		public static Quaternion Euler(float x, float y, float z) 
		{
			return EulerToQuaternion(new Vector3D(x, y, z));
		}

		public static Quaternion Euler(Vector3D vector)
		{
			return EulerToQuaternion(vector);
		}

		public float SqrMagnitude(Quaternion q)
		{
			return q.Dot(q);
		}

		public float Magnitude(Quaternion q)
		{
			return (float)System.Math.Sqrt(SqrMagnitude(q));
		}

		public static bool EqualUsingDot(float dot)
		{
			return dot > 1.0f - Math.Epsilon;
		}

		public Quaternion RotateTowards(Quaternion to, float maxDegreesDelta = 360)
		{
			float ang = Angle(to);
			if (ang == 0.0f) 
				return to;
			return Slerp(to, System.Math.Min(1.0f, maxDegreesDelta / ang));
		}

		public Quaternion Slerp(Quaternion q, float t)
		{
			Quaternion ret;

			float fCos = Dot(q);

			if ((1.0f + fCos) > Math.Epsilon)
			{
				float fCoeff0, fCoeff1;

				if ((1.0f - fCos) > Math.Epsilon)
				{
					float omega = (float)System.Math.Acos(fCos);
					float invSin = 1.0f / (float)System.Math.Sin(omega);
					fCoeff0 = (float)System.Math.Sin((1.0f - t) * omega) * invSin;
					fCoeff1 = (float)System.Math.Sin(t * omega) * invSin;
				}
				else
				{
					fCoeff0 = 1.0f - t;
					fCoeff1 = t;
				}

				ret.X = fCoeff0 * X + fCoeff1 * q.X;
				ret.Y = fCoeff0 * Y + fCoeff1 * q.Y;
				ret.Z = fCoeff0 * Z + fCoeff1 * q.Z;
				ret.W = fCoeff0 * W + fCoeff1 * q.W;
			}
			else
			{
				float fCoeff0 = (float)System.Math.Sin((1.0f - t) * Math.Pi * 0.5f);
				float fCoeff1 = (float)System.Math.Sin(t * Math.Pi * 0.5f);

				ret.X = fCoeff0 * X - fCoeff1 * Y;
				ret.Y = fCoeff0 * Y + fCoeff1 * X;
				ret.Z = fCoeff0 * Z - fCoeff1 * W;
				ret.W = Z;
			}

			return ret;
		}

		public void SetLookRotation(Vector3D view)
		{
			Vector3D up = new Vector3D(0, 1, 0);
			SetLookRotation(view, up);
		}

		public void SetLookRotation(Vector3D view, Vector3D up)
		{
			Quaternion q = LookRotation(view, up);
			Set(q.X, q.Y, q.Z, q.W);
		}

		public Quaternion LookRotation(Vector3D forward, Vector3D up)
		{
			forward.Normalize();

			Vector3D vector = Vector3D.Normalize(forward);
			Vector3D vector2 = Vector3D.Normalize(up.Cross(vector));
			Vector3D vector3 = vector.Cross(vector2);
			float m00 = vector2.X;
			float m01 = vector2.Y;
			float m02 = vector2.Z;
			float m10 = vector3.X;
			float m11 = vector3.Y;
			float m12 = vector3.Z;
			float m20 = vector.X;
			float m21 = vector.Y;
			float m22 = vector.Z;

			float num8 = (m00 + m11) + m22;
			Quaternion quaternion = new Quaternion();
			if (num8 > 0)
			{
				float num = (float)System.Math.Sqrt(num8 + 1);
				quaternion.W = num * 0.5f;
				num = 0.5f / num;
				quaternion.X = (m12 - m21) * num;
				quaternion.Y = (m20 - m02) * num;
				quaternion.Z = (m01 - m10) * num;
				return quaternion;
			}
			if ((m00 >= m11) && (m00 >= m22))
			{
				float num7 = (float)System.Math.Sqrt(((1 + m00) - m11) - m22);
				float num4 = 0.5f / num7;
				quaternion.X = 0.5f * num7;
				quaternion.Y = (m01 + m10) * num4;
				quaternion.Z = (m02 + m20) * num4;
				quaternion.W = (m12 - m21) * num4;
				return quaternion;
			}
			if (m11 > m22)
			{
				float num6 = (float)System.Math.Sqrt(((1 + m11) - m00) - m22);
				float num3 = 0.5f / num6;
				quaternion.X = (m10 + m01) * num3;
				quaternion.Y = 0.5f * num6;
				quaternion.Z = (m21 + m12) * num3;
				quaternion.W = (m20 - m02) * num3;
				return quaternion;
			}
			float num5 = (float)System.Math.Sqrt(((1 + m22) - m00) - m11);
			float num2 = 0.5f / num5;
			quaternion.X = (m20 + m02) * num2;
			quaternion.Y = (m21 + m12) * num2;
			quaternion.Z = 0.5f * num5;
			quaternion.W = (m01 - m10) * num2;
			return quaternion;
		}

		public Vector3D QuaternionToEuler()
		{
			float sqw = W * W;
			float sqx = X * X;
			float sqy = Y * Y;
			float sqz = Z * X;
			float unit = sqx + sqy + sqz + sqw;
			float test = X * W - Y * Z;
			Vector3D v;

			if (test > 0.4995f * unit)
			{
				// singularity at north pole
				v.Y = 2 * (float)System.Math.Atan2(Y, X);
				v.X = Math.Pi / 2;
				v.Z = 0;
				return Vector3D.NormalizeAngles(v * Math.Rad2Deg);
			}
			else if (test < -0.4995f * unit)
			{
				// singularity at south pole
				v.Y = -2 * (float)System.Math.Atan2(Y, X);
				v.X = -Math.Pi / 2;
				v.Z = 0;
				return Vector3D.NormalizeAngles(v * Math.Rad2Deg);
			}

			Quaternion q = new Quaternion(W, Z, X, Y);
			v.Y = (float)System.Math.Atan2(2 * q.X * q.W + 2 * q.Y * q.Z, 1 - 2 * (q.Z * q.Z + q.W * q.W));  // Yaw
			v.X = (float)System.Math.Asin(2 * (q.X * q.Z - q.W * q.Y));  // Pitch
			v.Z = (float)System.Math.Atan2(2 * q.X * q.Y + 2 * q.Z * q.W, 1 - 2 * (q.Y * q.Y + q.Z * q.Z));  // Roll
			return Vector3D.NormalizeAngles(v * Math.Deg2Rad);
		}

		public Quaternion NormalizeSafe(Quaternion q)
		{
			float mag = Magnitude(q);
			if (mag < Math.Epsilon)
				return Identity();
			else
				return q / mag;
		}

		private static Quaternion EulerToQuaternion(Vector3D someEulerAngles)
		{
			float cX = ((float)System.Math.Cos(someEulerAngles.X / 2.0f));
			float sX = ((float)System.Math.Sin(someEulerAngles.X / 2.0f));

			float cY = ((float)System.Math.Cos(someEulerAngles.Y / 2.0f));
			float sY = ((float)System.Math.Sin(someEulerAngles.Y / 2.0f));

			float cZ = ((float)System.Math.Cos(someEulerAngles.Z / 2.0f));
			float sZ = ((float)System.Math.Sin(someEulerAngles.Z / 2.0f));

			Quaternion qX = new Quaternion(sX, 0.0f, 0.0f, cX);
			Quaternion qY = new Quaternion(0.0f, sY, 0.0f, cY);
			Quaternion qZ = new Quaternion(0.0f, 0.0f, sZ, cZ);

			Quaternion q = (qY * qX) * qZ;
			return q;
		}
	}
}
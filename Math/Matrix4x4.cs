using System.Runtime.InteropServices;

namespace EngineLibrary.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4
    {
        public float[,] Mat;

        public Matrix4x4(Matrix4x4 matrix)
        {
            Mat = matrix.Mat;
        }

        public void Init()
        {
            Mat = new float[4, 4];
            SetIdentity();
        }

        public void SetIdentity()
        {
            Mat[0, 0] = 1;
            Mat[1, 1] = 1;
            Mat[2, 2] = 1;
            Mat[3, 3] = 1;
        }

        public void SetTranslation(Vector3D translation)
        {
            Mat[3, 0] = translation.X;
            Mat[3, 1] = translation.Y;
            Mat[3, 2] = translation.Z;
        }

        public void SetRotationX(float x)
        {
            Mat[1, 1] = (float)System.Math.Cos(x);
            Mat[1, 2] = (float)System.Math.Sin(x);
            Mat[2, 1] = -(float)System.Math.Sin(x);
            Mat[2, 2] = (float)System.Math.Cos(x);
        }

        public void SetRotationY(float y)
        {
            Mat[0, 0] = (float)System.Math.Cos(y);
            Mat[0, 2] = -(float)System.Math.Sin(y);
            Mat[2, 0] = (float)System.Math.Sin(y);
            Mat[2, 2] = (float)System.Math.Cos(y);
        }

        public void SetRotationZ(float z)
        {
            Mat[0, 0] = (float)System.Math.Cos(z);
            Mat[0, 1] = (float)System.Math.Sin(z);
            Mat[1, 0] = -(float)System.Math.Sin(z);
            Mat[1, 1] = (float)System.Math.Cos(z);
        }

        public void SetRotation(Vector3D rotation)
        {
            SetRotationX(rotation.X);

            SetRotationY(rotation.Y);

            SetRotationZ(rotation.Z);
        }

        public void SetScale(Vector3D scale)
        {
            Mat[0, 0] = scale.X;
            Mat[1, 1] = scale.Y;
            Mat[2, 2] = scale.Z;
        }

        public float GetDeterminant()
        {
            Vector4D minor = new Vector4D(), v1, v2, v3;
            float det;

            v1 = new Vector4D(this.Mat[0, 0], this.Mat[1, 0], this.Mat[2, 0], this.Mat[3, 0]);
            v2 = new Vector4D(this.Mat[0, 1], this.Mat[1, 1], this.Mat[2, 1], this.Mat[3, 1]);
            v3 = new Vector4D(this.Mat[0, 2], this.Mat[1, 2], this.Mat[2, 2], this.Mat[3, 2]);

            minor.Cross(v1, v2, v3);
            det = -(this.Mat[0, 3] * minor.X + this.Mat[1, 3] * minor.Y + this.Mat[2, 3] * minor.Z + this.Mat[3, 3] * minor.W);
            return det;
        }

        public void SetPerspectiveFovLH(float fov, float aspect, float znear, float zfar)
        {
            float yscale = 1.0f / (float)System.Math.Tan(fov / 2.0f);
            float xscale = yscale / aspect;
            Mat[0, 0] = xscale;
            Mat[1, 1] = yscale;
            Mat[2, 2] = zfar / (zfar - znear);
            Mat[2, 3] = 1.0f;
            Mat[3, 2] = (-znear * zfar) / (zfar - znear);
            Mat[3, 3] = 0.0f;
        }

        public void SetOrthoLH(float width, float height, float near_plane, float far_plane)
        {
            SetIdentity();
            Mat[0, 0] = 2.0f / width;
            Mat[1, 1] = 2.0f / height;
            Mat[2, 2] = 1.0f / (far_plane - near_plane);
            Mat[3, 2] = -(near_plane / (far_plane - near_plane));
        }

        public void Inverse()
        {
            int a, i, j;
            Matrix4x4 outmat = new Matrix4x4();
            Vector4D v = new Vector4D();
            Vector4D[] vec = new Vector4D[4];
           
            float det = this.GetDeterminant();
            if (det <= 0)
                return;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (j != i)
                    {
                        a = j;
                        if (j > i) a = a - 1;
                        vec[a].X = (this.Mat[j, 0]);
                        vec[a].Y = (this.Mat[j, 1]);
                        vec[a].Z = (this.Mat[j, 2]);
                        vec[a].W = (this.Mat[j, 3]);
                    }
                }

                v.Cross(vec[0], vec[1], vec[2]);

                outmat.Mat[0, i] = (float)System.Math.Pow(-1.0f, i) * v.X / det;
                outmat.Mat[1, i] = (float)System.Math.Pow(-1.0f, i) * v.Y / det;
                outmat.Mat[2, i] = (float)System.Math.Pow(-1.0f, i) * v.Z / det;
                outmat.Mat[3, i] = (float)System.Math.Pow(-1.0f, i) * v.W / det;
            }

            this.Mat = outmat.Mat;
        }

        public float this[int index, int sindex]
        {
            get
            {
                return Mat[index, sindex];
            }
            set
            {
                Mat[index, sindex] = value;
            }
        }

        public Vector4D this[int index]
        {
            get
            {
                return new Vector4D(Mat[index, 0], Mat[index, 1], Mat[index, 2], Mat[index, 3]);
            }
            set
            {
                Mat[index, 0] = value.X;
                Mat[index, 1] = value.Y;
                Mat[index, 2] = value.Z;
                Mat[index, 3] = value.W;
            }
        }

        public Matrix4x4 Mul(Matrix4x4 mat)
	    {
		    Matrix4x4 outmat = new Matrix4x4();
		    for (int i = 0; i < 4; i++)
		    {
		    	for (int j = 0; j < 4; j++)
		    	{
                    outmat.Mat[i, j] = Mat[i, 0] * mat.Mat[0, j] + Mat[i, 1] * mat.Mat[1, j] +
                        Mat[i, 2] * mat.Mat[2, j] + Mat[i, 3] * mat.Mat[3, j];
		    	}
            }
            Mat = outmat.Mat;

            return this;
	    }

        public Vector3D GetZDirection()
        {
            return new Vector3D(Mat[2, 0], Mat[2, 1], Mat[2, 2]);
        }

        public Vector3D GetXDirection()
        {
            return new Vector3D(Mat[0, 0], Mat[0, 1], Mat[0, 2]);
        }

        public Vector3D GetTranslation()
        {
            return new Vector3D(Mat[3, 0], Mat[3, 1], Mat[3, 2]);
        }
    }
}
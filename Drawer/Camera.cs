using System;
namespace Drawer
{
    internal struct Vector3F
    {
        public float X;
        public float Y;
        public float Z;
        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static Vector3F operator +(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vector3F operator -(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3F operator *(Vector3F v1, double d)
        {
            return new Vector3F((float)(v1.X * d), (float)(v1.Y * d), (float)(v1.Z * d));
        }
        public static Vector3F operator /(Vector3F v1, double d)
        {
            d = 1 / d;
            return v1 * d;
        }
        public static bool operator ==(Vector3F v1, Vector3F v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(Vector3F v1, Vector3F v2)
        {
            return !(v1 == v2);
        }
        public static Vector3F Cross(Vector3F a, Vector3F b)
        {
            return new Vector3F(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }
        public static Vector3F Normalize(Vector3F v)
        {
            if (v.Norma != 0.0) return new Vector3F
            {
                X = (float)(v.X / v.Norma),
                Y = (float)(v.Y / v.Norma),
                Z = (float)(v.Z / v.Norma)
            };
            return new Vector3F();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3F))
            {
                return false;
            }
            var f = (Vector3F)obj;
            return X == f.X &&
                   Y == f.Y &&
                   Z == f.Z;
        }
        public override int GetHashCode()=> base.GetHashCode();
        public double Norma=> Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
    }
    internal struct Camera
    {
        public Vector3F Position;
        public Vector3F Direction;
        public Vector3F Top;
        public bool isDragging;
        public bool isRotating;
        public double mouseX;
        public double mouseY;
        
    }
}

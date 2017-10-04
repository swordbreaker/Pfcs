using System.Numerics;

namespace WpfOpenGlLibrary.Helpers
{
    public static class Matrix4x4Extention
    {
        public static float[] ToArray(this Matrix4x4 m)
        {
            return new[]
            {
                m.M11, m.M21, m.M31, m.M41,
                m.M12, m.M22, m.M32, m.M42,
                m.M13, m.M23, m.M33, m.M43,
                m.M14, m.M24, m.M34, m.M44,
            };
        }


        //m00, m10, m20, m30, 
        //m01, m11, m21, m31, 
        //m02, m12, m22, m32, 
        //m03, m13, m23, m33
    }
}

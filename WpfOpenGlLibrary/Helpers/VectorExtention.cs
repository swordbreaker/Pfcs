using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfOpenGlLibrary.Helpers
{
    public static class VectorExtention
    {
        public static float[] ToArray(this Vector3 v)
        {
            return new[] {v.X, v.Y, v.Z};
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Media;
using OpenGL;

namespace WpfOpenGlLibrary.Helpers
{
    public class VertexHelper
    {
        public Color CurrentColor { get; set; } = Colors.Black;

        private readonly List<float> _verts = new List<float>();
        private readonly List<float> _colors = new List<float>();

        public void PutMany(IEnumerable<Vector3> verts, Color? color = null)
        {
            foreach (var vector3 in verts)
            {
                Put(vector3, color);
            }
        }

        public void PutMany(IEnumerable<Vector2> verts, Color? color = null)
        {
            PutMany(verts.Select(v => new Vector3(v.X, v.Y, 0)), color);
        }

        public void Put(float x, float y, float z, Color? color = null)
        {
            var c = color ?? CurrentColor;

            _verts.Add(x);
            _verts.Add(y);
            _verts.Add(z);

            _colors.Add(c.R);
            _colors.Add(c.G);
            _colors.Add(c.B);
            _colors.Add(c.A);
        }

        public void Put(float x, float y, Color? color = null) => Put(x, y, 0, color);
        public void Put(Vector3 v, Color? color = null) =>Put(v.X, v.Y, v.Z, color);
        public void Put(Vector2 v, Color? color = null) => Put(v.X, v.Y, color);

        public void Draw(PrimitiveType type)
        {
            using (var colorArrayLock = new MemoryLock(_colors.ToArray()))
            using (var vertexArrayLock = new MemoryLock(_verts.ToArray()))
            {
                Gl.ColorPointer(4, ColorPointerType.Float, 0, colorArrayLock.Address);
                Gl.EnableClientState(EnableCap.ColorArray);

                Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);
                Gl.EnableClientState(EnableCap.VertexArray);

                Gl.DrawArrays(type, 0, _verts.Count / 3);
            }
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        private readonly List<float> _normals = new List<float>();

        public void PutMany(Vector3[] verts, Color? color = null, Vector3[] normals = null)
        {
            Contract.Assert(normals == null || verts.Length == normals.Length, "normals == null || verts.Length == normals.Length");

            for (var i = 0; i < verts.Length; i++)
            {
                var n = normals?[i] ?? Vector3.Zero;
                Put(verts[i], color, n);
            }
        }

        public void PutMany(Vector2[] verts, Color? color = null, Vector3[] normals = null)
        {
            PutMany(verts.Select(v => new Vector3(v.X, v.Y, 0)).ToArray(), color, normals);
        }

        public void Put(float x, float y, float z, Color? color = null, Vector3 normal = default(Vector3))
        {
            var c = color ?? CurrentColor;

            _verts.Add(x);
            _verts.Add(y);
            _verts.Add(z);

            _colors.Add((float)c.R / 255);
            _colors.Add((float)c.G / 255);
            _colors.Add((float)c.B / 255);
            _colors.Add((float)c.A / 255);

            _normals.Add(normal.X);
            _normals.Add(normal.Y);
            _normals.Add(normal.Z);
        }

        public void Put(float x, float y, Color? color = null, Vector3 normal = default(Vector3)) => Put(x, y, 0, color, normal);
        public void Put(Vector3 v, Color? color = null, Vector3 normal = default(Vector3)) => Put(v.X, v.Y, v.Z, color, normal);
        public void Put(Vector2 v, Color? color = null, Vector3 normal = default(Vector3)) => Put(v.X, v.Y, color, normal);

        public void Draw(PrimitiveType type)
        {
            using (var colorArrayLock = new MemoryLock(_colors.ToArray()))
            using (var vertexArrayLock = new MemoryLock(_verts.ToArray()))
            using (var normalArrayLock = new MemoryLock(_normals.ToArray()))
            {
                Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);
                Gl.EnableClientState(EnableCap.VertexArray);

                Gl.ColorPointer(4, ColorPointerType.Float, 0, colorArrayLock.Address);
                Gl.EnableClientState(EnableCap.ColorArray);

                Gl.NormalPointer(NormalPointerType.Float, 0, normalArrayLock.Address);
                Gl.EnableClientState(EnableCap.NormalArray);

                Gl.DrawArrays(type, 0, _verts.Count / 3);
            }
        }
    }
}
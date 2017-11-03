using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace WpfOpenGlLibrary.Helpers
{
    public static class FiguresHelper
    {
        public static void DrawCircle(float r, Vector2 center, int n, Color? color = null)
        {
            var vHelper = new VertexHelper {CurrentColor = color ?? Colors.Black};

            var phi = 2f * (float)Math.PI / n;
            float x, y;

            for (int i = 0; i <= n; i++)
            {
                x = center.X + r * (float)Math.Cos(phi * i);
                y = center.Y + r * (float)Math.Sin(phi * i);
                vHelper.Put(x, y, normal: Vector3.UnitZ);
            }

            Gl.LineWidth(5);
            vHelper.Draw(PrimitiveType.TriangleFan);
        }

        public static void DrawPolygon(IEnumerable<Vector2> vecs, Color? color)
        {
            var vHelper = new VertexHelper {CurrentColor = color ?? Colors.Black};
            vHelper.PutMany(vecs.ToArray(), color);
            vHelper.Draw(PrimitiveType.Polygon);
        }

        public static void DrawRectangle(Rect rect, Color? color)
        {
            var vecs = new[]
            {
                rect.TopLeft.ToVector2(),
                rect.TopRight.ToVector2(),
                rect.BottomRight.ToVector2(),
                rect.BottomLeft.ToVector2()
            };
            DrawPolygon(vecs, color);
        }

        public static void DrawLine(Vector2 start, Vector2 end, float lineWidth, Color? color = null)
        {
            DrawLine(new Vector3(start, 0f), new Vector3(end, 0f), lineWidth, color);
        }

        public static void DrawLine(Vector3 start, Vector3 end, float lineWidth, Color? color = null)
        {
            Gl.LineWidth(lineWidth);
            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(new[] { start, end }, color);
            vHelper.Draw(PrimitiveType.Lines);
        }

        public static void DrawLine(Vector2[] verts, float lineWidth, Color? color = null)
        {
            Gl.LineWidth(lineWidth);
            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(verts, color);
            vHelper.Draw(PrimitiveType.Lines);
        }

        public static void DrawLine(Vector3[] verts, float lineWidth, Color? color = null)
        {
            Gl.LineWidth(lineWidth);
            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(verts, color);
            vHelper.Draw(PrimitiveType.Lines);
        }

        public static void DrawSpear(Vector2 start, float a, float b, float c, Color? color = null)
        {
            var vecs = new[]
            {
                //Triangle
                new Vector2(-a, b),
                new Vector2(-(a+c), 0),
                new Vector2(-a, -b) ,
                //Square
                new Vector2(-a, -b), 
                new Vector2(a, -b), 
                new Vector2(a, b), 
                new Vector2(-a, b),
                //Triangle
                new Vector2(a, -b), 
                new Vector2(a + c, 0), 
                new Vector2(a, b)
            };

            DrawPolygon(vecs, color);
        }

        public static void DrawCube(Vector3 center, float a, Color? color = null)
        {
            var a2 = a / 2f;
            var c = center;

            var verts = new[]
            {
                new Vector3(c.X - a2, c.Y - a2, c.Z - a2),
                new Vector3(c.X - a2, c.Y + a2, c.Z - a2),
                new Vector3(c.X + a2, c.Y + a2, c.Z - a2),
                new Vector3(c.X + a2, c.Y - a2, c.Z - a2),
                new Vector3(c.X + a2, c.Y - a2, c.Z + a2),
                new Vector3(c.X + a2, c.Y + a2, c.Z + a2),
                new Vector3(c.X - a2, c.Y + a2, c.Z + a2),
                new Vector3(c.X - a2, c.Y - a2, c.Z + a2),
            };

            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(verts, color);
            vHelper.Draw(PrimitiveType.TriangleFan);
        }

        /// <summary>
        /// Draws a 3d cross. X blue, y red and z green
        /// </summary>
        /// <param name="lineLenght">The line lenght.</param>
        /// <param name="lineWidth">Width of the line.</param>
        public static void Draw3DCross(float lineLenght, float lineWidth)
        {
            DrawLine(Vector3.Zero, Vector3.UnitX * lineLenght, lineWidth, Colors.Blue);
            DrawLine(Vector3.Zero, Vector3.UnitY * lineLenght, lineWidth, Colors.Red);
            DrawLine(Vector3.Zero, Vector3.UnitZ * lineLenght, lineWidth, Colors.Green);
        }
    }
}

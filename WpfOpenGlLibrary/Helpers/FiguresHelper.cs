using OpenGL;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace WpfOpenGlLibrary.Helpers
{
    public static class FiguresHelper
    {
        public static void DrawCircle(float r, Vector2 center, int n, Color? color = null)
        {
            var vHelper = new VertexHelper();
            vHelper.CurrentColor = color ?? Colors.Black;

            var phi = 2f * (float)Math.PI / n;
            float x, y;

            for (int i = 0; i <= n; i++)
            {
                x = center.X + r * (float)Math.Cos(phi * i);
                y = center.Y + r * (float)Math.Sin(phi * i);
                vHelper.Put(x, y);
            }

            Gl.LineWidth(5);
            vHelper.Draw(PrimitiveType.TriangleFan);
        }

        public static void DrawPolygon(IEnumerable<Vector2> vecs, Color? color)
        {
            var vHelper = new VertexHelper {CurrentColor = color ?? Colors.Black};
            vHelper.PutMany(vecs, color);
            vHelper.Draw(PrimitiveType.Polygon);
        }

        public static void DrawRectangle(Rect rect, Color? color)
        {
            var vecs = new Vector2[]
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
            Gl.LineWidth(lineWidth);
            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(new[]{start, end}, color);
            vHelper.Draw(PrimitiveType.Lines);
        }
    }
}

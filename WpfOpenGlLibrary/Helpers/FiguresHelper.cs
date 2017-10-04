using OpenGL;
using System;
using System.Collections.Generic;
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
            Gl.LineWidth(lineWidth);
            var vHelper = new VertexHelper { CurrentColor = color ?? Colors.Black };
            vHelper.PutMany(new[]{start, end}, color);
            vHelper.Draw(PrimitiveType.Lines);
        }

        public static void DrawSpear(Vector2 start, float a, float b, float c, Color? color = null)
        {
            //Matrix4x4 m = Matrix4x4.Identity;
            //// m.translate(start.X, start.Y, 0)M

            ////double alpha = atan(vy/vx) * 180 /Math.Pi;
            //// m = M.postmultiply(Math.rotate(alpha, 0,0,1);

            ////m.Translation = new Vector3(start.X, start.Y, 0);
            //m *= Matrix4x4.CreateTranslation(new Vector3(start.X, start.Y, 0));
            //m *= Matrix4x4.CreateRotationZ(30f * 180f / (float) Math.PI);
            //m = t * r;

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

            //for (var i = 0; i < vecs.Length; i++)
            //{
            //    vecs[i] = Vector2.Transform(vecs[i], m);
            //}

            DrawPolygon(vecs, color);
            //Triangle 1
            //a , -b
            //a + c , 0
            //a, b

            //Triangle 2
            //-a , b
            //-(a+c) , 0
            //-a, -b

            //M = T * R 
            //M = Mat.ID
            //M = Mat4.translate(tx, 0, 0):
            //M = M.premultiply(Mat4.rotate(30,0,0,1));
            //mygl.SetM(gl, M);

            // M = abs *  T * R * rel
            
            //Realtive drehung
            // M = M * R

            //Absolute drehung
            // M = R * M

            //DrawSpear(1.2f, 0.04f, 0.2f);
            //
        }
    }
}

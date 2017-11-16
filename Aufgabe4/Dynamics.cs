using System;
using System.Numerics;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Aufgabe4
{
    public abstract class Dynamics
    {
        public float[] Euler(float[] x, float dt)
        {
            var y = F(x);

            var xx = new float[x.Length]; //x + y * dt
            for (int i = 0; i < xx.Length; i++)
            {
                xx[i] = x[i] + y[i] * dt;
            }

            return xx;
        }

        protected abstract float[] F(float[] xs);

        public float[] Runge(float[] x, float dt)
        {
            var y1 = F(x);

            var xx = new float[x.Length];

            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + (dt / 2) * y1[i];

            var y2 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + (dt / 2) * y2[i];

            var y3 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + dt * (y3[i]) / 6;

            var y4 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + dt * (y1[i] + 2*y2[i] + 2*y3[i] + y4[i]) / 6;

            return xx;
        }
    }

    public class CylinderFlow : Dynamics
    {
        public float Radius;
        public float W = 0;
        public Matrix4x4 M = Matrix4x4.Identity;

        public CylinderFlow(float radius)
        {
            Radius = radius;
        }

        protected override float[] F(float[] xs)
        {
            var R2 = Radius * Radius;
            var v = new Vector2(xs[0], xs[1]);
            v = Vector2.Transform(v, Matrix4x4.CreateScale(-1) * M);
            var x = v.X;
            var y = v.Y;
            var x2y2 = x * x + y * y;

            var a1 = new[]
            {
                W/x2y2 * y  + (1 + R2 / x2y2 - 2 * R2 * x * x / (x2y2 * x2y2)),
                W/x2y2 * -x - (2 * R2 * x * y / (x2y2 * x2y2))
            };

            //v = new Vector2(xs[0], xs[1]);
            //v = Vector2.Transform(v, Matrix4x4.CreateTranslation(-6,0,0));
            //x = v.X;
            //y = v.Y;
            //x2y2 = x * x + y * y;

            //var a2 = new[]
            //{
            //    W/x2y2 * y  + (1 + R2 / x2y2 - 2 * R2 * x * x / (x2y2 * x2y2)),
            //    W/x2y2 * -x - (2 * R2 * x * y / (x2y2 * x2y2))
            //};

            return new[]
            {
                a1[0],// + a2[0],
                a1[1],// + a2[1]
            };
        }

        public CylinderFlowLineDrawer GetLineDrawer(Vector2 startPos) => new CylinderFlowLineDrawer(this, startPos);

        public class CylinderFlowLineDrawer
        {
            private readonly CylinderFlow _cf;
            private float[] _xs;
            private float _currentDt;

            internal CylinderFlowLineDrawer(CylinderFlow cf, Vector2 startPos)
            {
                _cf = cf;
                _xs = startPos.ToArray();
            }

            public void DrawLine(float dt, int steps)
            {
                VertexHelper.Clear();
                VertexHelper.CurrentColor = Colors.LightGreen;

                for (int i = 0; i < steps; i++)
                {
                    var t = ((float)Math.Sin(_currentDt) + 1f) / 2f;
                    VertexHelper.Put(_xs[0], _xs[1], Color.FromScRgb(1,.0f,t,0f));
                    _xs = _cf.Runge(_xs, dt);
                    _currentDt += dt;
                }

                VertexHelper.Draw(PrimitiveType.Points);
            }

            public void Skip(float dt)
            {
                _xs = _cf.Runge(_xs, dt);
                _currentDt += dt;
            }
        }
    }

    public class Lorenz : Dynamics
    {
        protected override float[] F(float[] xs)
        {
            var y = new[]
            {
                10 * xs[1] - 10 * xs[0],
                28 * xs[0] - xs[1] - xs[0] * xs[2],
                xs[0] * xs[1] - 8 * xs[2] / 3
            };

            return y;
        }

        public void DrawLain(Vector3 start, float dt, int steps)
        {
            VertexHelper.Clear();
            VertexHelper.CurrentColor = Colors.Black;
            VertexHelper.Put(start);

            var xs = start.ToArray();

            for (int i = 1; i < steps; i++)
            {
                xs = Runge(xs, dt);
                VertexHelper.Put(xs[0], xs[1], xs[2]);
            }

            VertexHelper.Draw(PrimitiveType.LineStrip);
        }
    }
}
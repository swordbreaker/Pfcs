using System;
using System.Numerics;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;

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

        protected abstract float[] F(float[] x);

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

    public class Lorenz : Dynamics
    {
        protected override float[] F(float[] x)
        {
            var y = new[]
            {
                10 * x[1] - 10 * x[0],
                28 * x[0] - x[1] - x[0] * x[2],
                x[0] * x[1] - 8 * x[2] / 3
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
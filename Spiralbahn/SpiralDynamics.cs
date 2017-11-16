using System.Linq;
using System.Numerics;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;

namespace Spiralbahn
{
    public class SpiralDynamics : Dynamics
    {
        private Vector3 B = new Vector3(1,1,-1); //Magnet feld
        private float q = -1; //elektr. Ladung
        private float m = 1; // masse

        protected override float[] F(float[] xs)
        {
            var pos = new Vector3(xs[0], xs[1], xs[2]);
            var v = new Vector3(xs[3], xs[4], xs[5]);
            var f = q * Vector3.Cross(v, B);

            var y = new[]
            {
                v.X, v.Y, v.Z,
                f.X/m, f.Y/m, f.Z/m 
            };

            return y;
        }

        public void DrawLain(Vector3 start, Vector3 vStart, float dt, int steps)
        {
            VertexHelper.Clear();
            VertexHelper.CurrentColor = Colors.Black;
            VertexHelper.Put(start);

            var xs = start.ToArray().Concat(vStart.ToArray()).ToArray();

            for (int i = 1; i < steps; i++)
            {
                xs = Runge(xs, dt);
                VertexHelper.Put(xs[0], xs[1], xs[2]);
            }

            VertexHelper.Draw(PrimitiveType.LineStrip);
        }
    }
}

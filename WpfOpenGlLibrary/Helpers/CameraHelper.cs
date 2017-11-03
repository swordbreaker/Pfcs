using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfOpenGlLibrary.Helpers
{
    public class CameraHelper
    {
        public float Elevation { get; set; }
        public float Azimut { get; set; }
        public float Radius { get; set; }

        public CameraHelper(float elevation = 0f, float azimut = 0f, float radius = 20f)
        {
            Elevation = elevation;
            Azimut = azimut;
            Radius = radius;
        }

        public void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.Up:
                    Elevation += 0.1f;
                    break;
                case Key.Down:
                    Elevation -= 0.1f;
                    break;
                case Key.Left:
                    Azimut -= 0.1f;
                    break;
                case Key.Right:
                    Azimut += 0.1f;
                    break;
            }
        }

        public Matrix4x4 CameraMatrix
        {
            get
            {
                var a = new Vector3(0, 0, Radius);
                var b = new Vector3(0, 0, 0);
                var up = new Vector3(0, 1, 0);

                var r1 = Matrix4x4.CreateRotationX(Elevation);
                var r2 = Matrix4x4.CreateRotationY(-Azimut);
                var r = r1 * r2;

                return Matrix4x4.CreateLookAt(Vector3.Transform(a, r), b, Vector3.Transform(up, r));
            }
        }
    }
}

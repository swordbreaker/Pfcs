using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;
using PixelFormat = OpenGL.PixelFormat;

namespace Aufgabe3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const float ViewPortLeft = -10;
        private const float ViewPortRight = 10;
        private const float ViewPortTop = 10;
        private const float ViewPortBottom = -10;
        private const float ViewPortNear = -200;
        private const float ViewPortFar = 200;

        private float _elevation = 0;
        private float _azimut = 0;
        private float _phi = 0;

        public MainWindow()
        {
            InitializeComponent();
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += OnKeyDown;

            Gl.Enable(EnableCap.DepthTest);
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.Up:
                    _elevation += 0.1f;
                    break;
                case Key.Down:
                    _elevation -= 0.1f;
                    break;
                case Key.Left:
                    _azimut -= 0.1f;
                    break;
                case Key.Right:
                    _azimut += 0.1f;
                    break;
            }
        }

        private void Render(object sender, GlControlEventArgs e)
        {

            //U = Model matrix
            //V = View matrix
            //X = U * X
            //X' = V * X

            //M = V * U * x

            //mat4 V = Mat4.lookAt(eye, target, up);

            // r = 20
            // A = (0,0,r)
            // B = (0,0,0)
            // up = (0,1,0)

            // elevation = 20
            // azimut = 45
            // R1 = Mat4.Rotate(-elevation, 1, 0,0);
            // R2 = Mat4.Rotate(azimut, 0, 1, 0)
            // R = R1.preMultipyl(R2)
            // V = Mat4.lookAt(R.transform(A), B, R.trasnform(up));


            //Mat4 U = Mat4.translate(2, 1, 0);
            //Mat4 R = Mat4.roatet(45, 

            //FiguresHelper.Draw3DCross(200f, 2f);

            //FiguresHelper.DrawCube(Vector3.Zero, 0.5f);


            Gl.Clear(ClearBufferMask.DepthBufferBit);

            var v = CameraMovement();

            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.LoadMatrix(v.ToArray());
            FiguresHelper.Draw3DCross(4, 1);

            var r1 = Matrix4x4.CreateRotationY(Mathf.ToRadian(_phi));
            var t = Matrix4x4.CreateTranslation(new Vector3(2f, 2f, 0f));
            var r2 = Matrix4x4.CreateRotationY(Mathf.ToRadian(90));
            var m = r2 * t * r1;

            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.MultMatrix(m.ToArray());

            FiguresHelper.DrawCircle(0.5f, new Vector2(0,0), 50, Colors.Chocolate);

            _phi += 1f;
        }

        private Matrix4x4 CameraMovement()
        {
            var radius = 20f;
            var a = new Vector3(0, 0, radius);
            var b = new Vector3(0, 0, 0);
            var up = new Vector3(0, 1, 0);

            var r1 = Matrix4x4.CreateRotationX(-_elevation);
            var r2 = Matrix4x4.CreateRotationY(_azimut);
            var r = r1 * r2;


            var v = Matrix4x4.CreateLookAt(Vector3.Transform(a, r), b, Vector3.Transform(up, r));

            return v;
        }
    }
}

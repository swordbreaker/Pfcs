using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using WpfOpenGlLibrary.Models;
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
        private const float ViewPortNear = -200f;
        private const float ViewPortFar = 200;

        //private const float ViewPortLeft = -0.001f;
        //private const float ViewPortRight = 0.001f;
        //private const float ViewPortTop = 0.001f;
        //private const float ViewPortBottom = -0.001f;
        //private const float ViewPortNear = 0.001f;
        //private const float ViewPortFar = 100;

        private float _elevation = 0;
        private float _azimut = 0;
        private float _phi = 0;

        private ShaderHelper _shader;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += OnKeyDown;
            
            Gl.Enable(EnableCap.DepthTest);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _shader = OpenGlWpfControl.Shader;
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
            if(_shader == null) return;

            var v = CameraMovement();

            //Gl.MatrixMode(MatrixMode.Modelview);
            //Gl.LoadMatrix(v.ToArray());

            _shader.M = v;

            FiguresHelper.Draw3DCross(4, 1);

            var lPos = new[] {1f, 1f, 1f};
            _shader.LightPos = new Vector3(1f, 1f, 1f);
            //Gl.Light(LightName.Light0, LightParameter.Position, lPos);

            //Gl.Enable(EnableCap.Lighting);
            //Gl.Enable(EnableCap.Light0);

            var scaleM = Matrix4x4.CreateScale(0.01f) * v;
            //Gl.PushMatrix();
            //Gl.LoadMatrix(scaleM.ToArray());
            _shader.M = scaleM;
            Figure3DHelper.DrawMesh(new Mesh(@"boomerang.obj"), Colors.Chocolate);
            //Gl.PopMatrix();

            var r1 = Matrix4x4.CreateRotationY(Mathf.ToRadian(_phi));
            var t = Matrix4x4.CreateTranslation(new Vector3(2f, 2f, 0f));
            var r2 = Matrix4x4.CreateRotationY(Mathf.ToRadian(90));
            var m = r2 * t * r1;

            //Gl.MatrixMode(MatrixMode.Modelview);
            _shader.M = m * v;
            //Gl.MultMatrix(m.ToArray());

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

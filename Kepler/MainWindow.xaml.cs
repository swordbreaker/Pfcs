using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Aufgabe2;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Kepler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const float ViewPortLeft = -60;
        private const float ViewPortRight = 60;
        private const float ViewPortTop = 60;
        private const float ViewPortBottom = -60;
        private const float ViewPortNear = 100f;
        private const float ViewPortFar = -100f;

        private const float G = 9.81e-6f;
        private const float RE = 6.378f;
        private const float GM = G * RE * RE;

        private readonly Projectile _satelite;

        private readonly CameraHelper _cameraHelper = new CameraHelper(10, 40, 10f);

        public MainWindow()
        {
            InitializeComponent();

            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += _cameraHelper.OnKeyDown;
            KeyDown += OnKeyDown;

            var pos = new Vector2(42,0);
            var v0 = new Vector2(0, (float)Math.Sqrt(GM / 42));

            _satelite = new Projectile(v0, pos, DrawSphere);

            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.PolygonOffsetFill);
            Gl.PolygonOffset(1,1);
            Gl.DepthMask(true);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
        }

        private void Render(object sender, GlControlEventArgs e)
        {
            Gl.Clear(ClearBufferMask.DepthBufferBit);
            Gl.MatrixMode(MatrixMode.Modelview);

            var v = _cameraHelper.CameraMatrix;
            var u = Matrix4x4.CreateRotationX(Mathf.ToRadian(90));
            
            var m = u * v;

            Gl.LoadMatrix(m.ToArray());

            FiguresHelper.Draw3DCross(50f, 1f);

            Figure3DHelper.DrawSphere(20,20,20, true, Colors.Yellow);
            Gl.LineWidth(1);
            Figure3DHelper.DrawSphere(20,20,20, false, Colors.Red);
            //FiguresHelper.DrawCircle(RE, new Vector2(0,0), 20, Colors.Yellow);
           
            _satelite.Draw();
        }

        private static void DrawCircle(Vector2 pos, Vector2 v, Vector2 a, Color color)
        {
            FiguresHelper.DrawCircle(0.1f*RE, pos, 20, color);
        }

        private static void DrawSphere(Vector2 pos, Vector2 v, Vector2 a, Color color)
        {
            var t = Matrix4x4.CreateTranslation(new Vector3(pos, 0));
            Gl.MultMatrix(t.ToArray());
            Figure3DHelper.DrawSphere(2, 20, 20, true, color);
        }
    }
}

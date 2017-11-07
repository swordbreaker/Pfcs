using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;

namespace Aufgabe4
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
        private const float ViewPortNear = -100f;
        private const float ViewPortFar = 100f;

        private ShaderHelper _shader;
        private readonly CameraHelper _cameraHelper = new CameraHelper(0, 0, 10f);
        private readonly Lorenz _lorenz = new Lorenz();


        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += _cameraHelper.OnKeyDown;
            KeyDown += OnKeyDown;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _shader = OpenGlWpfControl.Shader;
            Gl.Enable(EnableCap.DepthTest);
            Gl.DepthFunc(DepthFunction.Lequal);
            Gl.Enable(EnableCap.PolygonOffsetFill);
            Gl.PolygonOffset(1, 1);
            Gl.DepthMask(true);
        }

        private void Render(object sender, GlControlEventArgs e)
        {
            if (_shader == null) return;

            var v = _cameraHelper.CameraMatrix;

            _shader.M = v;

            FiguresHelper.Draw3DCross(500, 2);
            Gl.LineWidth(1);
            _lorenz.DrawLain(new Vector3(10, 10, -100), 0.01f, 5000);
            //DrawLain(-20, -20, 0, 0, 1, 0, 0.0005f, 500);              // Spirale
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        //  -------  Klothoide (Cornu'sche Spirale) ------------
        public void DrawLain(float xStart, float yStart, float zStart,
            float phi, float ds, float kruemmung,
            float dKruemmung, int nPunkte)
        {
            VertexHelper.Clear();
            float x = xStart, y = yStart, z = zStart;

            VertexHelper.Put(x,y,z);
            for (int i = 1; i < nPunkte; i++)
            {
                x += (float)Math.Cos(phi) * ds;
                y += (float)Math.Sin(phi) * ds;
                VertexHelper.Put(x,y,z);
                phi += kruemmung * ds;
                kruemmung += dKruemmung * ds;
            }

            VertexHelper.Draw(PrimitiveType.LineStrip);
        }
    }
}

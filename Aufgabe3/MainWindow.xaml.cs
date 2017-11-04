using System.Numerics;
using System.Windows;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using WpfOpenGlLibrary.Models;
using Matrix4x4 = System.Numerics.Matrix4x4;

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
        private const float ViewPortNear = 1f;
        private const float ViewPortFar = 20f;

        private float _phi = 0;

        private ShaderHelper _shader;
        private readonly CameraHelper _cameraHelper = new CameraHelper(0,0,5f);

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += _cameraHelper.OnKeyDown;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
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
            if(_shader == null) return;

            var v = _cameraHelper.CameraMatrix;

            _shader.M = v;

            _shader.ShadingLevel = 0;
            FiguresHelper.Draw3DCross(4, 1);
            _shader.ShadingLevel = 1;

            _shader.LightPos = new Vector3(1f, 1f, 1f);

            var r1 = Matrix4x4.CreateRotationY(Mathf.ToRadian(_phi));
            var t = Matrix4x4.CreateTranslation(new Vector3(5f, 2f, 0f));
            var r2 = Matrix4x4.CreateRotationX(Mathf.ToRadian(90)) * Matrix4x4.CreateRotationY(Mathf.ToRadian(90)) * Matrix4x4.CreateRotationZ(Mathf.ToRadian(45));
            var scaleM = Matrix4x4.CreateScale(0.2f);
            var r3 = Matrix4x4.CreateRotationY(Mathf.ToRadian(-_phi * 10));
            var m = r3 * r2 * t * r1 * scaleM;

            _shader.M = m * v;

            Figure3DHelper.DrawMesh(new Mesh(@"boomerang.obj"), Colors.Chocolate);

            _phi += 3f;
        }
    }
}

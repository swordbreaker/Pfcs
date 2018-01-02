using System.Numerics;
using System.Windows;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using WpfOpenGlLibrary.Models;

namespace SlerpTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const float ViewPortLeft = -10;
        private const float ViewPortRight = 10;
        private const float ViewPortTop = -10;
        private const float ViewPortBottom = 10;
        private const float ViewPortNear = 100f;
        private const float ViewPortFar = -100f;

        private float _x = ViewPortLeft;
        private float _dx = 0.05f;
        
        private Quaternion qStart = Quaternion.CreateFromAxisAngle(new Vector3(1,0,0), Mathf.ToRadian(10));
        private Quaternion qEnd = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathf.ToRadian(30));

        private float _t = 0;
        private float _dt = 0.1f;

        private readonly CameraHelper _cameraHelper = new CameraHelper(10, 40, 1);
        private ShaderHelper _shader;

        private Quader _quad = new Quader();

        public MainWindow()
        {
            InitializeComponent();

            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += _cameraHelper.OnKeyDown;
            //KeyDown += OnKeyDown;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _shader = OpenGlWpfControl.Shader;
            _shader.ShadingLevel = 1;
            _shader.LightPos = new Vector3(2,10,2);
            _shader.Ambien = new Vector3(0.5f,0.5f,0.5f);
            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.PolygonOffsetFill);
            Gl.PolygonOffset(1, 1);
            Gl.DepthMask(true);
        }

        private void Render(object sender, GlControlEventArgs e)
        {
            if (_shader == null) return;
            Gl.Clear(ClearBufferMask.DepthBufferBit);

            var v = _cameraHelper.CameraMatrix;

            _shader.M = v;

            _shader.ShadingLevel = 0;
            FiguresHelper.Draw3DCross(10, 1);
            _shader.ShadingLevel = 1;

            var qIntern = Quaternion.Slerp(qStart, qEnd, _t);
            _t += _dt;

            var m = Matrix4x4.CreateFromQuaternion(qIntern) * Matrix4x4.CreateTranslation(_x,0,0) * v;
            _shader.M = m;

            _quad.Draw(0.8f, 0.5f, 1.6f, true);
                       
            _x += _dx;
        }

    }
}

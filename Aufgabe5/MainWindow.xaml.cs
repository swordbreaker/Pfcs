using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using WpfOpenGlLibrary.Models;

namespace Aufgabe5
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
        private const float ViewPortNear = -100f;
        private const float ViewPortFar = 100f;

        private ShaderHelper _shader;
        private CameraHelper _cameraHelper = new CameraHelper();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            KeyDown += _cameraHelper.OnKeyDown;
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
            _shader.M = Matrix4x4.Identity;
            _shader.P = _cameraHelper.CameraMatrix;

            Quader.Draw(1, 1, 1, true);
        }
    }
}

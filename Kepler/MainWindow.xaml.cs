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
using Aufgabe2;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = OpenGL.Matrix4x4;

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
        private const float ViewPortNear = -10;
        private const float ViewPortFar = 10;

        private float _elevation = 0;
        private float _azimut = 0;

        private const float G = 9.81e-6f;
        private const float RE = 6.378f;
        private const float GM = G * RE * RE;

        private Projectile _satelite;

        public MainWindow()
        {
            InitializeComponent();

            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            //KeyDown += OnKeyDown;

            _satelite = new Projectile(new Vector2(0,0.002f), new Vector2(42,0), DrawCircle);

            Gl.Enable(EnableCap.DepthTest);
        }

        private void Render(object sender, GlControlEventArgs e)
        {
            Gl.Clear(ClearBufferMask.DepthBufferBit);

            FiguresHelper.DrawCircle(RE, new Vector2(0,0), 20, Colors.Yellow);

            _satelite.Draw();
        }

        private static void DrawCircle(Vector2 pos, Vector2 v, Vector2 a, Color color)
        {
            FiguresHelper.DrawCircle(0.1f*RE, pos, 20, color);
        }
    }
}

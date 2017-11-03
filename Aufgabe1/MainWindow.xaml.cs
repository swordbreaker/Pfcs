using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;

namespace Aufgabe1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public float A1 { get; set; } = 2f;
        public float A2 { get; set; } = 2f;
        public float W1 { get; set; } = 1f;
        public float W2 { get; set; } = 1f;


        private float _phi = 0.1f;

        private uint _programmId;

        public MainWindow()
        {
            InitializeComponent();
            OpenGlWpfControl.Ortho = new WpfOpenGlLibrary.OpenGlWpfControl.OrthoProjection(3,-3,3,-3,-100,100);
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
        }

        private void Render(object sender, GlControlEventArgs glControlEventArgs)
        {
            var center = Vector2.Zero;
            DrawCircle(center, A1, A2, W1, W2, _phi);
            _phi = (_phi + 0.1f) % 360;
        }

        private void DrawCircle(Vector2 center, float a1, float a2, float w1, float w2, float phi)
        {
            var vertHelper = new VertexHelper {CurrentColor = Colors.LightGreen};
            var n = 400;

            var dt = (float)(2 * Math.PI) / (w1 * (n - 1));
            float t = 0;

            for (int i = 0; i < n; i++, t += dt)
            {
                var x = (float)Math.Sin(w1 * t) * a1 + center.X;
                var y = (float)Math.Cos(w2 * t - phi) * a2 + center.Y;
                vertHelper.Put(x,y);
            }

            vertHelper.Draw(PrimitiveType.LineLoop);
            Gl.LineWidth(6);
        }
    }
}

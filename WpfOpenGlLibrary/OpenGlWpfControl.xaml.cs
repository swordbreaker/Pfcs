using System;
using System.Windows;
using System.Windows.Controls;
using OpenGL;

namespace WpfOpenGlLibrary
{
    /// <summary>
    /// Interaction logic for OpenGlWpfControl.xaml
    /// </summary>
    public partial class OpenGlWpfControl : UserControl
    {
        public struct OrthoProjection
        {
            public readonly double Left;
            public readonly double Right;
            public readonly double Top;
            public readonly double Bottom;
            public readonly double Near;
            public readonly double Far;

            public OrthoProjection(double left, double right, double top, double bottom, double near, double far)
            {
                this.Left = left;
                this.Right = right;
                this.Top = top;
                this.Bottom = bottom;
                this.Near = near;
                this.Far = far;
            }
        }

        public DependencyProperty OrthoProperty = DependencyProperty.Register(
            nameof(Ortho), typeof(OrthoProjection), typeof(OpenGlWpfControl));

        public OrthoProjection Ortho { get; set; } = new OrthoProjection(-1, 1, -1, 1, -1, 1);

        public OpenGL.GlControl GlControl { get; private set; }

        #region Events

        public event EventHandler<GlControlEventArgs> OnRender;

        #endregion

        public OpenGlWpfControl()
        {
            InitializeComponent();
            GlControl = OpenGlControl;
        }

        private void GlControl_OnContextCreated(object sender, GlControlEventArgs e)
        {
            var control = (GlControl)sender;
            AdjustOrtho(new Size(control.Height, control.Width));

            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.LoadIdentity();

            Gl.ClearColor(1, 1, 1, 1);
        }

        private void GlControl_OnRender(object sender, GlControlEventArgs e)
        {
            var control = sender as GlControl;

            var vpx = 0;
            var vpy = 0;
            var vpw = control.ClientSize.Width;
            var vph = control.ClientSize.Height;

            Gl.Viewport(vpx, vpy, vpw, vph);

            Gl.Clear(ClearBufferMask.ColorBufferBit);

            OnRender?.Invoke(sender, e);
        }

        private void Control_OnResize(object sender, EventArgs e)
        {
            var control = (GlControl)sender;
            AdjustOrtho(new Size(control.Width, control.Height));
        }

        private void AdjustOrtho(Size size)
        {
            var aspect = (float)size.Height / size.Width;

            Gl.MatrixMode(MatrixMode.Projection);
            Gl.LoadIdentity();
            Gl.Ortho(Ortho.Left, Ortho.Right, Ortho.Bottom * aspect, Ortho.Top * aspect, Ortho.Near, Ortho.Far);
        }
    }
}

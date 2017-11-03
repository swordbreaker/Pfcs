using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;

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

        //public DependencyProperty OrthoProperty = DependencyProperty.Register(
        //    nameof(Ortho), typeof(OrthoProjection), typeof(OpenGlWpfControl));

        public Color BgColor
        {
            get { return _bgColor; }
            set
            {
                _bgColor = value;
                _bgColorVec = new Vector4(value.R/255f, value.G/255f, value.B/255f, value.A/255f);
            }
        }

        private Vector4 _bgColorVec = new Vector4(0f,0f,0f,0f);
        private Color _bgColor;

        public OrthoProjection Ortho { get; set; } = new OrthoProjection(-1, 1, -1, 1, -1, 1);

        public GlControl GlControl { get; private set; }

        public ShaderHelper Shader { get; private set; }

        #region Events

        public event EventHandler<GlControlEventArgs> OnRender;
        public event EventHandler<Vector2> OnMouseClick;

        #endregion

        public OpenGlWpfControl()
        {
            BgColor = Colors.White;
            InitializeComponent();
            GlControl = OpenGlControl;
            GlControl.DepthBits = 24;
        }

        private void GlControl_OnContextCreated(object sender, GlControlEventArgs e)
        {
            var control = (GlControl)sender;

            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.LoadIdentity();

            Gl.ClearColor(_bgColorVec.X, _bgColorVec.Y, _bgColorVec.Z, _bgColorVec.W);

            Shader = new ShaderHelper();
            AdjustOrtho(new Size(control.Height, control.Width));
        }

        private void GlControl_OnRender(object sender, GlControlEventArgs e)
        {
            var control = sender as GlControl;

            var vpx = 0;
            var vpy = 0;
            var vpw = control.ClientSize.Width;
            var vph = control.ClientSize.Height;

            Gl.Viewport(vpx, vpy, vpw, vph);

            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Gl.ClearColor(_bgColorVec.X, _bgColorVec.Y, _bgColorVec.Z, _bgColorVec.W);

            OnRender?.Invoke(sender, e);
        }

        private void Control_OnResize(object sender, EventArgs e)
        {
            var control = (GlControl)sender;

            AdjustOrtho(new Size(control.Width, control.Height));
        }

        private void AdjustOrtho(Size size)
        {
            if (Shader == null) return;

            var aspect = (float)size.Height / size.Width;

            //var p = Matrix4x4.Identity;

            Gl.MatrixMode(MatrixMode.Projection);
            Gl.LoadIdentity();

            //var m = Matrix4x4.CreatePerspectiveOffCenter((float)Ortho.Left, (float)Ortho.Right, (float) Ortho.Bottom, (float) Ortho.Top,
            //    (float) Ortho.Near, (float) Ortho.Far);
            //Gl.LoadMatrix(m.ToArray());

            //Gl.Ortho(Ortho.Left, Ortho.Right, Ortho.Bottom * aspect, Ortho.Top * aspect, Ortho.Near, Ortho.Far);

            Shader.P = Matrix4x4.CreateOrthographicOffCenter((float) Ortho.Left, (float) Ortho.Right,
                (float) Ortho.Bottom * (float)aspect, (float) Ortho.Top * (float)aspect, (float) Ortho.Near,
                (float) Ortho.Far);

            //var p = Matrix4x4.CreatePerspective((float)Math.Abs(Ortho.Left - Ortho.Right), (float)Math.Abs(Ortho.Top - Ortho.Bottom),
            //    (float)Ortho.Near, (float)Ortho.Far);

            //Gl.LoadMatrix(p.ToArray());

            //gluPerspective(65.0, (float)g_Width / g_Height, g_nearPlane, g_farPlane);
            //var ortho = Matrix4x4.CreateOrthographicOffCenter(
            //    (float) Ortho.Left,
            //    (float) Ortho.Right,
            //    (float) (Ortho.Bottom * aspect),
            //    (float) (Ortho.Top * aspect),
            //    (float) Ortho.Near,
            //    (float) Ortho.Far);

            //Shader.P = Matrix4x4.Identity * ortho;
        }

        private void OpenGlControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var control = (GlControl) sender;
            var relativePoint = control.PointToClient(e.Location);
            OnMouseClick?.Invoke(sender, new Vector2(e.X, e.Y));
        }
    }
}

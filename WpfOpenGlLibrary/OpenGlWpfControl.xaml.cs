﻿using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
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

        public DependencyProperty OrthoProperty = DependencyProperty.Register(
            nameof(Ortho), typeof(OrthoProjection), typeof(OpenGlWpfControl));

        public OrthoProjection Ortho { get; set; } = new OrthoProjection(-1, 1, -1, 1, -1, 1);

        public OpenGL.GlControl GlControl { get; private set; }

        public ShaderHelper Shader { get; private set; }

        #region Events

        public event EventHandler<GlControlEventArgs> OnRender;
        public event EventHandler<Vector2> OnMouseClick;

        #endregion

        public OpenGlWpfControl()
        {
            InitializeComponent();
            GlControl = OpenGlControl;
        }

        private void GlControl_OnContextCreated(object sender, GlControlEventArgs e)
        {
            var control = (GlControl)sender;

            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.LoadIdentity();

            Gl.ClearColor(1, 1, 1, 1);

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

            var p = Matrix4x4.Identity;

            //Gl.MatrixMode(MatrixMode.Projection);
            //Gl.LoadIdentity();
            //Gl.Ortho(Ortho.Left, Ortho.Right, Ortho.Bottom * aspect, Ortho.Top * aspect, Ortho.Near, Ortho.Far);

            var ortho = Matrix4x4.CreateOrthographicOffCenter(
                (float) Ortho.Left,
                (float) Ortho.Right,
                (float) (Ortho.Bottom * aspect),
                (float) (Ortho.Top * aspect),
                (float) Ortho.Near,
                (float) Ortho.Far);

            Shader.P = Matrix4x4.Identity * ortho;
        }

        private void OpenGlControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var control = (GlControl) sender;
            var relativePoint = control.PointToClient(e.Location);
            OnMouseClick?.Invoke(sender, new Vector2(e.X, e.Y));
        }
    }
}
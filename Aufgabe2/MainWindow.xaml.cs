using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Aufgabe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const float G                 = 9.81f;                 //Gravity
        private const float M                = 1;                     //Mass

        private static Vector2 V0            = new Vector2(10f, 4f);  //Initial Velocity Vector
        private static readonly Vector2 Pos0 = new Vector2(-8f, 0);   //Start position for bullets
        private float _vStrenght             = V0.Length();
        private Vector2 _v0Norm              = Vector2.Normalize(V0);
        private float _alpha;                                         //Angle in radians

        private readonly Queue<Projectile> _bullets = new Queue<Projectile>();
        private readonly Projectile _plate;

        private const float ViewPortLeft = -10;
        private const float ViewPortRight = 10;
        private const float ViewPortTop = 10;
        private const float ViewPortBottom = -10;
        private const float ViewPortNear = -100;
        private const float ViewPortFar = 100;


        public MainWindow()
        {
            InitializeComponent();
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.OnMouseClick += MouseClick;
            //OpenGlWpfControl.Loaded += (sender, args) => _shaderHelper = new ShaderHelper();
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);
            KeyDown += OnKeyDown;

            _alpha = (float)Math.Acos(Vector2.Dot(_v0Norm, Vector2.UnitX));
            _plate = new Projectile(Vector2.Zero, Pos0 + V0 * 1f, DrawSquare) { UseGravity = false };

            //_shaderHelper = new ShaderHelper();
        }

        //private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    _shaderHelper = new ShaderHelper();
        //}

        private void MouseClick(object sender, Vector2 p)
        {
            var control = OpenGlWpfControl;
            //var pos = control.PointFromScreen(new Point(e.X, -e.Y)).ToVector2();
            //var pos = e.GetPosition(control).ToVector2();
            var pos = new Vector2(p.X, -p.Y);

            var size = new Size(control.ActualWidth, control.ActualHeight);

            //Move pos to center
            pos -= new Vector2((float)size.Width / 2, (float)size.Height / 2);

            //scale pos to fit viewport
            var scaleX = Math.Abs(ViewPortRight - ViewPortLeft) / (float)size.Width;
            var scaleY = Math.Abs(ViewPortBottom - ViewPortTop) / (float)size.Height;

            pos *= new Vector2(scaleX, scaleY);

            V0 = pos - Pos0;
        }

        private static void DrawSquare(Vector2 pos, Color color)
        {
            FiguresHelper.DrawRectangle(new Rect(pos.X - 0.25, pos.Y - 1, 0.5, 2), color);
        }

        private static void DrawCircle(Vector2 pos, Color color)
        {
            FiguresHelper.DrawCircle(0.5f, pos, 20, color);
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.S:
                    _bullets.Enqueue(new Projectile(V0, Pos0, DrawCircle));
                    _plate.UseGravity = true;
                    break;
                case Key.V:
                    _vStrenght = (_vStrenght + 0.1f) % 15;
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.Up:
                    _alpha = (_alpha + (float)(5*Math.PI/180)) % 90;
                    _v0Norm = new Vector2((float)Math.Cos(_alpha), (float)Math.Sin(_alpha));
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.Down:
                    _alpha = (_alpha - (float)(5 * Math.PI / 180)) % 90;
                    _v0Norm = new Vector2((float)Math.Cos(_alpha), (float)Math.Sin(_alpha));
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.R:
                    Reset();
                    break;
            }
        }

        private void Render(object sender, GlControlEventArgs glControlEventArgs)
        {

            var start = new Vector2(1, 2);
            Matrix4x4 m = Matrix4x4.Identity;
            m *= Matrix4x4.CreateTranslation(new Vector3(start.X, start.Y, 0));
            m *= Matrix4x4.CreateRotationZ(30f * 180f / (float)Math.PI);

            OpenGlWpfControl.Shader.M = m;
            FiguresHelper.DrawSpear(new Vector2(1, 2), 1.2f, 0.04f, 0.2f);
            OpenGlWpfControl.Shader.M = Matrix4x4.Identity;

            if (_bullets.Count > 0)
            {
                foreach (var bullet in _bullets)
                {
                    bullet.Draw();
                }

                if (_bullets.Peek().Pos.Y <= ViewPortLeft) _bullets.Dequeue();
            }

            _plate.Draw();
            FiguresHelper.DrawCircle(0.2f, Pos0, 20, Colors.Green);
            FiguresHelper.DrawLine(Pos0, Pos0 + V0, 1, Colors.Green);
        }

        private void Reset()
        {
            _plate.UseGravity = false;
            _plate.Reset();
        }

        private void OpenGlWpfControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var control = (OpenGlWpfControl)sender;
            var pos = e.GetPosition(control).ToVector2();

            var size = new Size(control.ActualWidth, control.ActualHeight);

            //Move pos to center
            pos -= new Vector2((float)size.Width / 2, (float)size.Height / 2);

            //scale pos to fit viewport
            var scaleX = Math.Abs(ViewPortRight - ViewPortLeft) / (float)size.Width;
            var scaleY = Math.Abs(ViewPortBottom - ViewPortTop) / (float)size.Height;

            pos *= new Vector2(scaleX, scaleY);

            V0 = pos - Pos0;
        }
    }
}

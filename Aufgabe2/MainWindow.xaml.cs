using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Aufgabe2.Annotations;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Aufgabe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields
        public const float G = 9.81f; //Gravity
        private const float M = 1;    //Mass
        private bool _isRunning = true;

        private Vector2 _v0 = new Vector2(10f, 4f);
        private float _alpha;

        private static readonly Vector2 Pos0 = new Vector2(-8f, 0);   //Start position for bullets
        private float _vStrenght;
        private Vector2 _v0Norm;

        private readonly Queue<Projectile> _bullets = new Queue<Projectile>();
        private readonly Projectile _plate;

        private const float ViewPortLeft = -10;
        private const float ViewPortRight = 10;
        private const float ViewPortTop = 10;
        private const float ViewPortBottom = -10;
        private const float ViewPortNear = -100;
        private const float ViewPortFar = 100;

        private Projectile.DrawDelegate _projectilDrawDelegate = DrawCircle;
        #endregion

        #region Properties
        public Vector2 V0 //Initial Velocity Vector
        {
            get => _v0;
            private set
            {
                _v0 = value;
                OnPropertyChanged();
            }
        }

        public float Alpha //Angle in radians
        {
            get => _alpha;
            private set
            {
                _alpha = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AlphaDeg));
            }
        }

        public float AlphaDeg => Alpha * 180/(float)Math.PI;
        #endregion

        public MainWindow()
        {
            _vStrenght = V0.Length();
            _v0Norm = Vector2.Normalize(V0);

            InitializeComponent();
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);
            KeyDown += OnKeyDown;

            Alpha = (float)Math.Acos(Vector2.Dot(_v0Norm, Vector2.UnitX));
            _plate = new Projectile(Vector2.Zero, Pos0 + V0 * 1f, DrawSquare) { UseGravity = false };
        }

        private static void DrawSquare(Vector2 pos, Vector2 v, Vector2 a, Color color)
        {
            FiguresHelper.DrawRectangle(new Rect(pos.X - 0.25, pos.Y - 1, 0.5, 2), color);
        }

        private static void DrawCircle(Vector2 pos, Vector2 v, Vector2 a, Color color)
        {
            FiguresHelper.DrawCircle(0.5f, pos, 20, color);
        }

        private static void DrawSpear(Vector2 pos, Vector2 v, Vector2 a, Color c)
        {
            Gl.MatrixMode(MatrixMode.Modelview);
            Gl.LoadIdentity();
            Gl.Translate(pos.X, pos.Y, 0);

            float alpha = (float)Math.Atan(v.Y/v.X) * 180f / (float)Math.PI;
            Gl.Rotate(alpha, 0, 0, 1);

            FiguresHelper.DrawSpear(new Vector2(1, 2), 1.2f, 0.04f, 0.2f);
            Gl.LoadIdentity();
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.S:
                    if (!_isRunning) break;
                    _bullets.Enqueue(new Projectile(V0, Pos0, _projectilDrawDelegate));
                    _plate.UseGravity = true;
                    break;
                case Key.Space:
                    _isRunning = !_isRunning;

                    foreach (var projectile in _bullets)
                    {
                        projectile.UseGravity = _isRunning;
                    }

                    _plate.UseGravity = _isRunning;
                    break;
                case Key.V:
                    _vStrenght = (_vStrenght + 0.1f) % 15;
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.Up:
                    Alpha = (Alpha + (float)(5*Math.PI/180)) % (float)(Math.PI * 2);
                    _v0Norm = new Vector2((float)Math.Cos(Alpha), (float)Math.Sin(Alpha));
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.Down:
                    Alpha = (Alpha - (float)(5 * Math.PI / 180)) % (float)(Math.PI*2);
                    _v0Norm = new Vector2((float)Math.Cos(Alpha), (float)Math.Sin(Alpha));
                    V0 = _v0Norm * _vStrenght;
                    break;
                case Key.R:
                    Reset();
                    break;
            }
        }

        private void Render(object sender, GlControlEventArgs glControlEventArgs)
        {
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

        private void ToggleButtonBullet_OnChecked(object sender, RoutedEventArgs e)
        {
            _projectilDrawDelegate = DrawCircle;
        }

        private void ToggleButtonSpear_OnChecked(object sender, RoutedEventArgs e)
        {
            _projectilDrawDelegate = DrawSpear;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

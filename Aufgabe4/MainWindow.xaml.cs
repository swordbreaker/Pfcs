using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Aufgabe4.Annotations;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Aufgabe4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const float ViewPortLeft = -10;
        private const float ViewPortRight = 10;
        private const float ViewPortTop = 10;
        private const float ViewPortBottom = -10;
        private const float ViewPortNear = -100f;
        private const float ViewPortFar = 100f;

        public float Radius
        {
            get => _cylinderFlow.Radius;
            set => SetField(ref _cylinderFlow.Radius, value);
        }

        public float X
        {
            get => _pos.X;
            set => SetField(ref _pos, new Vector2(value, _pos.Y));
        }

        public float Y
        {
            get => _pos.Y;
            set => SetField(ref _pos, new Vector2(_pos.X, value));
        }

        public float W
        {
            get => _cylinderFlow.W;
            set
            {
                SetField(ref _cylinderFlow.W, value);
                _deltaPhi = _cylinderFlow.W * _animationTime;
            }
            
        }

        public float Speed
        {
            get => _speed;
            set => SetField(ref _speed, value);
        }

        private Vector2 _pos = Vector2.Zero;

        private ShaderHelper _shader;
        private readonly CylinderFlow _cylinderFlow = new CylinderFlow(2f);
        private float _startX;
        private readonly float _animationTime; //in seconds
        private float _speed = 0.1f;
        private float _phi;
        private float _deltaPhi;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            OpenGlWpfControl.BgColor = Colors.RoyalBlue;
            OpenGlWpfControl.OnRender += Render;
            OpenGlWpfControl.Ortho = new OpenGlWpfControl.OrthoProjection(ViewPortLeft, ViewPortRight, ViewPortTop, ViewPortBottom, ViewPortNear, ViewPortFar);

            _animationTime = OpenGlWpfControl.GlControl.AnimationTime / 1000f;
            KeyDown += OnKeyDown;
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

            var m = Matrix4x4.CreateTranslation(new Vector3(_pos, 0));
            _cylinderFlow.M = Matrix4x4.CreateScale(-1,-1,1) * m;

            Gl.LineWidth(1);

            var steps = 10;
            var dt = 0.02f;

            for (int i = -20; i < 20; i++)
            {
                var y = i / 2f;
                if (Math.Abs(y - _pos.Y) < 0.01f) y += 0.01f;
                var drawer = _cylinderFlow.GetLineDrawer(new Vector2(-15, y));
                drawer.Skip(_startX);
                for (int j = 0; j < 150; j++)
                {
                    drawer.DrawLine(dt, steps);
                    drawer.Skip(steps * dt);
                }
            }

            _startX = (_startX + _speed) % ((steps * dt * 2));

            _shader.M = Matrix4x4.CreateRotationZ(_phi) * m;
            FiguresHelper.DrawCircle(_cylinderFlow.Radius, 20, new [] {Colors.Orange, Colors.Blue, Colors.Green});

            _phi += _deltaPhi;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    W -= 0.1f;
                    break;
                case Key.E:
                    W += 0.1f;
                    break;
                case Key.W:
                    Y += 0.1f;
                    break;
                case Key.S:
                    Y -= 0.1f;
                    break;
                case Key.A:
                    X -= 0.1f;
                    break;
                case Key.D:
                    X += 0.1f;
                    break;
                case Key.R:
                    X = 0;
                    Y = 0;
                    W = 0;
                    Radius = 2f;
                    break;
            }
        }

        //  -------  Klothoide (Cornu'sche Spirale) ------------
        public void DrawLain(float xStart, float yStart, float zStart,
            float phi, float ds, float kruemmung,
            float dKruemmung, int nPunkte)
        {
            VertexHelper.Clear();
            float x = xStart, y = yStart, z = zStart;

            VertexHelper.Put(x,y,z);
            for (int i = 1; i < nPunkte; i++)
            {
                x += (float)Math.Cos(phi) * ds;
                y += (float)Math.Sin(phi) * ds;
                VertexHelper.Put(x,y,z);
                phi += kruemmung * ds;
                kruemmung += dKruemmung * ds;
            }

            VertexHelper.Draw(PrimitiveType.LineStrip);
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

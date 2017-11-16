using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary;
using WpfOpenGlLibrary.Helpers;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Spiralbahn
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

        private Vector2 _pos = Vector2.Zero;

        private ShaderHelper _shader;
        private readonly SpiralDynamics _spiralDynamics = new SpiralDynamics();
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

            Gl.LineWidth(1);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
        }


        protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

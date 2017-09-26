using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Helpers;

namespace PfcsSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public float A1 { get; set; } = 0.5f;
        public float A2 { get; set; } = 0.5f;
        public float W1 { get; set; } = 1f;
        public float W2 { get; set; } = 1f;


        private float _phi = 0.1f;

        private uint _programmId;

        public MainWindow()
        {
            InitializeComponent();
            OpenGlWpfControl.Ortho = new WpfOpenGlLibrary.OpenGlWpfControl.OrthoProjection(3,-3,3,-3,-100,100);
            OpenGlWpfControl.OnRender += Render;
        }

        private void Render(object sender, GlControlEventArgs glControlEventArgs)
        {
            var center = Vector2.Zero;
            DrawCircle(center, A1, A2, W1, W2, _phi);
            _phi = (_phi + 0.1f) % 360;
        }

        //private void GlControl_OnContextCreated(object sender, GlControlEventArgs e)
        //{
        //    Gl.MatrixMode(MatrixMode.Projection);
        //    Gl.LoadIdentity();
        //    Gl.Ortho(-1, 1f, -1, 1, -1f, 1.0);

        //    Gl.MatrixMode(MatrixMode.Modelview);
        //    Gl.LoadIdentity();

        //    Gl.ClearColor(1,1,1,1);

        //    //var shader = Gl.CreateShader(ShaderType.VertexShader);
        //    //Gl.ShaderSource(shader, File.ReadAllLines("Shaders/vshader0.glsl"));
        //    //Gl.CompileShader(shader);

        //    //var program = Gl.CreateProgram();
        //    //Gl.AttachShader(program, shader);
        //    //Gl.LinkProgram(program);
        //    //Gl.UseProgram(program);

        //    //_programmId = program;
        //}

        //private void GlControl_OnRender(object sender, GlControlEventArgs e)
        //{
        //    var senderControl = sender as GlControl;

        //    int vpx = 0;
        //    int vpy = 0;
        //    int vpw = senderControl.ClientSize.Width;
        //    int vph = senderControl.ClientSize.Height;

        //    Gl.Viewport(vpx, vpy, vpw, vph);
            
        //    Gl.Clear(ClearBufferMask.ColorBufferBit);

        //    // Old school OpenGL 1.1
        //    // Setup & enable client states to specify vertex arrays, and use Gl.DrawArrays instead of Gl.Begin/End paradigm
        //    using (var vertexArrayLock = new MemoryLock(_ArrayPosition))
        //    using (var vertexColorLock = new MemoryLock(_colors))
        //    {
        //        // Note: the use of MemoryLock objects is necessary to pin vertex arrays since they can be reallocated by GC
        //        // at any time between the Gl.VertexPointer execution and the Gl.DrawArrays execution

        //        //Gl.ColorPointer(3, ColorPointerType.Float, 0, vertexColorLock.Address);
        //        //Gl.EnableClientState(EnableCap.ColorArray);

        //        //Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);

        //        //Gl.EnableClientState(EnableCap.VertexArray);
        //        //Gl.LineWidth(5);

        //        //Gl.DrawArrays(PrimitiveType.LineLoop, 0, 3);

        //        //var center = new Vector2((float)senderControl.ClientSize.Width/2, (float)senderControl.ClientSize.Height/2);
        //        var center = Vector2.Zero;

                

        //        DrawCircle(center, A1, A2, W1, W2, _phi);
        //        _phi = (_phi + 0.1f) % 360;
        //    }
        //}

        private void DrawCircle(Vector2 center, float a1, float a2, float w1, float w2, float phi)
        {
            var vertHelper = new VertexHelper {CurrentColor = Colors.Green};
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

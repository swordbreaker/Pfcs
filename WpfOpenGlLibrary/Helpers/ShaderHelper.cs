using System.IO;
using OpenGL;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace WpfOpenGlLibrary.Helpers
{
    public class ShaderHelper
    {
        private readonly int _mId;
        private readonly int _pId;
        private Matrix4x4 _m = Matrix4x4.Identity;
        private Matrix4x4 _p = Matrix4x4.Identity;

        public Matrix4x4 M
        {
            get => _m;
            set
            {
                _m = value;
                Gl.UniformMatrix4(_mId, 1, false, _m.ToArray());
            }
        }

        public Matrix4x4 P
        {
            get => _p;
            set
            {
                _p = value;
                Gl.UniformMatrix4(_pId, 1, false, _p.ToArray());
            }
        }

        public ShaderHelper()
        {
            uint vShaderId = Gl.CreateShader(ShaderType.VertexShader);
            uint fShaderId = Gl.CreateShader(ShaderType.FragmentShader);

            var vShader = File.ReadAllText("Shaders/vshader1.glsl");
            var fShader = File.ReadAllText("Shaders/fshader0.glsl");

            Gl.ShaderSource(vShaderId, new [] { vShader });
            Gl.CompileShader(vShaderId);

            Gl.ShaderSource(fShaderId, new[] { fShader });
            Gl.CompileShader(fShaderId);
                
            var programId = Gl.CreateProgram();
            Gl.AttachShader(programId, vShaderId);
            Gl.AttachShader(programId, fShaderId);

            Gl.LinkProgram(programId);
            Gl.UseProgram(programId);

            _mId = Gl.GetUniformLocation(programId, "M");
            _pId = Gl.GetUniformLocation(programId, "P");

            Gl.UniformMatrix4(_mId, 1, false, _m.ToArray());
            Gl.UniformMatrix4(_pId, 1, false, _p.ToArray());
        }
    }
}

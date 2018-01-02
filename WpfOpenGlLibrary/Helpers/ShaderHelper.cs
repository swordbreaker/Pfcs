using System.Collections.Generic;
using System.IO;
using System.Numerics;
using OpenGL;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace WpfOpenGlLibrary.Helpers
{
    public class ShaderHelper
    {
        private readonly int _mId;
        private readonly int _pId;
        private readonly int _lId;
        private readonly int _shadingLevelId;
        private readonly int _ambientId;
        private readonly int _diffuseId;

        private Matrix4x4 _m = Matrix4x4.Identity;
        private Matrix4x4 _p = Matrix4x4.Identity;
        private Vector3 _l = new Vector3(0,0,10);
        private int _shadingLevel = 0;
        private float _diffuse = 0.8f;

        private Stack<Matrix4x4> _mStack = new Stack<Matrix4x4>();
        private Vector3 _ambient = new Vector3(0.2f,0.2f,0.2f);

        public Matrix4x4 M
        {
            get => _m;
            set
            {
                _m = value;
                Gl.UniformMatrix4(_mId, false, _m.ToArray());
            }
        }

        public Matrix4x4 P
        {
            get => _p;
            set
            {
                _p = value;
                Gl.UniformMatrix4(_pId, false, _p.ToArray());
            }
        }

        public Vector3 LightPos
        {
            get => _l;
            set
            {
                _l = value;
                var v = new Vector4(value, 1);
                //TODO outsource to shader
                v = Vector4.Transform(v, M);
                Gl.Uniform4(_lId, v.X, v.Y, v.Z, v.W);
            }
        }

        public Vector3 Ambien
        {
            get => _ambient;
            set
            {
                _ambient = value;
                Gl.Uniform3(_ambientId, _ambient.X, _ambient.Y, _ambient.Z);
            }
        }

        public int ShadingLevel
        {
            get => _shadingLevel;
            set
            {
                _shadingLevel = value;
                Gl.Uniform1(_shadingLevelId, _shadingLevel);
            }
        }

        public ShaderHelper()
        {
            var programId = Gl.CreateProgram();
            var vShaderId = Gl.CreateShader(ShaderType.VertexShader);
            var fShaderId = Gl.CreateShader(ShaderType.FragmentShader);

            var vShader = File.ReadAllText("Shaders/vshader2.glsl");
            var fShader = File.ReadAllText("Shaders/fshader0.glsl");

            Gl.ShaderSource(vShaderId, new [] { vShader });
            Gl.CompileShader(vShaderId);

            Gl.ShaderSource(fShaderId, new[] { fShader });
            Gl.CompileShader(fShaderId);

            Gl.GetShader(vShaderId, ShaderParameterName.CompileStatus, out int success);

            Gl.CheckErrors();
            Gl.AttachShader(programId, vShaderId);
            Gl.CheckErrors();
            Gl.AttachShader(programId, fShaderId);
            Gl.CheckErrors();

            Gl.LinkProgram(programId);
            Gl.CheckErrors();
            Gl.ValidateProgram(programId);
            Gl.UseProgram(programId);

            Gl.CheckErrors();

            _mId = Gl.GetUniformLocation(programId, "M");
            _pId = Gl.GetUniformLocation(programId, "P");
            _lId = Gl.GetUniformLocation(programId, "lightPosition");
            _shadingLevelId = Gl.GetUniformLocation(programId, "shadingLevel");
            _ambientId = Gl.GetUniformLocation(programId, "ambient");
            _diffuseId = Gl.GetUniformLocation(programId, "diffuse");
            
            //// -----  set uniform variables  -----------------------
            //gl.glUniform1i(shadingLevelId, shadingLevel);
            //gl.glUniform1f(ambientId, ambient);
            //gl.glUniform1f(diffuseId, diffuse);
            //gl.glUniformMatrix4fv(lightPositionId, 1, false, lightPosition, 0);

            Gl.UniformMatrix4(_mId, false, _m.ToArray());
            Gl.UniformMatrix4(_pId, false, _p.ToArray());
            Gl.Uniform4(_lId, _l.X, _l.Y, _l.Z, 1);
            Gl.Uniform1(_shadingLevelId, _shadingLevel);
            Gl.Uniform3(_ambientId, _ambient.X, _ambient.Y, _ambient.Z);
            Gl.Uniform1(_diffuseId, _diffuse);
        }

        public void PushM(Matrix4x4 m)
        {
            _mStack.Push(M);
            M = m;
        }

        public void PopM()
        {
            M = _mStack.Pop();
        }
    }
}

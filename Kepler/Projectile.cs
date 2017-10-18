using System;
using System.Numerics;
using System.Windows.Media;
using Kepler;
using Microsoft.Win32;
using WpfOpenGlLibrary.Helpers;

namespace Aufgabe2
{
    public class Projectile
    {
        //protected float G { get; set; }

        private readonly Vector2 V0;
        public Vector2 V { get; private set; }
        //public Vector2 A { get; private set; }
        private readonly Vector2 _pos0;

        private const float Dt = 60f;

        private const float G = 9.81e-6f;
        private const float RE = 6.378f;
        private const float GM = G * RE * RE;


        public Vector2 Pos { get; private set; }

        public Color Color { get; set; } = Colors.Black;

        public delegate void DrawDelegate(Vector2 pos, Vector2 v, Vector2 a, Color color);
        private readonly DrawDelegate _drawAction;

        public bool UseGravity { get; set; } = true;

        public Projectile(Vector2 vo, Vector2 pos0, DrawDelegate drawAction)
        {
            V0 = vo;
            V = V0;
            _pos0 = pos0;
            Pos = _pos0;
            _drawAction = drawAction;
        }

        public void Draw()
        {
            _drawAction.Invoke(Pos, V, Vector2.Zero, Color);
            if (!UseGravity) return;

            var r = Pos.Length();
            var a = -(GM / (float) Math.Pow(r, 3)) * Pos;
            //var a = f / _m;
            Pos = Pos + V * Dt;
            V = V + a * Dt;
        }

        public void Reset()
        {
            Pos = _pos0;
            V = V0;
        }
    }
}

using System;
using System.Numerics;
using System.Windows.Media;
using Microsoft.Win32;
using WpfOpenGlLibrary.Helpers;

namespace Aufgabe2
{
    public class Projectile
    {
        private const float G = MainWindow.G;

        private readonly Vector2 V0;
        public Vector2 V { get; private set; }
        public Vector2 A { get; private set; } = new Vector2(0f, -G);
        private readonly Vector2 _pos0;

        private const float Dt = 0.01f;

        public Vector2 Pos { get; private set; }

        public Color Color { get; set; } = Colors.Black;

        private readonly Action<Vector2, Color> _drawAction;

        public bool UseGravity { get; set; } = true;

        public Projectile(Vector2 vo, Vector2 pos0, Action<Vector2, Color> drawAction)
        {
            V0 = vo;
            V = V0;
            _pos0 = pos0;
            Pos = _pos0;
            _drawAction = drawAction;
        }

        public void Draw()
        {
            _drawAction.Invoke(Pos, Color);
            if (!UseGravity) return;
            Pos = Pos + V * Dt;
            V = V + A * Dt;
        }

        public void Reset()
        {
            Pos = _pos0;
            V = V0;
        }
    }
}

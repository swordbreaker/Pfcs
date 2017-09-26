using System.Numerics;
using System.Windows;

namespace WpfOpenGlLibrary.Helpers
{
    public static class PointExtention
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }
    }
}
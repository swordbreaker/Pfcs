using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfOpenGlLibrary
{
    public static class PointExtention
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }
    }
}

using System;

namespace WpfOpenGlLibrary.Helpers
{
    public static class Mathf
    {
        public const float Epsilon = 0.0000001f;
        public static readonly float PI = 3.1415926f;

        private static readonly Random _random = new Random();

        public static float ToRadian(float v) => v / 180f * (float)Math.PI;
        public static float ToDeg(float v) => v / (float)Math.PI * 180f;

        public static float GaussDistribution(float omega, float mu)
        {
            float x = 0f;
            lock (_random)
            {
                x = (float)_random.NextDouble();
            }

            var a = Math.Pow((x - mu) / omega, 2);
            var b = Math.Pow(Math.E, -0.5f * a);
            var c = 1 / (omega * Math.Sqrt(2f * PI));

            return (float)(c * b);
        }

        public static float GetRandomRange(float min, float max)
        {
            lock (_random)
            {
                return (float)(min + _random.NextDouble() * max);
            }
        }
    }
}

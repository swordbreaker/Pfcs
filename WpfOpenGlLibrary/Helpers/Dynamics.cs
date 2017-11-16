namespace WpfOpenGlLibrary.Helpers
{
    public abstract class Dynamics
    {
        public float[] Euler(float[] x, float dt)
        {
            var y = F(x);

            var xx = new float[x.Length]; //x + y * dt
            for (int i = 0; i < xx.Length; i++)
            {
                xx[i] = x[i] + y[i] * dt;
            }

            return xx;
        }

        protected abstract float[] F(float[] xs);

        public float[] Runge(float[] x, float dt)
        {
            var y1 = F(x);

            var xx = new float[x.Length];

            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + (dt / 2) * y1[i];

            var y2 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + (dt / 2) * y2[i];

            var y3 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + dt * (y3[i]) / 6;

            var y4 = F(xx);
            for (int i = 0; i < xx.Length; i++)
                xx[i] = x[i] + dt * (y1[i] + 2 * y2[i] + 2 * y3[i] + y4[i]) / 6;

            return xx;
        }
    }
}

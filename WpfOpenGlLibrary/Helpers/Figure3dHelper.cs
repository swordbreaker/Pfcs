using System;
using System.Numerics;
using System.Windows.Media;
using OpenGL;
using WpfOpenGlLibrary.Models;

namespace WpfOpenGlLibrary.Helpers
{
    public static class Figure3DHelper
    {
        public static void DrawMesh(Mesh mesh, Color color)
        {
            var vh = new VertexHelper() { CurrentColor =  color};
            vh.PutMany(mesh.Verts, normals: mesh.Normals);
            vh.Draw(PrimitiveType.TriangleStrip);
        }

        public static void DrawSphere(float r, int n1, int n2, bool solid, Color color)
        {
            var x = new float[n1];                     // Halbkreis in xy-Ebene von Nord- zum Suedpol
            var y = new float[n1];
            var nx = new float[n1];                    // Normalenvektoren
            var ny = new float[n1];
            float dphi = (float)(Math.PI / (n1 - 1)), phi;
            for (int i = 0; i < n1; i++)
            {
                phi = (float)(0.5 * Math.PI) - i * dphi;
                x[i] = r * (float)Math.Cos(phi);
                y[i] = r * (float)Math.Sin(phi);
                nx[i] = x[i];
                ny[i] = y[i];
            }

            if (solid)
                DrawAreal(x, y, nx, ny, n2, color);
            else
                ZeichneRotGitternetz(x, y, nx, ny, n2, color);
        }

        public static void DrawAreal(          // Rotationsflaeche (Rotation um y-Achse)
            float[] x, float[] y,                // Kurve in xy-Ebene
            float[] nx, float[] ny,              // Normalenvektoren
            int n2, Color color)                              // Anzahl Drehungen um y-Achse
        {

            int n1 = x.Length;                             // Anzahl Breitenlinien
            var xa = new float[n1, n2];              // Vertex-Koordinaten
            var ya = new float[n1, n2];
            var za = new float[n1, n2];
            var nxa = new float[n1, n2];             // Normalen
            var nya = new float[n1, n2];
            var nza = new float[n1, n2];

            BerechnePunkte(x, y, nx, ny,
                xa, ya, za, nxa, nya, nza);

            // ------  Streifen zeichnen   ------
            int j2;

            var vertexHelper = new VertexHelper() { CurrentColor = color };

            //vb.rewindBuffer(gl);
            for (int j = 0; j < n2; j++)                     // n2 Streifen von Norden nach Sueden
                for (int i = 0; i < n1; i++)
                {
                    var normal = new Vector3(nxa[i, j], nya[i, j], nza[i, j]);
                    var vertex = new Vector3(xa[i, j], ya[i, j], za[i, j]);
                    vertexHelper.Put(vertex, normal: normal);
                    //vb.setNormal(nxa[i][j], nya[i][j], nza[i][j]);
                    //vb.putVertex(xa[i][j], ya[i][j], za[i][j]);
                    j2 = (j + 1) % n2;

                    normal = new Vector3(nxa[i, j2], nya[i, j2], nza[i, j2]);
                    vertex = new Vector3(nxa[i, j2], nya[i, j2], nza[i, j2]);
                    vertexHelper.Put(vertex, normal: normal);
                    //vb.setNormal(nxa[i][j2], nya[i][j2], nza[i][j2]);
                    //vb.putVertex(xa[i][j2], ya[i][j2], za[i][j2]);
                }

            vertexHelper.Draw(PrimitiveType.TriangleStrip);
            //vb.copyBuffer(gl);
            //int nVerticesStreifen = 2 * n1;                  // Anzahl Vertices eines Streifens
            //for (int j = 0; j < n2; j++)                     // die Streifen muessen einzeln gezeichnet werden
            //    gl.glDrawArrays(GL3.GL_TRIANGLE_STRIP, j * nVerticesStreifen, nVerticesStreifen);  // Streifen von Norden nach Sueden
        }

        // ----  n1 x n2 Punkte-Gitternetz einer Rotationsflaeche berechnen  ---------
        //
        //       Die gegebene Kurve in der xy-Ebene wird um die y-Achse gedreht

        private static void BerechnePunkte(float[] x, float[] y,          // Kurve in xy-Ebene
            float[] nx, float[] ny,        // Normalen in xy-Ebene
            float[,] xa, float[,] ya, float[,] za,     // Gitternetz
            float[,] nxa, float[,] nya, float[,] nza)  // gedrehte Normalen
        {
            int n1 = xa.GetLength(0);                            // Anzahl Zeilen
            int n2 = xa.GetLength(1);                         // Anzahl Spalten
            float todeg = (float)(180 / Math.PI);
            float dtheta = (float)(2 * Math.PI / n2);        // Drehwinkel
            float c = (float)Math.Cos(dtheta);             // Koeff. der Drehmatrix
            float s = (float)Math.Sin(dtheta);

            for (int i = 0; i < n1; i++)                     // erste Nord-Sued Linie
            {
                xa[i, 0] = x[i];
                ya[i, 0] = y[i];
                nxa[i, 0] = nx[i];
                nya[i, 0] = ny[i];
            }
            // ------  alle Vertices der Rotationsflaeche berechnen -----
            int j2;
            for (int j = 0; j < n2 - 1; j++)                     // n2-1 Nord-Sued Linien
                for (int i = 0; i < n1; i++)
                {
                    j2 = j + 1;
                    xa[i, j2] = c * xa[i, j] + s * za[i, j];         // Drehung um y-Achse
                    ya[i, j2] = ya[i, j];
                    za[i, j2] = -s * xa[i, j] + c * za[i, j];
                    nxa[i, j2] = c * nxa[i, j] + s * nza[i, j];      // gedrehter Normalenvektor
                    nya[i, j2] = nya[i, j];
                    nza[i, j2] = -s * nxa[i, j] + c * nza[i, j];
                }
        }

        public static void ZeichneRotGitternetz(         // Rotationsflaeche (Rotation um y-Achse)
            float[] x, float[] y,                // Kurve in xy-Ebene
            float[] nx, float[] ny,              // Normalenvektoren
            int n2, Color color)                              // Anzahl Drehungen um y-Achse
        {
            int n1 = x.Length;                            // Anzahl Breitenlinien
            var xa = new float[n1, n2];                    // Vertex-Koordinaten
            var ya = new float[n1, n2];
            var za = new float[n1, n2];
            var nxa = new float[n1, n2];                   // Normalen
            var nya = new float[n1, n2];
            var nza = new float[n1, n2];

            BerechnePunkte(x, y, nx, ny, xa, ya, za, nxa, nya, nza);


            var vertexHelper = new VertexHelper() { CurrentColor = color };
            for (int i = 0; i < n1; i++)                     // n1 Breitenlinien (Kresie um y-Achse)
                for (int j = 0; j < n2; j++)
                {
                    var normal = new Vector3(nxa[i, j], nya[i, j], nza[i, j]);
                    var vertex = new Vector3(nxa[i, j], nya[i, j], nza[i, j]);
                    vertexHelper.Put(vertex, normal: normal);
                }

            vertexHelper.Draw(PrimitiveType.LineLoop);
            //int nVerticesOffset = n2;                  // Anzahl Vertices einer Breitenlinie
            //for (int i = 0; i < n1; i++)                     // die Linien muessen einzeln gezeichnet werden
            //    gl.glDrawArrays(GL3.GL_LINE_LOOP, i * nVerticesOffset, n2);  // Breitenlinie

            vertexHelper = new VertexHelper() { CurrentColor = color };
            //vb.rewindBuffer(gl);
            for (int j = 0; j < n2; j++)                     // n2 Laengslinien
                for (int i = 0; i < n1; i++)
                {
                    var normal = new Vector3(nxa[i, j], nya[i, j], nza[i, j]);
                    var vertex = new Vector3(xa[i, j], ya[i, j], za[i, j]);
                    vertexHelper.Put(vertex, normal: normal);
                }

            vertexHelper.Draw(PrimitiveType.LineLoop);
            //vb.copyBuffer(gl);
            //nVerticesOffset = n1;                  // Anzahl Vertices einer Laengslinie
            //for (int j = 0; j < n2; j++)                     // die Linien muessen einzeln gezeichnet werden
            //    gl.glDrawArrays(GL3.GL_LINE_LOOP, j * nVerticesOffset, n1);  // Laengslinie

        }

    }
}

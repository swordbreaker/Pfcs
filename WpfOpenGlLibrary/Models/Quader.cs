using System.Numerics;
using OpenGL;
using WpfOpenGlLibrary.Helpers;

namespace WpfOpenGlLibrary.Models
{

    public class Quader
    {
        Vector3 e1 = new Vector3(1, 0, 0);               // Normalenvektoren
        Vector3 e2 = new Vector3(0, 1, 0);
        Vector3 e3 = new Vector3(0, 0, 1);
        Vector3 e1n = new Vector3(-1, 0, 0);             // negative Richtung
        Vector3 e2n = new Vector3(0, -1, 0);
        Vector3 e3n = new Vector3(0, 0, -1);

        public void Viereck(Vector3 A, Vector3 B, Vector3 C, Vector3 D, Vector3 n)      // Normale
        {
            VertexHelper.Put(A, normal: n);
            VertexHelper.Put(B, normal: n);
            VertexHelper.Put(C, normal: n);
            VertexHelper.Put(C, normal: n);
            VertexHelper.Put(D, normal: n);
            VertexHelper.Put(A, normal: n);
            //vb.setNormal(n.x, n.y, n.z);
            //vb.putVertex(A.x, A.y, A.z);          // Dreieck 1
            //vb.putVertex(B.x, B.y, B.z);
            //vb.putVertex(C.x, C.y, C.z);
            //vb.putVertex(C.x, C.y, C.z);          // Dreieck 2
            //vb.putVertex(D.x, D.y, D.z);
            //vb.putVertex(A.x, A.y, A.z);
        }


        public void kante(Vector3 a, Vector3 b)
        {
            VertexHelper.Put(a);
            VertexHelper.Put(b);

            //vb.putVertex(a.x, a.y, a.z);
            //vb.putVertex(b.x, b.y, b.z);
        }


        public void Draw(float a, float b, float c, bool gefuellt)
        {
            a *= 0.5f;
            b *= 0.5f;
            c *= 0.5f;
            var A = new Vector3(a, -b, c);           // Bodenpunkte
            var B = new Vector3(a, -b, -c);
            var C = new Vector3(-a, -b, -c);
            var D = new Vector3(-a, -b, c);
            var E = new Vector3(a, b, c);           // Deckpunkte
            var F = new Vector3(a, b, -c);
            var G = new Vector3(-a, b, -c);
            var H = new Vector3(-a, b, c);

            VertexHelper.Clear();
            if (gefuellt)
            {
                Viereck(D, C, B, A, e2);            // Boden
                Viereck(E, F, G, H, e2n);             // Deckflaeche
                Viereck(A, B, F, E, e1n);             // Seitenflaechen
                Viereck(B, C, G, F, e3);
                Viereck(D, H, G, C, e1);
                Viereck(A, E, H, D, e3n);

                VertexHelper.Draw(PrimitiveType.Triangles);
            }
            else
            {
                kante(A, B);                         // Boden
                kante(B, C);
                kante(C, D);
                kante(D, A);
                kante(E, F);                         // Decke
                kante(F, G);
                kante(G, H);
                kante(H, E);
                kante(A, E);                         // Kanten
                kante(B, F);
                kante(C, G);
                kante(D, H);

                VertexHelper.Draw(PrimitiveType.Lines);
            }
        }

    }
}

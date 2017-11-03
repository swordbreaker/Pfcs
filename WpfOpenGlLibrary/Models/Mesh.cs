using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace WpfOpenGlLibrary.Models
{
    public class Mesh
    {
        public Vector3[] Verts { get; private set; }
        public Vector3[] Normals { get; private set; }

        public Mesh(Vector3[] verts, Vector3[] normals, Face[] faces)
        {
            Init(verts, normals, faces);
        }

        public Mesh(string objFile)
        {
            var verts = new List<Vector3>();
            var norms = new List<Vector3>();
            var faces = new List<Face>();

            foreach (var line in File.ReadLines(objFile))
            {
                var components = line.Split(' ');
                if (components[0] == "v")
                {
                    verts.Add(ParseVector(components.Skip(1).ToArray()));
                }
                else if (components[0] == "vn")
                {
                    norms.Add(ParseVector(components.Skip(1).ToArray()));
                }
                else if (components[0] == "f")
                {
                    faces.Add(ParseFace(components.Skip(1).ToArray()));
                }
            }

            Init(verts.ToArray(), norms.ToArray(), faces.ToArray());
        }

        private void Init(Vector3[] verts, Vector3[] normals, Face[] faces)
        {
            Verts = new Vector3[faces.Length * 3];
            Normals = new Vector3[faces.Length * 3];

            var k = 0;
            for (var i = 0; i < faces.Length; i++,k += 3)
            {
                Verts[k + 0] = verts[faces[i].VertsId[0] - 1];
                Verts[k + 1] = verts[faces[i].VertsId[1] - 1];
                Verts[k + 2] = verts[faces[i].VertsId[2] - 1];

                Normals[k + 0] = normals[faces[i].NormsId[0] - 1];
                Normals[k + 1] = normals[faces[i].NormsId[1] - 1];
                Normals[k + 2] = normals[faces[i].NormsId[2] - 1];
            }
        }

        private Vector3 ParseVector(string[] comps)
        {
            return new Vector3(float.Parse(comps[0]), float.Parse(comps[1]), float.Parse(comps[2]));
        }

        private Face ParseFace(string[] comps)
        {
            var vIds = new int[3];
            var nIds = new int[3];

            for (var i = 0; i < comps.Length; i++)
            {
                var comp = comps[i];
                var c = comp.Split(new[] { "//" }, StringSplitOptions.None);
                vIds[i] = int.Parse(c[0]);
                nIds[i] = int.Parse(c[1]);
            }

            return new Face(vIds, nIds);
        }

        public struct Face
        {
            public readonly int[] VertsId;
            public readonly int[] NormsId;

            public Face(int[] vertsId, int[] normsId)
            {
                VertsId = vertsId;
                NormsId = normsId;
            }
        }
    }
}

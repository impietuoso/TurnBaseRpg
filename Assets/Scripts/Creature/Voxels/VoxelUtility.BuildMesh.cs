using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TTT.Voxels
{
    public static partial class VoxelUtility
    {
        public static void ApplyRelaxation(MeshData mesh, int steps = 2, float f = .4f)
        {
            var neighbors = new List<int>[mesh.Vertices.Count];

            for (var i = 0; i < neighbors.Length; i++)
                neighbors[i] = new List<int>();

            foreach (var submesh in mesh.SubmeshTriangles)
            {
                var tris = submesh.Value;

                for (var i = 0; i < tris.Count; i += 3)
                {
                    var a = tris[i];
                    var b = tris[i + 1];
                    var c = tris[i + 2];

                    neighbors[a].Add(b);
                    neighbors[a].Add(c);
                    neighbors[b].Add(a);
                    neighbors[b].Add(c);
                    neighbors[c].Add(a);
                    neighbors[c].Add(b);
                }
            }

            for (var i = 0; i < steps; i++)
                RelaxSurface(mesh.Vertices, neighbors, f);
        }

        private static void RelaxSurface(List<Vector3> vertices, List<int>[] neighbors, float strength = 0.5f)
        {
            var newPositions = new Vector3[vertices.Count];

            for (var i = 0; i < vertices.Count; i++)
            {
                var n = neighbors[i];
                if (n.Count == 0)
                {
                    newPositions[i] = vertices[i];
                    continue;
                }

                var avg = Vector3.zero;
                foreach (var j in n) avg += vertices[j];
                avg /= n.Count;
                newPositions[i] = Vector3.Lerp(vertices[i], avg, strength);
            }

            for (var i = 0; i < vertices.Count; i++)
                vertices[i] = newPositions[i];
        }

        public static Mesh BuildMesh(MeshData meshData, Vector3 rotation)
        {
            if (meshData == null) throw new NullReferenceException();
            var mesh = new Mesh();
            var vertices = meshData.Vertices.ToArray();
            var quaternion = Quaternion.Euler(rotation);

            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = quaternion * vertices[i];

            mesh.SetVertices(vertices);

            var subMeshKeys = meshData.SubmeshTriangles.Keys.ToArray();
            mesh.subMeshCount = subMeshKeys.Length;

            for (var i = 0; i < subMeshKeys.Length; i++)
                mesh.SetTriangles(meshData.SubmeshTriangles[subMeshKeys[i]], i);

            //mesh.RecalculateNormals();
            SetSmoothNormals(mesh);
            mesh.RecalculateBounds();
            return mesh;
        }

        public static void Scale(List<Vector3> vertices, Vector3 scale)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];
                v.x *= scale.x;
                v.y *= scale.y;
                v.z *= scale.z;
                vertices[i] = v;
            }
        }

        public static void Centralize(List<Vector3> vertices, Vector3 center)
        {
            if (vertices.Count == 0) return;

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var v in vertices)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            var size = max - min;
            var targetPivot = min + Vector3.Scale(size, center);

            for (var i = 0; i < vertices.Count; i++)
                vertices[i] -= targetPivot;
        }

        private static void SetSmoothNormals(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var normals = new Vector3[vertices.Length];

            for (var i = 0; i < mesh.subMeshCount; i++)
            {
                var triangles = mesh.GetTriangles(i);
                for (var j = 0; j < triangles.Length; j += 3)
                {
                    int i1 = triangles[j], i2 = triangles[j + 1], i3 = triangles[j + 2];

                    var v1 = vertices[i1];
                    var v2 = vertices[i2];
                    var v3 = vertices[i3];

                    var side1 = v2 - v1;
                    var side2 = v3 - v1;
                    var normal = Vector3.Cross(side1, side2);

                    normals[i1] += normal;
                    normals[i2] += normal;
                    normals[i3] += normal;
                }
            }

            for (var i = 0; i < normals.Length; i++)
                normals[i] = normals[i].normalized;

            mesh.SetNormals(normals);
        }
    }
}
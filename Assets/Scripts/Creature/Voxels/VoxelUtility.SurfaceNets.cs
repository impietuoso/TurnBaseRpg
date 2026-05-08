using System.Collections.Generic;
using UnityEngine;

namespace TTT.Voxels
{
    public static partial class VoxelUtility
    {
        public class MeshData
        {
            public List<Vector3> Vertices = new();
            public Dictionary<int, List<int>> SubmeshTriangles = new();
        }

        public static MeshData GenerateMesh(VoxelObject voxelObject)
        {
            var pad = 1;
            var sx = voxelObject.Size.x + 2 * pad;
            var sy = voxelObject.Size.y + 2 * pad;
            var sz = voxelObject.Size.z + 2 * pad;

            // Store ColorIndex in the grid. 0 means empty.
            var grid = new byte[sx, sy, sz];

            foreach (var v in voxelObject.Voxels)
                grid[v.X + pad, v.Y + pad, v.Z + pad] = v.ColorIndex;

            var mesh = new MeshData();
            var vertexIndices = new int[sx * sy * sz];
            for (var i = 0; i < vertexIndices.Length; i++) vertexIndices[i] = -1;

            // 1. Generate Vertices
            for (var x = 0; x < sx - 1; x++)
            for (var y = 0; y < sy - 1; y++)
            for (var z = 0; z < sz - 1; z++)
            {
                var mask = 0;
                if (grid[x, y, z] > 0) mask |= 1;
                if (grid[x + 1, y, z] > 0) mask |= 2;
                if (grid[x, y + 1, z] > 0) mask |= 4;
                if (grid[x + 1, y + 1, z] > 0) mask |= 8;
                if (grid[x, y, z + 1] > 0) mask |= 16;
                if (grid[x + 1, y, z + 1] > 0) mask |= 32;
                if (grid[x, y + 1, z + 1] > 0) mask |= 64;
                if (grid[x + 1, y + 1, z + 1] > 0) mask |= 128;

                if (mask == 0 || mask == 255) continue;

                var sumPos = Vector3.zero;
                var edgeCount = 0;

                CheckEdge(x, y, z, 1, 0, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y + 1, z, 1, 0, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y, z + 1, 1, 0, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y + 1, z + 1, 1, 0, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y, z, 0, 1, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x + 1, y, z, 0, 1, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y, z + 1, 0, 1, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x + 1, y, z + 1, 0, 1, 0, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y, z, 0, 0, 1, grid, ref sumPos, ref edgeCount);
                CheckEdge(x + 1, y, z, 0, 0, 1, grid, ref sumPos, ref edgeCount);
                CheckEdge(x, y + 1, z, 0, 0, 1, grid, ref sumPos, ref edgeCount);
                CheckEdge(x + 1, y + 1, z, 0, 0, 1, grid, ref sumPos, ref edgeCount);

                if (edgeCount <= 0) continue;
                var avgPos = sumPos / edgeCount;
                mesh.Vertices.Add(avgPos - new Vector3(pad, pad, pad));
                vertexIndices[x + y * sx + z * sx * sy] = mesh.Vertices.Count - 1;
            }

            // 2. Generate Quads
            for (var x = 0; x < sx - 1; x++)
            for (var y = 0; y < sy - 1; y++)
            for (var z = 0; z < sz - 1; z++)
            {
                // Edge along X
                if (y > 0 && z > 0)
                {
                    var c1 = grid[x, y, z];
                    var c2 = grid[x + 1, y, z];
                    if (c1 > 0 != c2 > 0)
                    {
                        var colorIndex = c1 > 0 ? c1 : c2;
                        var v1 = vertexIndices[x + (y - 1) * sx + (z - 1) * sx * sy];
                        var v2 = vertexIndices[x + y * sx + (z - 1) * sx * sy];
                        var v3 = vertexIndices[x + (y - 1) * sx + z * sx * sy];
                        var v4 = vertexIndices[x + y * sx + z * sx * sy];

                        if (v1 != -1 && v2 != -1 && v3 != -1 && v4 != -1)
                            AddQuad(mesh, colorIndex, c2 > 0, v1, v2, v4, v3);
                    }
                }

                // Edge along Y
                if (x > 0 && z > 0)
                {
                    var c1 = grid[x, y, z];
                    var c2 = grid[x, y + 1, z];
                    if (c1 > 0 != c2 > 0)
                    {
                        var colorIndex = c1 > 0 ? c1 : c2;
                        var v1 = vertexIndices[x - 1 + y * sx + (z - 1) * sx * sy];
                        var v2 = vertexIndices[x + y * sx + (z - 1) * sx * sy];
                        var v3 = vertexIndices[x - 1 + y * sx + z * sx * sy];
                        var v4 = vertexIndices[x + y * sx + z * sx * sy];

                        if (v1 != -1 && v2 != -1 && v3 != -1 && v4 != -1)
                            AddQuad(mesh, colorIndex, c1 > 0, v1, v2, v4, v3);
                    }
                }

                // Edge along Z
                if (x > 0 && y > 0)
                {
                    var cc1 = grid[x, y, z];
                    var cc2 = grid[x, y, z + 1];
                    if (cc1 > 0 == cc2 > 0) continue;

                    var colorIndexZ = cc1 > 0 ? cc1 : cc2;
                    var vv1 = vertexIndices[x - 1 + (y - 1) * sx + z * sx * sy];
                    var vv2 = vertexIndices[x + (y - 1) * sx + z * sx * sy];
                    var vv3 = vertexIndices[x - 1 + y * sx + z * sx * sy];
                    var vv4 = vertexIndices[x + y * sx + z * sx * sy];

                    if (vv1 != -1 && vv2 != -1 && vv3 != -1 && vv4 != -1)
                        AddQuad(mesh, colorIndexZ, cc2 > 0, vv1, vv2, vv4, vv3);
                }
            }

            return mesh;
        }

        private static void CheckEdge(int x, int y, int z, int dx, int dy, int dz, byte[,,] grid, ref Vector3 sumPos, ref int edgeCount)
        {
            if (grid[x, y, z] > 0 == grid[x + dx, y + dy, z + dz] > 0) return;
            sumPos += new Vector3(x + dx * 0.5f, y + dy * 0.5f, z + dz * 0.5f);
            edgeCount++;
        }

        private static void AddQuad(MeshData mesh, int colorIndex, bool flip, int v0, int v1, int v2, int v3)
        {
            if (!mesh.SubmeshTriangles.TryGetValue(colorIndex, out var triangles))
                mesh.SubmeshTriangles[colorIndex] = triangles = new List<int>();

            if (flip)
            {
                triangles.Add(v0);
                triangles.Add(v2);
                triangles.Add(v1);
                triangles.Add(v0);
                triangles.Add(v3);
                triangles.Add(v2);
            }
            else
            {
                triangles.Add(v0);
                triangles.Add(v1);
                triangles.Add(v2);
                triangles.Add(v0);
                triangles.Add(v2);
                triangles.Add(v3);
            }
        }
    }
}
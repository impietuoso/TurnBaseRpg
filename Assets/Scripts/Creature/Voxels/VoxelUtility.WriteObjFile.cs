using System.IO;
using System.Text;
using UnityEngine;

namespace TTT.Voxels
{
    public static partial class VoxelUtility
    {
        public static void WriteObjFile(string path, MeshData meshData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Exported from VoxelUtility");

            foreach (var v in meshData.Vertices)
            {
                // Standard OBJ is Right-Handed. Flipping X to convert from Unity LHS.
                sb.AppendLine($"v {-v.x} {v.y} {v.z}");
            }

            foreach (var submesh in meshData.SubmeshTriangles)
            {
                sb.AppendLine($"g color_{submesh.Key}");
                var triangles = submesh.Value;
                for (var i = 0; i < triangles.Count; i += 3)
                {
                    // OBJ indices are 1-based. 
                    // We flip the order of two indices (i+1 and i+2) to maintain correct face orientation after flipping X.
                    sb.AppendLine($"f {triangles[i] + 1} {triangles[i + 2] + 1} {triangles[i + 1] + 1}");
                }
            }

            File.WriteAllText(path, sb.ToString());
            Debug.Log($"Wrote .obj file to {path}");
        }
    }
}

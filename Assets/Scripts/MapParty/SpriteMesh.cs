using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class SpriteMesh : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MeshFilter meshFilter;

        private void Start()
        {
            GenerateMeshFromSpritePhysicsShape();
        }

        private void GenerateMeshFromSpritePhysicsShape()
        {
            var sprite = spriteRenderer.sprite;
            if (sprite == null) return;

            var mesh = new Mesh { name = "SpriteMesh" };
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            var shapeCount = sprite.GetPhysicsShapeCount();
            for (var i = 0; i < shapeCount; i++)
            {
                var shape = new List<Vector2>();
                sprite.GetPhysicsShape(i, shape);

                var startVIndex = vertices.Count;
                foreach (var v in shape)
                {
                    vertices.Add(new Vector3(v.x, v.y, 0));
                    
                    // Convert local position to UV
                    // Sprite.rect is in pixels, but shape vertices are in units relative to pivot
                    var u = (v.x * sprite.pixelsPerUnit + sprite.pivot.x) / sprite.rect.width;
                    var v_coord = (v.y * sprite.pixelsPerUnit + sprite.pivot.y) / sprite.rect.height;
                    uvs.Add(new Vector2(u, v_coord));
                }

                // Simple Fan Triangulation (assumes convex physics shape)
                for (var t = 1; t < shape.Count - 1; t++)
                {
                    triangles.Add(startVIndex);
                    triangles.Add(startVIndex + t);
                    triangles.Add(startVIndex + t + 1);
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, uvs);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }
    }
}

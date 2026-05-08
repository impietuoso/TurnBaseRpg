using System.Collections.Generic;
using System.IO;
using TTT.Voxels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TricksAndTreatsOrThreats.Editor
{
    public class VoxelPreview : MonoBehaviour
    {
        [SerializeField] private MeshRenderer voxelTemplate;
        [SerializeField] private MeshFilter meshTemplate;
        [SerializeField] private Material[] palette;
        [SerializeField] private bool mirror;

        [Header("Mesh Config")]
        [SerializeField] private Object file;
        [SerializeField] private Vector3 meshRotation = new(-90, 0, 90);
        [SerializeField] private Vector3 meshScale = Vector3.one;
        [SerializeField] private Vector3 meshCenter;
        [SerializeField] private int relaxationSteps;
        [SerializeField] private float relaxationF = .4f;

#if UNITY_EDITOR
        [ContextMenu("Preview Mesh")]
        public void PreviewMesh()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);

            if (!file || !meshTemplate)
            {
                Debug.LogWarning("File or MeshFilter is missing.");
                return;
            }

            if (!BuildMeshes(out var meshes)) return;

            foreach (var (mesh, data) in meshes)
            {
                var filter = Instantiate(meshTemplate, transform);

                var pos = new Vector3(data.Position.x, data.Position.z, data.Position.y);
                filter.transform.localPosition = pos;
                filter.sharedMesh = mesh;
                filter.name = data.Name;

                if (!mirror || !data.Name.EndsWith("_m")) continue;
                pos.x *= -1;
                filter = Instantiate(filter, transform);
                filter.name = filter.name.Replace("_m(Clone)", "");
                filter.transform.localPosition = pos;
                filter.transform.localScale = new(-1, 1, 1);
            }
        }

        [ContextMenu("Preview Voxel")]
        public void PreviewVoxel()
        {
            voxelTemplate.gameObject.SetActive(true);
            for (var i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);

            if (!file || !voxelTemplate)
            {
                Debug.LogWarning("File or VoxelPrefab is missing.");
                return;
            }

            if (!ReadVoxelData(out var models)) return;

            foreach (var data in models)
            foreach (var voxel in data.Voxels)
            {
                var instance = Instantiate(voxelTemplate, transform);
                var pos = new Vector3(voxel.X, voxel.Z, voxel.Y); // Swapping Y and Z for Unity coordinate system
                instance.transform.localPosition = data.Position + pos;
                if (voxel.ColorIndex <= 0) continue;
                instance.sharedMaterial = palette[voxel.ColorIndex];
            }

            voxelTemplate.gameObject.SetActive(false);
        }

        [ContextMenu("Write Mesh File")]
        public void WriteMeshFile()
        {
            if (!file)
            {
                Debug.LogWarning("File is missing.");
                return;
            }

            if (!BuildMeshes(out var models)) return;

            var fileName = UnityEditor.AssetDatabase.GetAssetPath(file);
            var folder = Path.GetDirectoryName(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);

            foreach (var (mesh, data) in models)
            {
                var meshName = data.Name.Replace("_m", "");
                var path = Path.Combine(folder!, $"{fileName} {meshName}.asset");

                var existingMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if (existingMesh)
                {
                    existingMesh.Clear();
                    UnityEditor.EditorUtility.CopySerialized(mesh, existingMesh);
                }
                else
                    UnityEditor.AssetDatabase.CreateAsset(mesh, path);
            }

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log($"Saved {models.Count} mesh assets to {folder}");
        }

        private bool ReadVoxelData(out VoxelObject[] models)
        {
            models = null;
            byte[] bytes = null;
            var path = UnityEditor.AssetDatabase.GetAssetPath(file);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                bytes = File.ReadAllBytes(path);
            if (bytes == null) return false;

            models = VoxelUtility.ReadVoxData(bytes);
            return models != null;
        }

        private bool BuildMeshes(out List<(Mesh mesh, VoxelObject data)> meshes)
        {
            meshes = null;
            if (!ReadVoxelData(out var models)) return false;
            meshes = new();

            foreach (var data in models)
            {
                var meshData = VoxelUtility.GenerateMesh(data);
                VoxelUtility.ApplyRelaxation(meshData, relaxationSteps, relaxationF);
                VoxelUtility.Scale(meshData.Vertices, meshScale);
                VoxelUtility.Centralize(meshData.Vertices, meshCenter);
                var mesh = VoxelUtility.BuildMesh(meshData, meshRotation);
                mesh.name = data.Name.Replace("_m", "");
                meshes.Add((mesh, data));
            }

            return true;
        }
#endif
    }
}
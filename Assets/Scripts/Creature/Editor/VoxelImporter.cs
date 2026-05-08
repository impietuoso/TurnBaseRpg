#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using TTT.Voxels;
using UnityEditor;
using UnityEngine;

namespace TTT.Editor
{
    [CustomEditor(typeof(VoxelImporter))]
    public class VoxelImporterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var config = (VoxelImporter)target;
            if (!config.file) return;
            GUILayout.Space(20);

            if (!GUILayout.Button("Import")) return;
            if (!ReadVoxelData(config, out var voxelData)) return;
            var models = BuildMeshes(config, voxelData);
            WriteMeshFile(config, models);
        }

        private static bool ReadVoxelData(VoxelImporter config, out VoxelObject[] models)
        {
            models = null;
            var path = AssetDatabase.GetAssetPath(config.file);
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

            var bytes = File.ReadAllBytes(path);
            models = VoxelUtility.ReadVoxData(bytes);
            return models != null;
        }

        private static List<(Mesh, VoxelObject )> BuildMeshes(VoxelImporter vi, VoxelObject[] voxelData)
        {
            var result = new List<(Mesh mesh, VoxelObject data)>();
            foreach (var data in voxelData)
            {
                var meshData = VoxelUtility.GenerateMesh(data);
                VoxelUtility.ApplyRelaxation(meshData, vi.relaxationSteps, vi.relaxationF);
                VoxelUtility.Scale(meshData.Vertices, vi.meshScale);
                VoxelUtility.Centralize(meshData.Vertices, vi.meshCenter);
                var mesh = VoxelUtility.BuildMesh(meshData, vi.meshRotation);
                mesh.name = data.Name.Replace("_m", "");
                result.Add((mesh, data));
            }

            return result;
        }

        private static void WriteMeshFile(VoxelImporter config, List<(Mesh, VoxelObject)> models)
        {
            var assetPath = AssetDatabase.GetAssetPath(config.file);
            var folder = Path.GetDirectoryName(assetPath);
            var fileName = Path.GetFileNameWithoutExtension(assetPath);

            foreach (var (mesh, data) in models)
            {
                var meshName = data.Name.Replace("_m", "");
                var path = Path.Combine(folder!, $"{fileName} {meshName}.asset");

                var existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if (existingMesh)
                {
                    existingMesh.Clear();
                    EditorUtility.CopySerialized(mesh, existingMesh);
                }
                else
                {
                    AssetDatabase.CreateAsset(mesh, path);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Saved {models.Count} mesh assets to {folder}");
        }
    }
}
#endif
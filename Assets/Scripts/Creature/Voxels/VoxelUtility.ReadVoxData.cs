using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TTT.Voxels
{
    public static partial class VoxelUtility
    {
        public static VoxelObject[] ReadVoxData(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                Debug.LogError("Voxel data is null or empty.");
                return null;
            }

            var usedColors = new Dictionary<byte, byte>();
            var models = new List<VoxelObject>();
            VoxelObject currentModel = null;

            // Scene graph data
            var childToParent = new Dictionary<int, int>();
            var nodeTranslations = new Dictionary<int, Vector3>();
            var nodeNames = new Dictionary<int, string>();
            var shapeNodes = new List<(int nodeId, int modelId)>();

            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);
            var magic = new string(reader.ReadChars(4));
            var version = reader.ReadInt32();

            if (magic != "VOX ")
            {
                Debug.LogError("Invalid VOX file.");
                return null;
            }

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var chunkId = new string(reader.ReadChars(4));
                var chunkContentSize = reader.ReadInt32();
                var chunkChildrenSize = reader.ReadInt32();

                var startPos = reader.BaseStream.Position;

                if (chunkId == "SIZE")
                {
                    var x = reader.ReadInt32();
                    var y = reader.ReadInt32();
                    var z = reader.ReadInt32();
                    currentModel = new VoxelObject { Size = new Vector3Int(x, y, z) };
                    models.Add(currentModel);
                }
                else if (chunkId == "XYZI")
                {
                    if (currentModel != null)
                    {
                        var numVoxels = reader.ReadInt32();
                        for (var i = 0; i < numVoxels; i++)
                        {
                            var voxel = new Voxel
                            {
                                X = reader.ReadByte(),
                                Y = reader.ReadByte(),
                                Z = reader.ReadByte(),
                                ColorIndex = reader.ReadByte()
                            };

                            if (!usedColors.TryGetValue(voxel.ColorIndex, out var v))
                                usedColors[voxel.ColorIndex] = v = (byte)(usedColors.Count + 1);
                            voxel.ColorIndex = v;

                            currentModel.Voxels.Add(voxel);
                        }
                    }
                }
                else if (chunkId == "nTRN")
                {
                    var nodeId = reader.ReadInt32();
                    var nodeAttribsCount = reader.ReadInt32();
                    for (var j = 0; j < nodeAttribsCount; j++)
                    {
                        var key = ReadString(reader);
                        var val = ReadString(reader);
                        if (key == "_name") nodeNames[nodeId] = val;
                    }

                    var childNodeId = reader.ReadInt32();
                    var reservedId = reader.ReadInt32();
                    var layerId = reader.ReadInt32();
                    var numFrames = reader.ReadInt32();

                    childToParent[childNodeId] = nodeId;

                    for (var i = 0; i < numFrames; i++)
                    {
                        var frameAttribsCount = reader.ReadInt32();
                        for (var j = 0; j < frameAttribsCount; j++)
                        {
                            var key = ReadString(reader);
                            var val = ReadString(reader);

                            if (key != "_t") continue;
                            var parts = val.Split(' ');
                            if (parts.Length < 3) continue;

                            if (float.TryParse(parts[0], out var x) &&
                                float.TryParse(parts[1], out var y) &&
                                float.TryParse(parts[2], out var z))
                                // Convert MagicaVoxel (X, Y, Z) to Unity (X, Z, Y)
                                nodeTranslations[nodeId] = new Vector3(x, y, z);
                        }
                    }
                }
                else if (chunkId == "nGRP")
                {
                    var nodeId = reader.ReadInt32();
                    SkipDictionary(reader); // Node attributes

                    var numChildren = reader.ReadInt32();
                    for (var i = 0; i < numChildren; i++)
                    {
                        var childId = reader.ReadInt32();
                        childToParent[childId] = nodeId;
                    }
                }
                else if (chunkId == "nSHP")
                {
                    var nodeId = reader.ReadInt32();
                    SkipDictionary(reader); // Node attributes

                    var numModels = reader.ReadInt32();
                    for (var i = 0; i < numModels; i++)
                    {
                        var modelId = reader.ReadInt32();
                        SkipDictionary(reader); // Model attributes
                        shapeNodes.Add((nodeId, modelId));
                    }
                }

                // Move to next chunk
                reader.BaseStream.Position = startPos + chunkContentSize;
            }

            // Resolve positions and names from scene graph
            foreach (var (shapeNodeId, modelId) in shapeNodes)
            {
                if (modelId < 0 || modelId >= models.Count) continue;
                var pos = Vector3.zero;
                string name = null;
                var curr = shapeNodeId;

                // Traverse up the tree
                while (true)
                {
                    if (nodeTranslations.TryGetValue(curr, out var t))
                        pos += t;
                    if (string.IsNullOrEmpty(name) && nodeNames.TryGetValue(curr, out var n))
                        name = n;

                    if (!childToParent.TryGetValue(curr, out var parent)) break;
                    curr = parent;
                }

                models[modelId].Position = pos;
                models[modelId].Name = name;
            }

            return models.ToArray();
        }

        private static void SkipDictionary(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var kLen = reader.ReadInt32();
                reader.BaseStream.Seek(kLen, SeekOrigin.Current);
                var vLen = reader.ReadInt32();
                reader.BaseStream.Seek(vLen, SeekOrigin.Current);
            }
        }

        private static string ReadString(BinaryReader reader)
        {
            var len = reader.ReadInt32();
            var bytes = reader.ReadBytes(len);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
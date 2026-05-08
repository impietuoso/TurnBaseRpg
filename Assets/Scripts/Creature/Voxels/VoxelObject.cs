using System.Collections.Generic;
using UnityEngine;

namespace TTT.Voxels
{
    public class VoxelObject
    {
        public string Name;
        public Vector3 Position;
        public Vector3Int Size;
        public readonly List<Voxel> Voxels = new();
    }
}
using System.Linq;
using UnityEngine;

namespace TTT.Voxels
{
    public class VoxelPalette : MonoBehaviour
    {
        public Material[] palette;

        [ContextMenu("Update Palette")]
        public void UpdatePalette()
        {
            foreach (var part in GetComponentsInChildren<VoxelPalettePart>())
                part.GetComponent<MeshRenderer>().sharedMaterials
                    = part.indexes.Select(i => palette[i]).ToArray();
        }
    }
}
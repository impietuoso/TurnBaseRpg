using UnityEngine;
using Object = UnityEngine.Object;

namespace TTT.Voxels
{
    public class VoxelImporter : ScriptableObject
    {
        public Object file;
        public Vector3 meshRotation = new(-90, 0, 90);
        public Vector3 meshScale = Vector3.one;
        public Vector3 meshCenter;
        public int relaxationSteps;
        public float relaxationF = .4f;
    }
}
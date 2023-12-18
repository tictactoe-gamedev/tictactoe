using UnityEngine;

[CreateAssetMenu(fileName = "VoxelBase", menuName = "Custom/Voxels")]
public class VoxelData : ScriptableObject
{
    public VoxelType type;
    public int amount;
    public Material material;
}

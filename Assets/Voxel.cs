using UnityEngine;

public class Voxel : MonoBehaviour
{
    public enum VoxelType { Gold, Silver, Bronze, Platinum }

    [SerializeField] private VoxelType type;
    [SerializeField] private int amount;

    public VoxelType Type => type;
    public int Amount => amount;

    // Base spawn probability
    public float BaseSpawnProbability { get; private set; }

    // Modifiers for neighbouring voxel spawns
    public float[] NeighbourModifiers { get; private set; }

    public void Initialize(VoxelType voxelType, int voxelAmount,
         float baseSpawnProbability, float[] neighbourModifiers)
    {
        type = voxelType;
        amount = voxelAmount;
        BaseSpawnProbability = baseSpawnProbability;
        NeighbourModifiers = neighbourModifiers;
    }
}
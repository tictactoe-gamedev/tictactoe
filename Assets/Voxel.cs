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

    public static int TotalVoxelCount { get; private set; }

    private bool isInitialized = false;

    private VoxelType defaultVoxelType = VoxelType.Gold; // Default type
    private int defaultVoxelAmount = 0;
    private float defaultBaseSpawnProbability = 0.0f;
    private float[] defaultNeighbourModifiers = new float[0]; // Default values

    // check if the voxel has been initialized 
    void Awake()
    {
        if (!isInitialized)
        {
            Initialize(defaultVoxelType, defaultVoxelAmount, defaultBaseSpawnProbability, defaultNeighbourModifiers);
        }
    }

    // sets up the voxel properties and marks the voxel as initialized. It also increments the total voxel count.
    public void Initialize(VoxelType voxelType, int voxelAmount,
         float baseSpawnProbability, float[] neighbourModifiers)
    {
        isInitialized = true;
        type = voxelType;
        amount = voxelAmount;
        BaseSpawnProbability = baseSpawnProbability;
        NeighbourModifiers = neighbourModifiers;
        TotalVoxelCount++;
    }

    void OnDestroy()
    {
        // Drops the total voxel count when a voxel is destroyed.
        TotalVoxelCount--;
    }
}
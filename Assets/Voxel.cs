using UnityEngine;

public class Voxel : MonoBehaviour
{
    public enum VoxelType
    {
        Undefined,
        Gold,
        Silver,
        Bronze,
        Platinum
    }

    [SerializeField] private VoxelType type;
    [SerializeField] private int amount;

    public VoxelType Type => type;
    public int Amount => amount;

    [SerializeField] private float baseSpawnProbability = 0f;
    public float BaseSpawnProbability => baseSpawnProbability;

    [SerializeField] private float[] neighbourModifiers = new float[0];
    public float[] NeighbourModifiers => neighbourModifiers;

    public static int TotalVoxelCount { get; private set; }

    private bool isInitialized = false;
    private VoxelType defaultVoxelType = VoxelType.Gold; // Default type
    private int defaultVoxelAmount = 0;

    void Awake()
    {
        if (!isInitialized)
        {
            type = defaultVoxelType;
            amount = defaultVoxelAmount;
            isInitialized = true;
            TotalVoxelCount++;
        }
    }

    public void Initialize(VoxelType voxelType, int voxelAmount,
                        float baseSpawnProb, float[] neighbourMods)
    {
        isInitialized = true;
        type = voxelType;
        amount = voxelAmount;
        baseSpawnProbability = baseSpawnProb;
        neighbourModifiers = neighbourMods;
        TotalVoxelCount++;
    }

    void OnDestroy()
    {
        TotalVoxelCount--;
    }
}
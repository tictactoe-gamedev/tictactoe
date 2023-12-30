using UnityEngine;
using System.Collections.Generic;

public class FinecraftGod : MonoBehaviour
{
    public static FinecraftGod Instance { get; private set; } // ensure only one FinecraftGod exists
    public static int TotalVoxelCount {  get; private set; }

    [SerializeField] private Voxel[] voxelPrefabs;
    [SerializeField] private Vector3 worldDimensions = new Vector3(5, 1, 3);
    [SerializeField] private int maxVoxels = 20;
    [SerializeField] private float noSpawnProbability = 0.1f;   // Chance of not spawning a voxel, adjustable in the Inspector
    [SerializeField] private int heightLevelsToProcess = 3;     // number of height levels to process, adjustable in the Inspector
    [SerializeField] private int minVoxelAmount = 1;
    [SerializeField] private int maxVoxelAmount = 11;

    private int currentYLevel = 0;

    [System.Serializable]
    private struct VoxelGenerationRule
    {
        [SerializeField] private float baseSpawnProbability;
        [SerializeField] private float[] neighbourModifiers;

        public float BaseSpawnProbability => baseSpawnProbability;
        public float[] NeighbourModifiers => neighbourModifiers;
    }

    [SerializeField] private VoxelGenerationRule[] voxelTypeData;
    private Voxel[,,] voxels; // the ,, signifies a 3d array

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Direct initialization of the voxel array
        voxels = new Voxel[(int)worldDimensions.x, (int)worldDimensions.y, (int)worldDimensions.z];
        Debug.Log($"Voxel array created with dimensions: {worldDimensions.x}x{worldDimensions.y}x{worldDimensions.z}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && TotalVoxelCount < maxVoxels)
        {
            Debug.Log("K key press detected. Populating current Y level with voxels.");
            PopulateLevel(currentYLevel);
        }
    }

    private void PopulateLevel(int level)
    {
        Debug.Log($"Populating Y level {level} with voxels.");

        // loops thru every position in current Y level of the world
        for (int x = 0; x < worldDimensions.x; x++)
        {
            for (int z = 0; z < worldDimensions.z; z++)
            {
                Vector3Int position = new Vector3Int(x, level, z);

                // noSpawnProbability check
                if (Random.value > noSpawnProbability)
                {
                    DecideAndCreateVoxel(position);
                }
                else
                {
                    Debug.Log($"No voxel created at {position} due to no-spawn probability");
                }
            }
        }
        // After filling up one Y level, moves to the next level
        currentYLevel += 1;
    }

    void DecideAndCreateVoxel(Vector3Int position)
    {
        // Directly decide and create voxels
        Voxel.VoxelType chosenType = ChooseVoxelTypeBasedOnProbability(position);
        Voxel voxel = InstantiateVoxel(position, chosenType);
        voxels[position.x, position.y, position.z] = voxel;
    }

    private Voxel InstantiateVoxel(Vector3Int position, Voxel.VoxelType voxelType)
    {
        Voxel voxelScript = Instantiate(voxelPrefabs[(int)voxelType], position, Quaternion.identity); // spawns voxel at the random position without any rotation applied to the voxel object

        if (voxelScript)
        {
            int amount = Random.Range(minVoxelAmount, maxVoxelAmount);

            voxelScript.Initialize(voxelType, amount,
                        voxelTypeData[(int)voxelType].BaseSpawnProbability,
                        voxelTypeData[(int)voxelType].NeighbourModifiers);

            Debug.Log($"Voxel prefab created at {position} with type {voxelType}.");
        }

        return voxelScript;
    }

    void OnDestroy()
    {
        TotalVoxelCount--;
    }

    private Voxel.VoxelType ChooseVoxelTypeBasedOnProbability(Vector3Int position)
    {
        // calculate the chance for each voxel type to spawn based on their neighbours
        float[] probabilities = CalculateInfluencedProbability(position);

        // summing up these probabilities
        float totalProbability = 0;
        foreach (var prob in probabilities)
            totalProbability += prob;

        // picks a random value
        float randomPoint = Random.value * totalProbability;
        // Cumulative probability check
        float cumulativeProbability = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomPoint <= cumulativeProbability)
            {
                Debug.Log($"Voxel type chosen based on probability: {(Voxel.VoxelType)i}");
                return (Voxel.VoxelType)i;
            }
        }
        // 'Undefined' picked as the default voxel type
        return Voxel.VoxelType.Undefined;
    }

    private float[] CalculateInfluencedProbability(Vector3Int position)
    {
        List<Voxel.VoxelType> neighbours = GetNeighbourVoxelTypes(position);
        float[] probabilities = new float[voxelTypeData.Length];
        Debug.Log($"Calculating probabilities for position {position}.");

        for (int i = 0; i < probabilities.Length; i++)
        {
            float initialProb = voxelTypeData[i].BaseSpawnProbability;
            probabilities[i] = initialProb;

            Debug.Log($"Initial probability for type {(Voxel.VoxelType)i}: {initialProb}");

            foreach (var neighbour in neighbours)
            {
                probabilities[i] += voxelTypeData[i].NeighbourModifiers[(int)neighbour];
            }
        }

        return probabilities;
    }

    private List<Voxel.VoxelType> GetNeighbourVoxelTypes(Vector3Int position)
    {
        var neighbourTypes = new List<Voxel.VoxelType>();
        var directions = new Vector3Int[] { Vector3Int.up, Vector3Int.down,
                             Vector3Int.left, Vector3Int.right, Vector3Int.forward,
                             Vector3Int.back,

        // Checks for voxels in diagonal directions
        new Vector3Int(1, 1, 1), new Vector3Int(-1, 1, 1), new Vector3Int(1, -1, 1),
        new Vector3Int(-1, -1, 1), new Vector3Int(1, 1, -1), new Vector3Int(-1, 1, -1),
        new Vector3Int(1, -1, -1), new Vector3Int(-1, -1, -1)
    };

        foreach (var dir in directions)
        {
            var neighbourPos = position + dir;
            if (VoxelIsAtValidPosition(neighbourPos) && voxels[neighbourPos.x, neighbourPos.y, neighbourPos.z] != null)
            {
                neighbourTypes.Add(voxels[neighbourPos.x, neighbourPos.y, neighbourPos.z].Type);
            }
        }

        return neighbourTypes;
    }

    private bool VoxelIsAtValidPosition(Vector3Int position)
    {
        // Check if the given position is within the boundaries of our world
        return position.x >= 0 && position.x < worldDimensions.x &&
               position.y >= 0 && position.y < worldDimensions.y &&
               position.z >= 0 && position.z < worldDimensions.z;
    }
}
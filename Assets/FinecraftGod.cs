using UnityEngine;
using System.Collections.Generic;

public class FinecraftGod : MonoBehaviour
{
    public static FinecraftGod Instance { get; private set; } // ensure only one FinecraftGod exists

    [SerializeField] private GameObject voxelPrefab;
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

        voxels = new Voxel[(int)worldDimensions.x, (int)worldDimensions.y, (int)worldDimensions.z];
        Debug.Log($"Voxel array created with dimensions: {worldDimensions.x}x{worldDimensions.y}x{worldDimensions.z}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && Voxel.TotalVoxelCount < maxVoxels)
        {
            Debug.Log("K key press detected. Populating current Y level with voxels.");
            PopulateCurrentYLevel();
        }
    }

    private void PopulateCurrentYLevel()
    {
        Debug.Log($"Populating Y level {currentYLevel} with voxels.");
        // loops thru every position in current Y level of the world
        for (int x = 0; x < worldDimensions.x; x++)
        {
            for (int z = 0; z < worldDimensions.z; z++)
            {
                Vector3Int position = new Vector3Int(x, currentYLevel, z);

                // checks if there's no voxel at this position and creates one if empty
                if (voxels[x, currentYLevel, z] == null)
                    CreateVoxel(position);
            }
        }
        // After filling up one Y level, moves to the next level
        currentYLevel = (currentYLevel + 1) % heightLevelsToProcess;
        Debug.Log($"Completed populating Y level {currentYLevel}.");
    }

    void CreateVoxel(Vector3Int position)
    {
        // Sometimes decides not to create a voxel
        if (Random.value <= noSpawnProbability)
        {
            Debug.Log($"No voxel created at {position} due to no-spawn probability");
            return;
        }
        // Choose what type of voxel to create based on nearby voxels
        Voxel.VoxelType chosenType = ChooseVoxelTypeBasedOnProbability(position);
        // Create the voxel at the chosen position with the chosen type
        Voxel voxel = InstantiateVoxel(position, chosenType);
        // Store the created voxel in an array
        voxels[position.x, position.y, position.z] = voxel;

        Debug.Log($"Voxel of type {chosenType} created at {position}.");
    }

    private Voxel InstantiateVoxel(Vector3Int position, Voxel.VoxelType voxelType)
    {
        var voxelObject = Instantiate(voxelPrefab, position, Quaternion.identity); // spawns voxel at the random position without any rotation applied to the voxel object
        var voxelScript = voxelObject.GetComponent<Voxel>();

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
        // checks each voxel type and sees which ones had highest probability of spawning
        for (int i = 0; i < probabilities.Length; i++)
        {
            // If our random point is less than the current probability, it found the voxel type
            if (randomPoint < probabilities[i])
            {
                Debug.Log($"Voxel type chosen based on probability: {(Voxel.VoxelType)i}");
                return (Voxel.VoxelType)i;
                // Else, subtracts this probability and checks the next voxel type
            }
            randomPoint -= probabilities[i];
        }
        // 'Gold' picked as the default voxel type
        return Voxel.VoxelType.Gold;
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
                             Vector3Int.back };

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
using UnityEngine;
using System.Collections.Generic;

public class FinecraftGod : MonoBehaviour
{
    public static FinecraftGod Instance { get; private set; } // ensure only one FinecraftGod exists

    [SerializeField] private GameObject voxelPrefab;
    [SerializeField] private int worldX = 5, worldY = 1, worldZ = 3, maxVoxels = 20;
    [SerializeField] private float noSpawnProbability = 0.1f;   // Chance of not spawning a voxel, adjustable in the Inspector
    [SerializeField] private int heightLevelsToProcess = 3;     // number of height levels to process, adjustable in the Inspector
    [SerializeField] private int minVoxelAmount = 1;
    [SerializeField] private int maxVoxelAmount = 11;

    private int currentYLevel = 0;

    [System.Serializable]
    public class VoxelData
    {
        public float baseSpawnProbability;      // the likelihood of the voxel type spawning
        public float[] neighbourModifiers;      // how neighbouring voxel influences spawn probability
    }

    [SerializeField] private VoxelData[] voxelTypeData;

    private int totalVoxelCount = 0;
    private int[] voxelTypeCounts; 

    /*
     * HashSet used to store created voxel positions.Vector3Int represents 3d position using xyz coords.
     * HashSet ensures no 2 voxels are created at same position
     * HashSet quickly tells if a position is already occupied. Much faster than a list or array
     * 
     * voxelPositions essential for the neighbour detection functionality
     * Can quickly check adjacent positions to find out which are occupied
    */
    private HashSet<Vector3Int> voxelPositions;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFields();
    }

    void Start()
    {
        GenerateInitialVoxel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && totalVoxelCount < maxVoxels)
        {
            PopulateCurrentYLevel();
        }
    }

    private void InitializeFields()
    {
        voxelPositions = new HashSet<Vector3Int>();
        voxelTypeCounts = new int[voxelTypeData.Length];
    }

    private void GenerateInitialVoxel()
    {
        Vector3Int initialPosition = GetRandomPosition();
        CreateVoxel(initialPosition);
    }

    private Vector3Int GetRandomPosition()
    {
        return new Vector3Int(Random.Range(-worldX, worldX + 1), worldY, Random.Range(-worldZ, worldZ + 1));
    }

    private void PopulateCurrentYLevel()
    {
        for (int x = -worldX; x <= worldX; x++)
        {
            for (int z = -worldZ; z <= worldZ; z++)
            {
                CreateVoxel(new Vector3Int(x, currentYLevel, z));
            }
        }
        currentYLevel = (currentYLevel + 1) % heightLevelsToProcess;
    }

    void CreateVoxel(Vector3Int position)
    {
        if (voxelPositions.Contains(position) || Random.value <= noSpawnProbability)
        {
            Debug.Log($"No voxel created at {position} (either position occupied or hit no-spawn probability)");
            return;
        }

        float[] influencedProbabilities = CalculateInfluencedProbability(position);
        Voxel.VoxelType chosenType = ChooseVoxelTypeBasedOnProbability(influencedProbabilities);

        var voxelObject = Instantiate(voxelPrefab, position, Quaternion.identity);
        var voxelScript = voxelObject.GetComponent<Voxel>();
        if (voxelScript != null)
        {
            var data = voxelTypeData[(int)chosenType];
            int amount = Random.Range(1, 11);
            voxelScript.Initialize(chosenType, amount, data.baseSpawnProbability, data.neighbourModifiers);

            voxelTypeCounts[(int)chosenType]++;
            totalVoxelCount++;
            voxelPositions.Add(position);

            Debug.Log($"Created {chosenType} voxel at {position} with amount {amount}");
        }
    }

    float[] CalculateInfluencedProbability(Vector3Int position)
    {
        List<Voxel.VoxelType> neighbours = GetNeighbourVoxelTypes(position);
        float[] probabilities = new float[voxelTypeData.Length];

        Debug.Log($"Calculating probabilities for position {position}. Neighbours: {string.Join(", ", neighbours)}");

        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] = voxelTypeData[i].baseSpawnProbability;
            foreach (var neighbour in neighbours)
            {
                probabilities[i] += voxelTypeData[i].neighbourModifiers[(int)neighbour];
                Debug.Log($"Neighbour {neighbour} influenced Type {(Voxel.VoxelType)i} probability to {probabilities[i]}");
            }
        }

        return probabilities;
    }

    private Voxel.VoxelType ChooseVoxelTypeBasedOnProbability(float[] probabilities)
    {
        float totalProbability = 0;
        foreach (var prob in probabilities)
            totalProbability += prob;

        float randomPoint = Random.value * totalProbability;
        for (int i = 0; i < probabilities.Length; i++)
        {
            if (randomPoint < probabilities[i])
                return (Voxel.VoxelType)i;

            randomPoint -= probabilities[i];
        }

        return Voxel.VoxelType.Gold;
    }

    private void InstantiateVoxel(Vector3Int position, Voxel.VoxelType voxelType)
    {
        var voxelObject = Instantiate(voxelPrefab, position, Quaternion.identity); // spawns voxel at the random position without any rotation applied to the voxel object
        var voxelScript = voxelObject.GetComponent<Voxel>();

        int amount = Random.Range(minVoxelAmount, maxVoxelAmount);

        if (voxelScript)
        {
            var data = voxelTypeData[(int)voxelType];
            voxelScript.Initialize(voxelType, amount, data.baseSpawnProbability, data.neighbourModifiers);
            UpdateVoxelCounts(voxelType);
        }
    }

    private void UpdateVoxelCounts(Voxel.VoxelType voxelType)
    {
        voxelTypeCounts[(int)voxelType]++;
        totalVoxelCount++;
        Debug.Log($"Created {voxelType} voxel at {voxelTypeCounts[(int)voxelType]}");
    }

    private List<Voxel.VoxelType> GetNeighbourVoxelTypes(Vector3Int position)
    {
        var neighbourTypes = new List<Voxel.VoxelType>();
        var directions = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };

        foreach (var dir in directions)
        {
            var neighbourPos = position + dir;
            if (voxelPositions.Contains(neighbourPos))
                neighbourTypes.Add(GetVoxelTypeAtPosition(neighbourPos));
        }
        return neighbourTypes;
    }

    private Voxel.VoxelType GetVoxelTypeAtPosition(Vector3Int position)
    {
        return (Voxel.VoxelType)Random.Range(0, voxelTypeData.Length);
    }
}
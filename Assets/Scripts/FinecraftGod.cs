using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject voxelPrefab;

    private Vector3 universeSize;

    public Material[] voxelMaterials;

    private int maxVoxelCount;

    private float noSpawnProbability = 0.2f;

    public float[] baseSpawnPossibilities;
    public float[] goldSpawnModifiers;
    public float[] silverSpawnModifiers;
    public float[] bronzeSpawnModifiers;
    public float[] platinumSpawnModifiers;

    private Vector3[] neighborOffsets = new Vector3[] {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

    private int totalVoxelCount => Voxel.goldVoxelCount + Voxel.silverVoxelCount + Voxel.bronzeVoxelCount + Voxel.platinumVoxelCount;

    private float voxelCreationCooldown = 0.01f;
    private float timeSinceLastVoxelCreation;

    private HashSet<Vector3> createdVoxelPositions = new HashSet<Vector3>();

    void Start()
    {
        universeSize = new Vector3(Random.Range(1, 11), Random.Range(1, 11), Random.Range(1, 11));
        Debug.Log($"World Created: X: {universeSize.x}, Y: {universeSize.y}, Z:{universeSize.z}");
        maxVoxelCount = (int)universeSize.x * (int)universeSize.y * (int)universeSize.z;
        Debug.Log($"Max Voxel Count: {maxVoxelCount}");
        GeneratePlane();
    }

    void Update()
    {
        timeSinceLastVoxelCreation += Time.deltaTime;

            if (Input.GetKey(KeyCode.K) && timeSinceLastVoxelCreation >= voxelCreationCooldown)
            //for(int i=0; i<maxVoxelCount; i++)
            {
                if(timeSinceLastVoxelCreation >= voxelCreationCooldown)
                {
                    if (totalVoxelCount == maxVoxelCount)
                    {
                        Debug.Log("Maximum voxel count reached. Cannot generate more voxels.");
                        return;
                    }
                    else
                    {
                        GenerateRandomVoxel();
                        timeSinceLastVoxelCreation = 0.0f;
                    }
                }
 
            }
    }

    void GenerateRandomVoxel()
    {
        float noSpawnRandom = Random.Range(0f, 1f);

        if (noSpawnRandom <= noSpawnProbability)
        {
            Debug.Log("Voxel not spawned.");
            return;
        }

        float xPos = Mathf.RoundToInt(Random.Range(0, universeSize.x-1));
        float yPos = Mathf.RoundToInt(Random.Range(0, universeSize.y-1));
        float zPos = Mathf.RoundToInt(Random.Range(0, universeSize.z-1));

        Vector3 voxelPosition = new Vector3(xPos, yPos, zPos);

        if (createdVoxelPositions.Contains(voxelPosition))
        {
            Debug.Log($"Voxel not spawned at {voxelPosition}. Voxel already exists at that position.");
            return;
        }

        createdVoxelPositions.Add(voxelPosition);

        Debug.Log($"Checking neighbors for point X: {xPos}, Y: {yPos}, Z: {zPos}");

        foreach (Vector3 offset in neighborOffsets)
        {
            Vector3 neighborPos = new Vector3(xPos, yPos, zPos) + offset;

            if(neighborPos.x >= -1 && neighborPos.x < universeSize.x+1 &&
                neighborPos.y >= -1 && neighborPos.y < universeSize.y+1 &&
                neighborPos.z >= -1 && neighborPos.z < universeSize.z+1)
            {
                Voxel neighborVoxel = GetVoxelAtPosition(neighborPos);
                if(neighborVoxel != null)
                {
                    Debug.Log($"Neighbor at X: {neighborPos.x}, Y: {neighborPos.y}, Z: {neighborPos.z} is {voxelMaterials[neighborVoxel.Type].name}");
                }
            }
        }

        int typeRandom = CalculateWeightedRandomType(xPos, yPos, zPos);
        int amountRandom = Random.Range(1, 10);
        if (typeRandom >= 0 &&  typeRandom < voxelMaterials.Length)
        {
            Debug.Log($"Random possibility: {noSpawnRandom}. Creating a {voxelMaterials[typeRandom].name} voxel at X: {xPos}, Y: {yPos}, Z: {zPos}");

            GameObject voxelObject = Instantiate(voxelPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);

            Voxel voxelComponent = voxelObject.GetComponent<Voxel>();

            voxelObject.GetComponent<Renderer>().material = voxelMaterials[typeRandom];

            voxelComponent.Initialize(typeRandom, amountRandom, baseSpawnPossibilities[typeRandom], GetSpawnModifiers(typeRandom));

            LogVoxelCounts();
        }
        else
        {
            Debug.LogError($"Invalid typeRandom value: {typeRandom}. It's outside the bounds of voxelMaterials array.");
        }
    }

    Voxel GetVoxelAtPosition(Vector3 position)
    {
        if (createdVoxelPositions.Contains(position))
        {
            return new Voxel();
        }
        return null;
    }

    float[] GetSpawnModifiers(int voxelType)
    {
        switch(voxelType)
        {
            case 0: return goldSpawnModifiers;
            case 1: return silverSpawnModifiers;
            case 2: return bronzeSpawnModifiers;
            case 3: return platinumSpawnModifiers;
            default: return new float[0];
        }
    }

    int CalculateWeightedRandomType(float x, float y, float z)
    {
        float bronzeWeight = 0.4f;
        float silverWeight = 0.3f;
        float goldWeight = 0.2f;
        float platinumWeight = 0.1f;

        float totalWeight = bronzeWeight + silverWeight + goldWeight + platinumWeight;

        // Calculate the sum of weights of neighboring voxels
        foreach (Vector3 offset in neighborOffsets)
        {
            Vector3 neighborPos = new Vector3(x, y, z) + offset;

            if (neighborPos.x >= 0 && neighborPos.x < universeSize.x &&
                neighborPos.y >= 0 && neighborPos.y < universeSize.y &&
                neighborPos.z >= 0 && neighborPos.z < universeSize.z)
            {
                Voxel neighborVoxel = GetVoxelAtPosition(neighborPos);
                if (neighborVoxel != null)
                {
                    switch (neighborVoxel.Type)
                    {
                        case 0: // Bronze
                            totalWeight += 0f;
                            break;
                        case 1: // Silver
                            totalWeight += 0.09f;
                            break;
                        case 2: // Gold
                            totalWeight += 0.06f;
                            break;
                        case 3: // Platinum
                            totalWeight += 0f;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        float randomValue = Random.Range(0f, totalWeight);

        if (randomValue <= bronzeWeight)
            return 0; // Bronze voxel type
        else if (randomValue <= bronzeWeight + silverWeight)
            return 1; // Silver voxel type
        else if (randomValue <= bronzeWeight + silverWeight + goldWeight)
            return 2; // Gold voxel type
        else
            return 3; // Platinum voxel type
    }

    void GeneratePlane()
    {
        for(int x=0;x < universeSize.x;x++)
        {
            for(int z=0;z < universeSize.z; z++)
            {
                GameObject voxelObject = Instantiate(voxelPrefab, new Vector3(x, -1, z), Quaternion.identity);
            }
        }
    }

    void LogVoxelCounts()
    {
        Debug.Log($"Gold Voxels: {Voxel.goldVoxelCount}, Silver Voxels: {Voxel.silverVoxelCount}, Bronze Voxels: {Voxel.bronzeVoxelCount}, Platinum Voxels: {Voxel.platinumVoxelCount}");
    }
}
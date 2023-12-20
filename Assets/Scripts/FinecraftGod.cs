using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject voxelPrefab;

    public Vector3 universeSize;

    public int maxVoxelCount = 100;

    private int totalVoxelCount => Voxel.goldVoxelCount + Voxel.silverVoxelCount + Voxel.bronzeVoxelCount + Voxel.platinumVoxelCount;

    void Start()
    {
        universeSize = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
        Debug.Log($"World Created: X: {universeSize.x}, Y: {universeSize.y}, Z:{universeSize.z}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateRandomVoxel();
        }
    }

    void GenerateRandomVoxel()
    {

        if (totalVoxelCount == maxVoxelCount)
        {
            Debug.Log("Maximum voxel count reached. Cannot generate more voxels.");
            return;
        }

        int typeRandom = Random.Range(0, 4);
        int amountRandom = Random.Range(1, 10);
        
        float xPos = Mathf.RoundToInt(Random.Range(-universeSize.x, universeSize.x));
        float yPos = Mathf.RoundToInt(Random.Range(-universeSize.y, universeSize.y));
        float zPos = Mathf.RoundToInt(Random.Range(-universeSize.z, universeSize.z));

        Debug.Log($"Coordinates: X: {xPos}, Y: {yPos}, Z: {zPos}");


        GameObject voxelObject = Instantiate(voxelPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);


        Voxel voxelComponent = voxelObject.GetComponent<Voxel>();


        voxelComponent.Initialize(typeRandom, amountRandom);

        LogVoxelCounts();
    }

    void LogVoxelCounts()
    {
        Debug.Log($"Gold Voxels: {Voxel.goldVoxelCount}, Silver Voxels: {Voxel.silverVoxelCount}, Bronze Voxels: {Voxel.bronzeVoxelCount}, Platinum Voxels: {Voxel.platinumVoxelCount}");
    }
}

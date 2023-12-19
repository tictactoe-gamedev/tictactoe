using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject voxelPrefab;

    public Vector3 universeSize;

    public int maxVoxelCount = 100;

    private int totalVoxelCount => Voxel.goldVoxelCount + Voxel.silverVoxelCount + Voxel.bronzeVoxelCount + Voxel.platinumVoxelCount;

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
        int amountRandom = Random.Range(0, 10);
        float xPos = Mathf.RoundToInt(Random.Range(-universeSize.x * 10, (universeSize.x + 1) * 10));
        float yPos = Mathf.RoundToInt(Random.Range(-universeSize.y * 10, (universeSize.y + 1) * 10));
        float zPos = Mathf.RoundToInt(Random.Range(-universeSize.z * 10, (universeSize.z + 1) * 10));

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

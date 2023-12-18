using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject voxelPrefab;

    public int universeSizeX = 10;
    public int universeSizeY = 10;
    public int universeSizeZ = 10;

    public int maxVoxelCount = 100;

    private static int goldVoxelCount = 0;
    private static int silverVoxelCount = 0;
    private static int bronzeVoxelCount = 0;
    private static int platinumVoxelCount = 0;


    void Start()
    {
        
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

        if (goldVoxelCount + silverVoxelCount + bronzeVoxelCount + platinumVoxelCount >= maxVoxelCount)
        {
            Debug.Log("Maximum voxel count reached. Cannot generate more voxels.");
            return;
        }

        int typeRandom = Random.Range(0, 4);
        int xPos = Random.Range(-universeSizeX, universeSizeX + 1);
        int yPos = Random.Range(-universeSizeY, universeSizeY + 1);
        int zPos = Random.Range(-universeSizeZ, universeSizeZ + 1);

        Debug.Log($"Coordinates: X: {xPos}, Y: {yPos}, Z: {zPos}");


        GameObject voxelObject = Instantiate(voxelPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);


        Voxel voxelComponent = voxelObject.GetComponent<Voxel>();


        switch (typeRandom)
        {
            case 0:
                voxelComponent.Initialize(0, 0);
                goldVoxelCount++;
                break;
            case 1:
                voxelComponent.Initialize(1, 0);
                silverVoxelCount++;
                break;
            case 2:
                voxelComponent.Initialize(2, 0);
                bronzeVoxelCount++;
                break;
            case 3:
                voxelComponent.Initialize(3, 0);
                platinumVoxelCount++;
                break;
            default:
                break;
        }
        LogVoxelCounts();
    }

    void LogVoxelCounts()
    {
        Debug.Log($"Gold Voxels: {goldVoxelCount}, Silver Voxels: {silverVoxelCount}, Bronze Voxels: {bronzeVoxelCount}, Platinum Voxels: {platinumVoxelCount}");
    }
}

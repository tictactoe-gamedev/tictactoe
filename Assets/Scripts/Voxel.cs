using UnityEngine;

public class Voxel : MonoBehaviour
{
    public int Type { get; private set; }
    public int Amount { get; private set; }

    public float BaseSpawnPossibility { get; private set; }
    public float[] NeighborSpawnModifiers { get; private set; }

    public static int goldVoxelCount = 0;
    public static int silverVoxelCount = 0;
    public static int bronzeVoxelCount = 0;
    public static int platinumVoxelCount = 0;

    public void Initialize(int voxelType, int voxelAmount, float baseSpawnPossibility, float[] neighborSpawnModifiers)
    {
        Type = voxelType;
        Amount = voxelAmount;
        BaseSpawnPossibility = baseSpawnPossibility;
        NeighborSpawnModifiers = neighborSpawnModifiers;
        
        switch(Type)
        {
            case 0:
                bronzeVoxelCount++;
                break;
            case 1:
                silverVoxelCount++; 
                break;
            case 2:
                goldVoxelCount++; 
                break;
            case 3:
                platinumVoxelCount++; 
                break;
            default:
                break;
        }
    }
}

using UnityEngine;

public class Voxel : MonoBehaviour
{
    public int Type { get; private set; }
    public int Amount { get; private set; }

    public static int goldVoxelCount = 0;
    public static int silverVoxelCount = 0;
    public static int bronzeVoxelCount = 0;
    public static int platinumVoxelCount = 0;

    public void Initialize(int voxelType, int voxelAmount)
    {
        Type = voxelType;
        Amount = voxelAmount;
        
        switch(Type)
        {
            case 0:
                goldVoxelCount++;
                break;
            case 1:
                silverVoxelCount++; 
                break;
            case 2:
                bronzeVoxelCount++; 
                break;
            case 3:
                platinumVoxelCount++; 
                break;
            default:
                break;
        }
    }
}

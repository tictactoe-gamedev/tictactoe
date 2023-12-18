using UnityEngine;

public class Voxel : MonoBehaviour
{
    private int type;
    private int amount;

    public int Type
    {
        get { return type; }
    }

    public int Amount
    {
        get { return amount; }
    }

    public void Initialize(int voxelType, int voxelAmount)
    {
        type = voxelType;
        amount = voxelAmount;
    }
}

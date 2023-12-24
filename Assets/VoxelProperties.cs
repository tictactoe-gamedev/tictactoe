using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelProperties : MonoBehaviour
{
    private Voxel.VoxelType voxelType;

    public void SetVoxelType(Voxel.VoxelType type)
    {
        voxelType = type;
        // You can add more properties or behaviors based on the voxel type if needed
        switch (voxelType)
        {
            case Voxel.VoxelType.Gold:
                // Set properties specific to Gold voxel
                break;
            case Voxel.VoxelType.Silver:
                // Set properties specific to Silver voxel
                break;
            case Voxel.VoxelType.Bronze:
                // Set properties specific to Bronze voxel
                break;
            case Voxel.VoxelType.Iron:
                // Set properties specific to Iron voxel
                break;
        }
    }
}
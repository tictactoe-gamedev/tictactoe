using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Voxel : MonoBehaviour
{
    [HideInInspector] public int blockType;
    //0 = gold
    //1 = silver
    //2 = bronze
    //3 = jello

    public int Amount { get; private set; }

    private void Start()
    {
        Amount = Random.Range(1, 10); //set random drop amount when block is broken
        
        FinecraftGod.VoxelCount++;
        
        switch (blockType)
        {
            case 0:
                FinecraftGod.GoldVoxelCount++;
                break;
            case 1:
                FinecraftGod.SilverVoxelCount++;
                break;
            case 2:
                FinecraftGod.BronzeVoxelCount++;
                break;
            case 3:
                FinecraftGod.JelloVoxelCount++;
                break;
            default:
                Debug.Log("Voxel type " + blockType + " does not exist!");
                break;
        }
    }
}

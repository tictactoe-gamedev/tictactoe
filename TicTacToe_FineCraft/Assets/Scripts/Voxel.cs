using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    [HideInInspector] public int blockType;
    /*0 = Gold
     *1 = Silver
     *2 = Bronze
     *3 = Cheese*/

    public int amount { get; private set; }

    void Start()
    {
        amount = Random.Range(0, 15);

        FinecraftGod.totalVoxelCount++;

        switch (blockType)
        {
            case 0:
                FinecraftGod.goldCount++;
                break;
            case 1:
                FinecraftGod.silverCount++;
                break;
            case 2:
                FinecraftGod.bronzeCount++;
                break;
            case 3:
                FinecraftGod.cheeseCount++;
                break;
            default:
                Debug.Log("That's not a voxel, that's " +blockType); 
                break;
        }
    }

   
}

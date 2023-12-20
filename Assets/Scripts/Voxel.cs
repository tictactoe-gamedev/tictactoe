using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    //Name whatever you want
    public string type;

    //The amount it will give you
    public int amount;

    public static int count = 0;

    //Constructor for Vox
    public Voxel(string type, int amount)
    {
        this.type = type;
        this.amount = amount;

        count++;

    }


   
}

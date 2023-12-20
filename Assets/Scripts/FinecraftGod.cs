using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{

    public GameObject prefab;

    private float timer;

    void Start()
    {
        //TypeIndex will find a random number from 0 to 3
        int typeIndex = Random.Range(0, 3);

        //Whatever number it gets will be the case
        string voxelType = GetVoxelType(typeIndex);

        int amountType = getAmountType(typeIndex);


        this.transform.position = Vector3.one * Random.Range(-10f, 10f);

    }

    
    void Update()
    {

        sphereSpawnPosition();

        timer += Time.deltaTime;

        if (timer > 5f) {

            var boxCheck = Instantiate(prefab);
            timer = 0f;

        }


    }
    
    //Function will spawn in the spheres in the given area randomly
    void sphereSpawnPosition()
    {
        var pos = transform.position;

        float rand = Random.Range(-0.1f, 0.1f);

        float rand2 = Random.Range(-0.1f, 0.1f);

        float rand3 = Random.Range(-0.1f, 0.1f);

        transform.localPosition += Time.deltaTime * new Vector3(x: pos.x + rand, y: pos.y + rand2, z: pos.z + rand3);
    }


    //Gets the type of Voxel based on the case
    string GetVoxelType(int typeIndex)
    {
        switch (typeIndex)
        {
            case 0:
                return "Gold";
            case 1:
                return "Silver";
            case 2:
                return "Bronze";
            default:
                return "NULL";
        }

    }

    //Gets the type of Amount based on the case
    int getAmountType(int typeIndex)
    {
        switch (typeIndex)
        {
            case 0:
                return 15;
            case 1:
                return 10;
            case 2:
                return 5;
            default:
                return 0;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject VoxelPrefab;
    public Vector3 WorldSize;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        GameObject voxel = Instantiate(VoxelPrefab, randomPosition, Quaternion.identity);
        voxel.GetComponent<Voxel>().Type = (Voxel.Material)Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

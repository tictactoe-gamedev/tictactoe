using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Vector3 universeSize;
    
    // Start is called before the first frame update
    void Start()
    {
        int voxelType = Random.Range(0, 4);
        Vector3 spawnPosition = new Vector3(Random.Range(-universeSize.x, universeSize.x), Random.Range(-universeSize.y, universeSize.y), Random.Range(-universeSize.z, universeSize.z));

        
        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        cube.GetComponent<Voxel>().blockType = (Voxel.BlockType)voxelType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

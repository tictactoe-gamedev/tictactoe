using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public static FinecraftGod Instance;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Vector3 universeSize;
    [SerializeField] private int maxVoxels;
    public static int VoxelCount;
    public static int GoldVoxelCount;
    public static int SilverVoxelCount;
    public static int BronzeVoxelCount;
    public static int JelloVoxelCount;
    
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        GenerateRandomVoxel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (VoxelCount < maxVoxels)
            {
                GenerateRandomVoxel();
            }
            else
            {
                Debug.Log("Max voxels reached!");
            }

            Debug.Log("gold: " + GoldVoxelCount + " silver: " + SilverVoxelCount + " bronze: " + BronzeVoxelCount + " jello: " + JelloVoxelCount);
        }
    }

    private void GenerateRandomVoxel()
    {
        int voxelType = Random.Range(0, 4);
        Vector3 spawnPosition = new Vector3(Random.Range(-universeSize.x, universeSize.x), Random.Range(-universeSize.y, universeSize.y), Random.Range(-universeSize.z, universeSize.z));

        
        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        Voxel voxel = cube.GetComponent<Voxel>();
        voxel.blockType = voxelType;
    }
}

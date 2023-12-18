using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public GameObject VoxelPrefab;
    public Vector3 WorldSize;
    public int MaxVoxelcount = 10;
    
    private int _currentVoxelCount = 0;
    private int _goldVoxelCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomVoxel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            GenerateRandomVoxel();
        }
    }

    void GenerateRandomVoxel()
    {
        if (_currentVoxelCount < MaxVoxelcount)
        {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
            GameObject voxel = Instantiate(VoxelPrefab, randomPosition, Quaternion.identity);

            int type;

            if (_goldVoxelCount < 2)
            {
                type = UnityEngine.Random.Range(0, 4);
            }
            else
            {
                type = UnityEngine.Random.Range(1, 4);
            }

            if ((Voxel.Material)type == Voxel.Material.Gold)
            {
                _goldVoxelCount++;
            }

            Debug.Log((Voxel.Material)type);

            voxel.GetComponent<Voxel>().Type = (Voxel.Material)type;
            _currentVoxelCount++;
        }
    }
}

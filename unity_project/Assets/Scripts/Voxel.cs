using System;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    //solution without SOs
    private VoxelType _voxelType;
    private int _amount;
    private int[] possibleAmounts = { 1, 5, 10 };

    //with SOs
    private VoxelData _data;
    private Material _material;
    public void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }
    public void InitVoxel(VoxelType type)
    {
        _voxelType = type;
        _amount = possibleAmounts[UnityEngine.Random.Range(0, possibleAmounts.Length)];
    }

    public void InitVoxelFromScriptableObject(VoxelData voxelData)
    {
        _data = voxelData;
        _material.color = _data.material.color;
    }
}


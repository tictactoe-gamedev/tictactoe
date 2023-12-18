using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public FinecraftGod Instance;

    [SerializeField] Voxel voxelPrefab;
    [SerializeField] int xWorldWidth;
    [SerializeField] int yWorldHeight;
    [SerializeField] int zWorldWidth;

    [SerializeField] int maxAmountOfVoxels;
    private List<Voxel> currentVoxels = new();

    public List<VoxelData> voxelDatas;
    private Dictionary<VoxelType, VoxelData> enumToObjectMap;

    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogError("There can't be two FinecraftGod! We are monotheists here");
        }

        GenerateVoxel();

        InitializeMap();
    }

    private void InitializeMap()
    {
        enumToObjectMap = new Dictionary<VoxelType, VoxelData>();

        foreach (var voxelData in voxelDatas)
        {
            enumToObjectMap.Add(voxelData.type, voxelData);
        }
    }

    public VoxelData GetVoxelDataByEnum(VoxelType enumValue)
    {
        if (enumToObjectMap.ContainsKey(enumValue))
        {
            return enumToObjectMap[enumValue];
        }

        Debug.LogError("Voxel Data not found for enum value: " + enumValue);
        return null;
    }


    void Update()
    {
        if (currentVoxels.Count <= maxAmountOfVoxels && Input.GetKeyDown(KeyCode.K))
        {
            GenerateVoxel();
        }

        if (currentVoxels.Count <= maxAmountOfVoxels && Input.GetKeyDown(KeyCode.Q))
        {
            GenerateVoxelFromScriptableObject();
        }
    }
    void GenerateVoxel()
    {
        var voxelType = (VoxelType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(VoxelType)).Length);

        Debug.Log($"AND THUS I CREATED A {voxelType.ToString().ToUpper()} VOXEL");

        var x = UnityEngine.Random.Range(-xWorldWidth, xWorldWidth + 1);
        var y = UnityEngine.Random.Range(-yWorldHeight, yWorldHeight + 1);
        var z = UnityEngine.Random.Range(-zWorldWidth, zWorldWidth + 1);

        var newVoxel = Instantiate(voxelPrefab, new Vector3(x, 0, z), Quaternion.identity);
        newVoxel.InitVoxel(voxelType);

        currentVoxels.Add(newVoxel);
    }

    void GenerateVoxelFromScriptableObject()
    {
        var voxelType = (VoxelType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(VoxelType)).Length);

        Debug.Log($"AND THUS I CREATED A {voxelType.ToString().ToUpper()} VOXEL");

        var x = UnityEngine.Random.Range(-xWorldWidth, xWorldWidth + 1);
        var y = UnityEngine.Random.Range(-yWorldHeight, yWorldHeight + 1);
        var z = UnityEngine.Random.Range(-zWorldWidth, zWorldWidth + 1);

        var newVoxel = Instantiate(voxelPrefab, new Vector3(x, 0, z), Quaternion.identity);
        newVoxel.InitVoxelFromScriptableObject(GetVoxelDataByEnum(voxelType));

        currentVoxels.Add(newVoxel);
    }
}

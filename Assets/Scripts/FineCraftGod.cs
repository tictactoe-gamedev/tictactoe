using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FineCraftGod : MonoBehaviour
{
    private Dictionary<string, List<Voxel>> _voxelRecords = new Dictionary<string, List<Voxel>>();
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _bounds;

    public int MaxNumberOfVoxels = 5;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAndRecordNewVoxelGeneration();
        int x = GetRandomPointWithinBounds(_bounds.x);
        int y = GetRandomPointWithinBounds(_bounds.y);
        int z = GetRandomPointWithinBounds(_bounds.z);
        Debug.Log($"X, Y, Z are {new Vector3(x, y, z)}");
        Debug.Log($"Bounds are {_bounds}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            /*Anything you'll write to here will be triggered
             on each press to K on your keyboard */
            if (CanGenerateNewVoxel())
            {
                GenerateAndRecordNewVoxelGeneration();
            }
        }
    }

    private int GetRandomPointWithinBounds(float boundPoint)
    {
        if (boundPoint < 0)
        {
            return Random.Range((int)Mathf.Round(_bounds.x), (int)Mathf.Round(_bounds.x * -1.0f));
        }
        else
        {
            return Random.Range((int)Mathf.Round(_bounds.x * -1), (int)Mathf.Round(_bounds.x));
        }
    }

    private Voxel GenerateRandomVoxel()
    {
        int voxelCode = Random.Range(0, 4);
        int voxelAmount = Random.Range(0, 100);
        return GetVoxelFromInt(voxelCode, voxelAmount);
    }

    private Voxel GetVoxelFromInt(int val, int amount)
    {
        return val switch
        {
            0 => new Voxel.GoldVoxel(amount),
            1 => new Voxel.SilverVoxel(amount),
            2 => new Voxel.BronzeVoxel(amount),
            3 => new Voxel.IronVoxel(amount),
            _ => throw new System.NotImplementedException()
        };
    }

    private bool CanGenerateNewVoxel()
    {
        int sum = 0;
        foreach ((var type, var voxelList) in _voxelRecords)
        {
            sum += voxelList.Count;
            Debug.Log($"[In-Check] {type}: {voxelList.Count}");
        }
        return sum < MaxNumberOfVoxels;
    }

    private void GenerateAndRecordNewVoxelGeneration()
    {
        var newVoxel = GenerateRandomVoxel();
        var key = newVoxel.GetType().ToString();
        if (_voxelRecords.TryGetValue(key, out var voxels))
        {
            voxels.Add(newVoxel);
        }
        else
        {
            _voxelRecords[key] = new List<Voxel> { newVoxel };
        }
        foreach ((var type, var voxelList) in _voxelRecords)
        {
            Debug.Log($"[Post-Gen] {type}: {voxelList.Count}");
        }
    }
}

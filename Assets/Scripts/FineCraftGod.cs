using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FineCraftGod : MonoBehaviour
{
    private Voxel _voxel;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _position;

    // Start is called before the first frame update
    void Start()
    {
        int voxelCode = Random.Range(0, -4);
        int voxelAmount = Random.Range(0, 100);
        _voxel = GetVoxelFromInt(voxelCode, voxelAmount);
        int x = Random.Range(-10, 10);
        int y = Random.Range(-10, 10);
        int z = Random.Range(-10, 10);
        _position = new Vector3(x, y, z);
        transform.position = _position;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinecraftGod : MonoBehaviour
{
    public static FinecraftGod Instance;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] int worldWidth = 3;
    [SerializeField] int worldHeight = 1;
    [SerializeField] int worldDepth = 5;

    [SerializeField] private int maxVoxelCount = 25;
    public static int totalVoxelCount;

    public static int goldCount;
    public static int silverCount;
    public static int bronzeCount;
    public static int cheeseCount;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SpawnVoxel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(totalVoxelCount < maxVoxelCount)
            {
                SpawnVoxel();
            }
            else
            {
                Debug.Log("Voxel limit reached");
            }
            
        }
    }

    private void SpawnVoxel()
    {
        int voxelType = Random.Range(0, 4);

        var x = Random.Range(-worldWidth, worldWidth);
        var y = Random.Range(-worldHeight, worldHeight);
        var z = Random.Range(-worldDepth, worldDepth);

        Vector3 spawnPos = new Vector3(x,y,z);


        GameObject block = Instantiate(cubePrefab,spawnPos,Quaternion.identity);
        Voxel voxel = block.GetComponent<Voxel>();
        voxel.blockType = voxelType;
        Debug.Log("Gold: " + goldCount + " Silver: " + silverCount + " Bronze: " + bronzeCount + " Cheese: " + cheeseCount + " TOTAL: " + totalVoxelCount);
    }
}

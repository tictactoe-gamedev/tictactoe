using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Voxel : MonoBehaviour
{
    public enum VoxelType { Gold, Silver, Bronze, Iron }

    public int worldX = 5;
    public int worldZ = 3;
    public int maxVoxelCount = 10;
    public int voxelsToCreate = 1;

    // Use a dictionary to map VoxelType to GameObject prefabs
    private Dictionary<VoxelType, GameObject> voxelPrefabs = new Dictionary<VoxelType, GameObject>();
    private Dictionary<VoxelType, int> voxelCounts = new Dictionary<VoxelType, int>();
    private List<GameObject> instantiatedVoxels = new List<GameObject>();   // store instantiated voxel game objects

    // Different types of blocks prefabs
    public GameObject goldVoxelPrefab;
    public GameObject silverVoxelPrefab;
    public GameObject bronzeVoxelPrefab;
    public GameObject ironVoxelPrefab;

    void Start()
    {
        InitializeVoxelCounts();
        InitializeVoxelPrefabs();
    }

    void Update()
    {
        // When we press the 'K' key, we create a new block, as long as not too many blocks created.
        if (Input.GetKeyDown(KeyCode.K) && voxelCounts.Values.Sum() < maxVoxelCount)
        {
            for (int i = 0; i < voxelsToCreate; i++)
            {
                GenerateVoxel();
            }
        }
    }

    void GenerateVoxel()
    {
        if (voxelCounts.Values.Sum() >= maxVoxelCount)
        {
            Debug.Log("Maximum total voxel count reached.");
            BatchMeshes(); // Call BatchMeshes function to combine meshes
            return;
        }

        List<VoxelType> availableTypes = GetAvailableVoxelTypes();

        if (availableTypes.Count == 0)
        {
            Debug.Log("Maximum count reached for all voxel types.");
            return;
        }

        // randomly choose a type of block that we can create.
        VoxelType newType = GetRandomVoxelType(availableTypes);

        // If we've already made too many of this type of block, we stop and show a message
        if (voxelCounts[newType] >= maxVoxelCount / System.Enum.GetNames(typeof(VoxelType)).Length)
        {
            Debug.Log("Maximum count reached for " + newType.ToString() + " voxel.");
            return;
        }

        // decide where to put the block in the world and create it there.
        int posX = Random.Range(-worldX, worldX + 1);
        int posY = 1;
        int posZ = Random.Range(-worldZ, worldZ + 1);

        // perform frustum culling here before creating the voxel
        if (!IsInFrustum(new Vector3(posX, posY, posZ)))
        {
            return; // Skip instantiation if the voxel is not in the frustum
        }

        // keep track of how many blocks of each type we've created.
        voxelCounts[newType]++;
        GameObject newVoxel = Instantiate(voxelPrefabs[newType], new Vector3(posX, posY, posZ), Quaternion.identity);
        instantiatedVoxels.Add(newVoxel);

        // Check if we've reached the maximum count before attempting to batch
        if (voxelCounts.Values.Sum() >= maxVoxelCount)
        {
            Debug.Log("Maximum total voxel count reached.");
            BatchMeshes(); // Call BatchMeshes function to combine meshes
        }
    }

    bool IsInFrustum(Vector3 position)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Check if the position is within the view frustum
        return GeometryUtility.TestPlanesAABB(planes, new Bounds(position, Vector3.zero));
    }

    void InitializeVoxelCounts()
    {
        foreach (VoxelType type in System.Enum.GetValues(typeof(VoxelType)))
        {
            voxelCounts[type] = 0;
        }
    }

    void InitializeVoxelPrefabs()
    {
        voxelPrefabs[VoxelType.Gold] = goldVoxelPrefab;
        voxelPrefabs[VoxelType.Silver] = silverVoxelPrefab;
        voxelPrefabs[VoxelType.Bronze] = bronzeVoxelPrefab;
        voxelPrefabs[VoxelType.Iron] = ironVoxelPrefab;
    }

    List<VoxelType> GetAvailableVoxelTypes()
    {
        return voxelCounts
            .Where(pair => pair.Value < maxVoxelCount / System.Enum.GetNames(typeof(VoxelType)).Length)
            .Select(pair => pair.Key)
            .ToList();
    }

    VoxelType GetRandomVoxelType(List<VoxelType> availableTypes)
    {
        int randomIndex = Random.Range(0, availableTypes.Count);
        return availableTypes[randomIndex];
    }

    void BatchMeshes()
    {
        // Combine instantiated voxel meshes into a single mesh
        if (instantiatedVoxels.Count > 0)
        {
            CombineInstance[] combine = new CombineInstance[instantiatedVoxels.Count];
            for (int i = 0; i < instantiatedVoxels.Count; i++)
            {
                MeshFilter meshFilter = instantiatedVoxels[i].GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    combine[i].mesh = meshFilter.sharedMesh;
                    combine[i].transform = meshFilter.transform.localToWorldMatrix;
                }
            }

            // Create a new combined mesh
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);

            // Assign the combined mesh to a GameObject
            GameObject combinedObject = new GameObject("CombinedVoxelMesh");
            combinedObject.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
            combinedObject.AddComponent<MeshRenderer>();

            instantiatedVoxels.Clear(); // Clear the list of instantiated voxel GameObjects
            instantiatedVoxels.Add(combinedObject); // Add the new combined voxel GameObject
        }
    }
}
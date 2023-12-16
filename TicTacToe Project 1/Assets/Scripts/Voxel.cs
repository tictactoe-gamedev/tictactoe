using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Voxel : MonoBehaviour
{
    public enum BlockType
    {
        Gold, Silver, Bronze, Jello
    }

    [HideInInspector] public BlockType blockType;

    public int Amount { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

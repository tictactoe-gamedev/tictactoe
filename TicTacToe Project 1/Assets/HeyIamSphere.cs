using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeyIamSphere : MonoBehaviour
{
    [SerializeField]private float speed = 1f;

    public static int SphereCount;
    private void Start()
    {
        SphereCount++;
        this.transform.position = Vector3.one * Random.Range(-10f, 10f);
    }
    
    void Update()
    {
        var pos = transform.position;

        float Rand = Random.Range(-1f, 1f);
        float Rand2 = Random.Range(-1f, 1f);
        float Rand3 = Random.Range(-1f, 1f);
        

        transform.position += new Vector3(pos.x + Rand, pos.y + Rand2, pos.z + Rand3) * (Time.deltaTime * speed);
    }
}

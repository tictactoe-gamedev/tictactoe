using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject g;

    
    void Start()
    {
        AClass a = new AClass();
        a.MyName = "AAA";
        AClass b = new AClass();
        b.MyName = "BBB";
        AClass c = new AClass();
        c.MyName = "CCC";
        
        Debug.Log("Names are: " + a.MyName + " " + b.MyName + " " + c.MyName);
        Debug.Log("Count is: " + AClass.Count);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Instantiated Object Count is: " + AClass.Count);
    }
}

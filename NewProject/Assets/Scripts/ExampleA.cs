using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ExampleA : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Some Comment
        int age = 40; 
        
        Debug.Log("My age is : " + age);

        AnotherClass a = new AnotherClass();

        a.AnotherAge = 20;
        
        a.Something();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

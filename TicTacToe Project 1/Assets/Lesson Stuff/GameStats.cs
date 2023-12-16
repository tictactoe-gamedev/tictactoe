using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStats : MonoBehaviour
{

    public static GameStats Instance;

    public int OnlinePlayers = 10;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Debug.Log("Bullet Count: " + HeyIamSphere.SphereCount);
    }
}

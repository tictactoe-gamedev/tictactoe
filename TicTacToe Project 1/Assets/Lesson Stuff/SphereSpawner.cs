using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject prefab;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1f)
        {
            var sphere = Instantiate(prefab);
            timer = 0f;
            Debug.Log(GameStats.Instance.OnlinePlayers);
        }
    }
}

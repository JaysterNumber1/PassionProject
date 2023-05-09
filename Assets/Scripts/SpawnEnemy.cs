using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public int numObjects = 10;
    public GameObject prefab;
    public GameObject player;
    public float radius = 10f;
    private float timertime;
    public float timerlength = 100;

    void Start()
    {
        spawn();
    }

    private void Update()
    {
        timertime += Time.deltaTime;
        
        if(timertime > timerlength)
        {
            timertime = 0;
            if (timerlength > 0.75) { timerlength /= 1.1f; } else { timerlength = 0.75f; }
            spawn();
            Debug.Log(timerlength);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
    public void spawn()
    {
        Vector3 center = player.transform.position;
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 pos = RandomCircle(center, radius);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}

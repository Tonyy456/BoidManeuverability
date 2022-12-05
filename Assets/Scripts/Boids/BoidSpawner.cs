using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By: Taylor Liu
public class BoidSpawner : MonoBehaviour
{
    public List<Vector3> spawnPoints;
    [SerializeField] GameObject boid;
    public int boidNum;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boidNum; i++) {
            GameObject one = Instantiate(boid);
            one.transform.position = spawnPoints[i % spawnPoints.Count];
        }
    }
}

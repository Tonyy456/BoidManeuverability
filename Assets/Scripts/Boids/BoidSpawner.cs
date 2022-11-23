using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public List<Vector3> spawnPoints;
    [SerializeField] GameObject boid;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 sp in spawnPoints) {
            GameObject one = Instantiate(boid);
            one.transform.position = sp;
        }
    }
}

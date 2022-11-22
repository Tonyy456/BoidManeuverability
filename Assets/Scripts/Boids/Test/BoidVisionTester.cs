using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidVisionTester : MonoBehaviour
{
    [SerializeField] private int resolution;
    [SerializeField] private float angle;
    [SerializeField] private float distance;
    BoidVision bv;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        bv = new BoidVision(transform.position, transform.forward, angle);
        bv.RayCast(resolution, distance);
        bv.DrawRays();
    }
}

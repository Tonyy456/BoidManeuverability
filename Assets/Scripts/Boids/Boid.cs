using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Other boids and paths")]
    public List<Transform> closeBoids;
    public List<Vector3> openPaths;
    public List<RaycastHit> obstructedPaths;
    public Rigidbody thisBoid;

    [Header("Individual Specs")]
    public Vector3 turnTowards;
    public float speed = 3f;
    public float rotSpeed = 15f;
    public int resolution = 375;
    public float distance = 5f;

    public void Start() {
        thisBoid = GetComponent<Rigidbody>();
    }

    private void Update() {
        var bv = new BoidVision(transform.position, transform.forward, 360);
        bv.RayCast(resolution, distance);
        bv.DrawRays();

        closeBoids = bv.BoidHits;
        openPaths = bv.MissedRays;
        obstructedPaths = bv.Hits;

        //Rotations
        if (closeBoids.Count > 0) {
            turnTowards = Avoid();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(turnTowards), rotSpeed * Time.deltaTime);
        }

        //Positions
        BoundPosition();
        thisBoid.velocity = transform.forward * speed;
    }

    private Vector3 Avoid() {
        Vector3 turnDir = new Vector3();

        foreach (Transform x in closeBoids) {
            turnDir += transform.position - x.transform.position;
        }

        turnDir = turnDir.normalized;

        return turnDir;
    }

    private void BoundPosition() {
        if (transform.position.z > 49f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 1.5f);
        if (transform.position.z < 0.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 49f);
        if (transform.position.y > 49f)
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        if (transform.position.y < 0.5f)
            transform.position = new Vector3(transform.position.x, 49f, transform.position.z);
        if (transform.position.x > 9.5f)
            transform.position = new Vector3(1.5f, transform.position.y, transform.position.z);
        if (transform.position.x < 0.5f)
            transform.position = new Vector3(8.5f, transform.position.y, transform.position.z);
    }
}

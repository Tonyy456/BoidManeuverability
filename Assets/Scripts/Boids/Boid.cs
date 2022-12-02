using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Other boids and paths")]
    public List<Transform> closeBoids;
    public List<Vector3> openPaths;
    public List<RaycastHit> obstructedPaths;

    [Header("Individual Specs")]
    public float speed = 3f;
    public float rotSpeed = 15f;
    public int resolution = 375;
    public float distance = 5f;
    public float cohesionFactor = 1f;

    [Header("Leader Specs")]
    public bool leader = false;
    public bool avoid = false;
    public bool align = false;
    public bool cohesion = false;

    [Header("Boundaries")]
    public float xMin = 1f;
    public float xMax = 10f;
    public float yMin = 1f;
    public float yMax = 49f;
    public float zMin = 1f;
    public float zMax = 49f;

    private Vector3 turnTowards;
    private bool wandering = false;
    private Vector3 wanderTo;

    private void Update() {
        var bv = new BoidVision(transform.position, transform.forward, 360);
        bv.RayCast(resolution, distance);
        bv.DrawRays();

        closeBoids = bv.BoidHits;
        openPaths = bv.MissedRays;
        obstructedPaths = bv.Hits;

        //Rotations
        if (closeBoids.Count > 0) {
            turnTowards = Avoid() + Align() + (Cohesion() * cohesionFactor);
        } else {
            turnTowards = Wander();
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(turnTowards), rotSpeed * Time.deltaTime);

        //Positions
        BoundPosition();
        transform.position += transform.forward.normalized * speed * Time.deltaTime;
    }

    //Avoid objects that get too close
    private Vector3 Avoid() {
        Vector3 turnDir = new Vector3();

        //Avoid other boids
        foreach (Transform x in closeBoids) {
            if (Vector3.Distance(transform.position, x.transform.position) < 4f) {
                turnDir += transform.position - x.transform.position;
            }
        }

        //TODO: Avoid obstacles and walls

        turnDir.Normalize();
        Debug.DrawRay(transform.position, turnDir * 5f, Color.green);

        return turnDir;
    }

    //Align with the average look direction of other close boids
    private Vector3 Align() {
        Vector3 turnDir = new Vector3();

        foreach (Transform x in closeBoids) {
            turnDir += x.transform.forward;
        }

        turnDir /= closeBoids.Count;
        Debug.DrawRay(transform.position, turnDir * 5f, Color.red);

        return turnDir;
    }

    //Try and move towards the average position of closeboids
    private Vector3 Cohesion() {
        Vector3 turnDir = new Vector3();
        Vector3 avgPos = new Vector3();

        foreach (Transform x in closeBoids) {
            avgPos += x.transform.position;
        }
        avgPos /= closeBoids.Count;

        turnDir = avgPos - transform.position;
        turnDir.Normalize();
        Debug.DrawRay(transform.position, turnDir * 5f, Color.blue);

        return turnDir;
    }

    //TODO: Wander(). If there are no boids close, then wander and not just move straight
    private Vector3 Wander() {
        if (!wandering) {
            wandering = true;
            wanderTo = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        } 
        
        Vector3 turnDir = wanderTo - transform.position;
        Debug.Log(turnDir);

        if (turnDir.x < 0.5f && turnDir.y < 0.5f && turnDir.z < 0.5f) {
            wandering = false;
        }

        Debug.DrawRay(transform.position, turnDir, Color.yellow);

        return turnDir;
    }

    //Keeps boids in a certain area using teleportation
    private void BoundPosition() {
        if (transform.position.z > zMax + 0.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMin + 0.5f);
        if (transform.position.z < zMin - 0.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax - 0.5f);
        if (transform.position.y > yMax + 0.5f)
            transform.position = new Vector3(transform.position.x, yMin + 0.5f, transform.position.z);
        if (transform.position.y < yMin - 0.5f)
            transform.position = new Vector3(transform.position.x, yMax - 0.5f, transform.position.z);
        if (transform.position.x > xMax + 0.5f)
            transform.position = new Vector3(xMin + 0.5f, transform.position.y, transform.position.z);
        if (transform.position.x < xMin - 0.5f)
            transform.position = new Vector3(xMax - 0.5f, transform.position.y, transform.position.z);
    }
}

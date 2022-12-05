using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By: Taylor Liu
//Sources: In class boids lecture slides
//         https://youtu.be/bqtqltqcQhw
public class Boid : MonoBehaviour
{
    [Header("Other boids and paths")]
    public List<Transform> closeBoids;
    public List<Vector3> openPaths;
    public List<RaycastHit> obstructedPaths;

    [Header("Individual Specs")]
    public float speed = 3f;
    public float baseRotSpeed = 15f;
    public float maxRotSpeed = 90f;
    public int resolution = 375;
    public float distance = 10f;
    public float cohesionFactor = 1f;
    public float avoidCollisionWeight = 2f;

    [SerializeField] private float AlignM = 2f;
    [SerializeField] private float AvoidM = 2f;
    [SerializeField] private float AVoidWallM = 2f;
    [SerializeField] private float CohesionM = 2f;

    private Vector3 turnTowards;

    private void Update() {
        var bv = new BoidVision(transform.position, transform.forward, 360);
        bv.RayCast(resolution, distance);
        bv.DrawRays();

        closeBoids = bv.BoidHits;
        openPaths = bv.MissedRays;
        obstructedPaths = bv.Hits;

        //Rotations
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance)) {
            turnTowards = AvoidWalls() * AVoidWallM + AvoidBoids() * AvoidM + Align() * AlignM + (Cohesion() * CohesionM  / cohesionFactor);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(turnTowards), TurnSpeed() * Time.deltaTime);
            if (hit.collider.tag != "Soid")
                transform.position += transform.forward.normalized * (speed * (hit.distance / (distance * 1.5f))) * Time.deltaTime;
            else
                transform.position += transform.forward.normalized * speed * Time.deltaTime;
        } else {
            turnTowards = Wander() / 4f + AvoidBoids() * AVoidWallM + Align() * AlignM + (Cohesion() * CohesionM / cohesionFactor);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(turnTowards), TurnSpeed() * Time.deltaTime);
            transform.position += transform.forward.normalized * speed * Time.deltaTime;
        }   
    }

    //Avoid other close boids
    private Vector3 AvoidBoids() {
        Vector3 turnDir = Vector3.zero;

        //Avoid other boids
        foreach (Transform x in closeBoids) {
            if (Vector3.Distance(transform.position, x.transform.position) < 4f) {
                turnDir += transform.position - x.transform.position;
            }
        }
        turnDir.Normalize();

        Debug.DrawRay(transform.position, turnDir * distance, Color.green);

        return turnDir;
    }

    //Avoid walls and obstacles
    private Vector3 AvoidWalls() {
        Vector3 bestPath = transform.forward;
        if (openPaths.Count > 0) {
            bestPath = openPaths[0];

            //Avoid walls
            foreach (Vector3 v in openPaths) {
                bestPath = Vector3.Distance(v, transform.forward) < Vector3.Distance(bestPath, transform.forward) ? v : bestPath;
            }
            Debug.DrawRay(transform.position, bestPath.normalized * distance, Color.cyan);
        }

        return bestPath;
    }

    //Align with the average look direction of other close boids
    private Vector3 Align() {
        Vector3 turnDir = Vector3.zero;

        foreach (Transform x in closeBoids) {
            turnDir += x.transform.forward;
        }

        if (closeBoids.Count > 0)
            turnDir /= closeBoids.Count;
        Debug.DrawRay(transform.position, turnDir * distance, Color.red);

        return turnDir;
    }

    //Try and move towards the average position of closeboids
    private Vector3 Cohesion() {
        Vector3 avgPos = Vector3.zero;

        if (closeBoids.Count > 0) {
            foreach (Transform x in closeBoids) {
                avgPos += x.transform.position;
            }
            avgPos /= closeBoids.Count;
        }

        Debug.DrawRay(transform.position, (avgPos - transform.position).normalized * distance, Color.blue);

        return avgPos - transform.position;
    }

    //Wander so less strict following
    private Vector3 Wander() {
        /*
        var rand = Random.Range(0, openPaths.Count);
        Debug.DrawRay(transform.position, openPaths[rand].normalized * distance, Color.cyan);
        return openPaths[rand];
        */
        return new Vector3(0,0,0.0001f);
    }

    //Changes turn speed based on how close the wall is
    private float TurnSpeed() {
        float minDist = 10f;

        foreach(RaycastHit hit in obstructedPaths) {
            minDist = Mathf.Min(minDist, hit.distance);
        }

        var speed = (baseRotSpeed * (distance / minDist));

        return Mathf.Clamp(speed, baseRotSpeed, maxRotSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written By: Tony D'Alesandro
 */
public class BoidVision
{
    private Vector3 center;
    private Vector3 facing;
    private float angle;

    private List<Transform> boids = new List<Transform>();
    private List<Vector3> missedRays = new List<Vector3>();
    private List<RaycastHit> hits = new List<RaycastHit>();
    private List<Vector3> rays = new List<Vector3>();
    public BoidVision(Vector3 position, Vector3 facing, float angle)
    {
        this.center = position;
        this.facing = facing;
        this.angle = angle;
    }

    public List<Transform> BoidHits
    {
        get
        {
            return boids;
        }
    }

    public List<Vector3> MissedRays
    {
        get
        {
            return missedRays;
        }
    }

    public List<RaycastHit> Hits
    {
        get
        {
            return hits;
        }
    }

    //https://stackoverflow.com/questions/9600801/evenly-distributing-n-points-on-a-sphere/44164075#44164075
    public void RayCast(int resolution, float distance)
    {       
        for(int i = 0; i < resolution; i++)
        {
            float index = i + 0.5f;
            float phi = Mathf.Acos(1 - 2 * index / resolution);
            float theta = Mathf.PI * (1 + Mathf.Pow(5f, 0.5f)) * index;
            Vector3 rayEnd = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi));
            rayEnd *= distance;
            rayEnd += center;
            Vector3 rayDirection = rayEnd - center;

            rays.Add(rayDirection);
            if (Vector3.Angle(rayDirection, rayDirection) > angle) continue;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(center, rayDirection, out hit, distance))
            {
                hits.Add(hit);
                Transform transform = hit.transform;
                if (transform.gameObject.GetComponent<Boid>() != null) {
                    boids.Add(transform);
                } 
            }
            else
            {
                missedRays.Add(rayDirection);
            }
        }
    }

    public List<Vector3> GetRayCasts()
    {
        return rays;
    }

    public void DrawRays()
    {
        foreach(Vector3 rayDirection in rays)
        {
            //Debug.DrawRay(center, rayDirection, Vector3.Angle(facing, rayDirection) < angle ? Color.green : Color.red);
        }
    }
}

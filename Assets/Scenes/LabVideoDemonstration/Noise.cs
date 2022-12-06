using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public float frequency;
    public Vector3Int points;
    public Vector3 center;
    public float seperation;
    public Transform parent;


    private List<GameObject> createdPoints = new List<GameObject>();

    public void Start()
    {
        CreatePoints();
    }
    public void OnValidate()
    {
        if(Application.isPlaying)
        {
            VisualizePoints();
        }
    }

    private void CreatePoints()
    {
        Vector3 gridDimensions = seperation * (Vector3)points;
        Vector3 start = center - gridDimensions / 2f;
        Vector3[,,] pointPositions = new Vector3[points.x, points.y, points.z];
        for (int i = 0; i < points.x; i++)
        {
            for (int j = 0; j < points.y; j++)
            {
                for (int k = 0; k < points.z; k++)
                {
                    pointPositions[i, j, k] = start + new Vector3(i, j, k) * seperation;
                }
            }
        }
        foreach (var v in pointPositions)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.3f);
            go.transform.parent = parent;
            createdPoints.Add(go);
            go.transform.position = v;
        }
    }

    private void VisualizePoints()
    {
        foreach(var point in createdPoints)
        {
            float surfaceVal = PerlinNoise.Noise3D(point.transform.position, frequency);
            point.GetComponent<MeshRenderer>().material.color = new Color(surfaceVal, surfaceVal, surfaceVal, 0.3f);
        }

    }
}

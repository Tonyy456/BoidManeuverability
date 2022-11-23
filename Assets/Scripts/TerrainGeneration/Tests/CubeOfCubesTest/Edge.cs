using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector3 position
    {
        get
        {
            return (start.Position + end.Position) / 2f;
        }
    }
    public int MeshIndex { get; set; } = 0;
    private GameObject go;
    private Vertex start;
    private Vertex end;

    public Edge(Vertex vertex1, Vertex vertex2, int meshIndex = 0)
    {
        start = vertex1;
        end = vertex2;
        this.MeshIndex = meshIndex;
    }

    public void DrawEdge(float duration)
    {
        Debug.DrawLine(start.Position, end.Position, Color.red, duration);
    }

    public void ToggleEdgeCirlce(float circleScale = 1)
    {
        if (go == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = new Vector3(circleScale, circleScale, circleScale);
            go.transform.position = position;
            go.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GameObject.Destroy(go);
        }
    }
}

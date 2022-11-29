using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCCube
{
    private Vertex[] vertices;
    private Edge[] edges;

    private List<int> triangles;
    public MCCube(Vertex[] vertices, Edge[] edges)
    {
        this.vertices = vertices;
        this.edges = edges;
        CheckBuild();
    }


    public bool IsSimilar(Vector3 v1, Vector3 v2)
    {
        Vector3 vn1 = v1.normalized;
        Vector3 vn2 = v2.normalized;
        if (!Mathf.Approximately(vn1.x, vn2.x)) return false;
        if (!Mathf.Approximately(vn1.y, vn2.y)) return false;
        if (!Mathf.Approximately(vn1.z, vn2.z)) return false;
        return true;
    }

    public void March()
    {
        int cubeIdx = 0;
        if (vertices[0].IsOn) cubeIdx += 1;
        if (vertices[1].IsOn) cubeIdx += 2;
        if (vertices[2].IsOn) cubeIdx += 4;
        if (vertices[3].IsOn) cubeIdx += 8;
        if (vertices[4].IsOn) cubeIdx += 16;
        if (vertices[5].IsOn) cubeIdx += 32;
        if (vertices[6].IsOn) cubeIdx += 64;
        if (vertices[7].IsOn) cubeIdx += 128;

        triangles = new List<int>();
        for (int i = 0; TriangulationTable.triangulation[cubeIdx, i] != -1; i += 3)
        {
            Edge idx1 = edges[TriangulationTable.triangulation[cubeIdx, i]];
            triangles.Add(idx1.Index);
            Edge idx2 = edges[TriangulationTable.triangulation[cubeIdx, i + 1]];
            triangles.Add(idx2.Index);
            Edge idx3 = edges[TriangulationTable.triangulation[cubeIdx, i + 2]];
            triangles.Add(idx3.Index);
        }
    }

    public List<int> getTriangles()
    {
        return triangles;
    }

    public void CheckBuild()
    {
        List<string> errorMessages = new List<string>();
        bool errorFound = false;

        Vector3 center = Vector3.zero;
        foreach(Vertex v in vertices)
        {
            center += v.Position;
        }
        center /= 8f;

        for (int i = 0; i < 8; i++)
        {
            if (!IsSimilar(vertices[i].Position - center,TriangulationTable.VertexToRPosition[i]))
            {
                errorMessages.Add($"vertex {i} not in position: {vertices[i].Position - center}, should be {TriangulationTable.VertexToRPosition[i]}");
                errorFound = true;
            }
        }

        for (int i = 0; i < 12; i++)
        {
            if (!IsSimilar(edges[i].Position - center, TriangulationTable.EdgeToPosition[i]))
            {
                errorMessages.Add($"edge {i} not in position: {edges[i].Position - center}, should be {TriangulationTable.EdgeToPosition[i]}");
                errorFound = true;
            }
        }


        if (!errorFound) return;

        //foreach (string var in errorMessages)
            //Debug.Log(var);
        //throw new System.Exception("Not a valid cube definition");
    }

    public void DispVertices()
    {
        foreach(Vertex v in vertices)
        {
            if(v.IsOn)
                PlaceCircle(v.Position, Color.white, "vOn");
            else
                PlaceCircle(v.Position, Color.black, "vOff");
        }
    }

    private GameObject PlaceCircle(Vector3 position, Color c, string name)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.GetComponent<MeshRenderer>().material.color = c;
        go.transform.position = position;
        return go;
    }
}

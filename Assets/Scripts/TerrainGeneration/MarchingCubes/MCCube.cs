using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCCube
{
    public Vector3[] vertices { get; set; }
    public bool[] status { get; set; }
    public Vector3[] edges { get; set; }
    public int[] edgeIndex { get; set; }
    public List<int> triangles { get; set; }

    public MCCube(Vector3[] vertices, bool[] vStatus, Vector3[] edges, int[] edgeIndex)
    {
        this.vertices = vertices;
        this.edges = edges;
        this.status = vStatus;
        this.edgeIndex = edgeIndex;
        if (vertices.Length != 8) throw new System.Exception($"Vertex count is not correct: {vertices.Length} != 8");
        if (status.Length != 8) throw new System.Exception($"Vertex count is not correct: {status.Length} != 8");
        if (edges.Length != 12) throw new System.Exception($"Edge count is not correct: {edges.Length} != 12");
        if (edgeIndex.Length != 12) throw new System.Exception($"Edge indices count is not correct: {edgeIndex.Length} != 12");
        CheckBuild();
    }


    public void March()
    {
        int cubeIdx = 0;
        if (status[0]) cubeIdx += 1;
        if (status[1]) cubeIdx += 2;
        if (status[2]) cubeIdx += 4;
        if (status[3]) cubeIdx += 8;
        if (status[4]) cubeIdx += 16;
        if (status[5]) cubeIdx += 32;
        if (status[6]) cubeIdx += 64;
        if (status[7]) cubeIdx += 128;

        triangles = new List<int>();
        for (int i = 0; TriangulationTable.triangulation[cubeIdx, i] != -1; i += 3)
        {
            int idx1 = TriangulationTable.triangulation[cubeIdx, i];
            triangles.Add(edgeIndex[idx1]);
            int idx2 = TriangulationTable.triangulation[cubeIdx, i + 1];
            triangles.Add(edgeIndex[idx2]);
            int idx3 = TriangulationTable.triangulation[cubeIdx, i + 2];
            triangles.Add(edgeIndex[idx3]);
        }
    }


    private bool IsSimilar(Vector3 v1, Vector3 v2)
    {
        Vector3 vn1 = v1.normalized;
        Vector3 vn2 = v2.normalized;
        if (!Mathf.Approximately(vn1.x, vn2.x)) return false;
        if (!Mathf.Approximately(vn1.y, vn2.y)) return false;
        if (!Mathf.Approximately(vn1.z, vn2.z)) return false;
        return true;
    }

    private void CheckBuild()
    {
        List<string> errorMessages = new List<string>();
        bool errorFound = false;

        Vector3 center = Vector3.zero;
        foreach(Vector3 v in vertices)
        {
            center += v;
        }
        center /= 8f;

        for (int i = 0; i < 8; i++) {
            if (!IsSimilar(vertices[i] - center,TriangulationTable.VertexToRPosition[i])) {
                errorMessages.Add($"vertex {i} not in position: {vertices[i] - center}, should be {TriangulationTable.VertexToRPosition[i]}");
                errorFound = true;
            }
        }

        for (int i = 0; i < 12; i++) {
            if (!IsSimilar(edges[i] - center, TriangulationTable.EdgeToPosition[i])) {
                errorMessages.Add($"edge {i} not in position: {edges[i] - center}, should be {TriangulationTable.EdgeToPosition[i]}");
                errorFound = true;
            }
        }

        if (!errorFound) return;
    }
}

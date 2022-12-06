using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Only purpose is to take all the vertices and edge for a cube and 
 * return the triangles that should be created for the mesh.
 */
 /* 
  * Written By: Tony D'Alesandro
  */
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
}

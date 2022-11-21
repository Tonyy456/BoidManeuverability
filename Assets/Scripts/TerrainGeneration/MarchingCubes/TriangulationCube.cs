using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangulationCube
{
    //boolean arrays signifying if they are on
    public int[] vertices { get; set; }
    public bool needsUpdated { get; set; } = false;

    /*
     * vertexLocations specify where the cube's vertexes are located.
     * edgePoints is the mid point for each edge of the cube.
     * used for quick access.
     */
    private Vector3[] vertexLocations { get; set; } =
    {
        new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(-0.5f,-0.5f,0.5f),
        new Vector3(-0.5f,0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f),
    };
    private Vector3[] edgePoints { get; set; } =
    {
        new Vector3(0f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,0f), new Vector3(0f,-0.5f,0.5f), new Vector3(-0.5f,-0.5f,0f),
        new Vector3(0f,0.5f,-0.5f), new Vector3(0.5f,0.5f,0f), new Vector3(0f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0f),
        new Vector3(-0.5f,0,-0.5f), new Vector3(0.5f,0,-0.5f), new Vector3(0.5f,0,0.5f), new Vector3(-0.5f,0,0.5f)
    };

    public TriangulationCube(Vector3 position, float size)
    {
        vertices = new int[8];
        for (int i = 0; i < 8; i++)
        {
            vertexLocations[i] *= size;
            vertexLocations[i] += position;
        }

        for(int i = 0; i < 12; i++)
        {
            edgePoints[i] *= size;
            edgePoints[i] += position;
        }
    }

    public void SetVertices(float[] surfaceValues, float threshold)
    {
        needsUpdated = true;        
        for (int i = 0; i < 8; i++)
        {
            vertices[i] = surfaceValues[i] > threshold ? 1 : 0;
        }
    }

    public void SetVertices(int[] status)
    {
        needsUpdated = true;
        for(int i = 0; i < 8; i++)
        {
            vertices[i] = status[i];
        }
    }

    public void SetVertex(int index, int status)
    {
        needsUpdated = true;
        vertices[index] = status;
    }

    public Vector3 getVertexLocation(int index)
    {
        return vertexLocations[index];
    }

    public Mesh CreateMesh()
    { //used only for testing purposes
        needsUpdated = false;
        Mesh mesh = new Mesh();
        mesh.vertices = edgePoints;

        int cubeIdx = 0;
        if (vertices[0] == 1) cubeIdx += 1;
        if (vertices[1] == 1) cubeIdx += 2;
        if (vertices[2] == 1) cubeIdx += 4;
        if (vertices[3] == 1) cubeIdx += 8;
        if (vertices[4] == 1) cubeIdx += 16;
        if (vertices[5] == 1) cubeIdx += 32;
        if (vertices[6] == 1) cubeIdx += 64;
        if (vertices[7] == 1) cubeIdx += 128;

        int edge = TriangulationTable.edges[cubeIdx];

        List<int> indices = new List<int>();
        for(int i = 0; TriangulationTable.triangulation[cubeIdx, i] != -1; i+=3)
        {
            indices.Add(TriangulationTable.triangulation[cubeIdx, i]);
            indices.Add(TriangulationTable.triangulation[cubeIdx, i + 1]);
            indices.Add(TriangulationTable.triangulation[cubeIdx, i + 2]);
        }

        indices.Reverse(); //why are meshes in clock wise winding order now?!?! or am I bad at coding...
        mesh.triangles = indices.ToArray();

        mesh.RecalculateNormals();
        return mesh;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cube
{
    public Vector3 center;
    public float height;
    public Cube(Vector3 center, float height)
    {
        this.height = height;
        this.center = center;
    }

    public Vector3[] getVerticesForCube(int x, int y, int z)
    {
        Vector3[] cubeVertices = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            Vector3 unitPosition = vertexPositions[i];
            unitPosition *= (height);
            unitPosition += center;
            cubeVertices[i] = unitPosition;
        }
        return cubeVertices;
    }

    /*
     * Get the edges in a list in the correct way defined at the top of this file.
     */
    public Vector3[] getEdgesForCube(int x, int y, int z)
    {
        Vector3[] edges = new Vector3[12];
        for (int i = 0; i < 12; i++)
        {
            Vector3 unitPosition = edgePosition[i];
            unitPosition *= (height / 2f);
            unitPosition += center;
            edges[i] = unitPosition;
        }
        return edges;
    }

    private static Vector3[] vertexPositions = new Vector3[8]
    {
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f)
    };

    private static Vector3[] edgePosition = new Vector3[12]
    {
        new Vector3(0, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, 0),
        new Vector3(0, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0),

        new Vector3(0, 0.5f, -0.5f),
        new Vector3(0.5f, 0.5f, 0),
        new Vector3(0, 0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f, 0),

        new Vector3(-0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(-0.5f, 0, 0.5f)
    };
}

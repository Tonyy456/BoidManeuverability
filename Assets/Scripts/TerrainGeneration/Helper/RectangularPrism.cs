using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *          4--------5             Y
 *         /|       /|             |    Z
 *        / |      / |             |   /
 *       7--------6  |             |  /
 *       |  |     |  |             | /
 *       |  0-----|--1             |/
 *       | /      | /              +--------X
 *       |/       |/
 *       3--------2
 *       
 *           +-----4------+
 *          /|           /|
 *         7 |          5 |          Y
 *        /  |         /  |          |    
 *       +------6-----+   |          |   Z
 *       |   |        |   9          |  /
 *       |   8        |   |          | /
 *       |   |        10  |          |/
 *       11  |        |   |          +--------X
 *       |   +-----0--|---+           
 *       |  /         |  /
 *       | 3          | 1
 *       |/           |/
 *  -->  +------2-----+
 *  |     
 *  | most negative vertex    
 *  |  
 *  
 *  Cube defines a cube at its center with a specific height.
 *  Vertices and edges are returned in an array in the above manner.
 */
 /* 
  * Written By: Tony D'Alesandro
  */
public struct RectangularPrism
{
    public Vector3 center;
    public Vector3 dimensions;
    public RectangularPrism(Vector3 center, Vector3 dimensions)
    {
        this.dimensions = dimensions;
        this.center = center;
    }

    public Vector3[] getVerticesForCube()
    {
        Vector3[] cubeVertices = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            Vector3 unitPosition = vertexPositions[i];
            unitPosition.Scale(dimensions);
            unitPosition += center;
            cubeVertices[i] = unitPosition;
        }
        return cubeVertices;
    }

    public Vector3[] getEdgesForCube()
    {
        Vector3[] edges = new Vector3[12];
        for (int i = 0; i < 12; i++)
        {
            Vector3 unitPosition = edgePosition[i];
            unitPosition.Scale(dimensions);
            unitPosition += center;
            edges[i] = unitPosition;
        }
        return edges;
    }

    public (Vector3 center, Vector3 scale)[] getSurfaces()
    {
        (Vector3 center, Vector3 scale)[] edges = new (Vector3 center, Vector3 scale)[6];
        for (int i = 0; i < 6; i++)
        {
            Vector3 surfaceCenter = Vector3.zero;
            for (int j = 0; j < 4; j++)
            {
                int vertex = surfaceIndexToVertices[4 * i + j];
                surfaceCenter += Vector3.Scale(vertexPositions[vertex], dimensions) + center;
            }
            surfaceCenter /= 4f;
            edges[i] = (surfaceCenter, Vector3.Scale(surfaceDimensions[i], dimensions));
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
        new Vector3(0, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, 0),
        new Vector3(0, -0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, 0),

        new Vector3(0, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0),
        new Vector3(0, 0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, 0),

        new Vector3(-0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, -0.5f),
        new Vector3(-0.5f, 0, -0.5f)
    };

    private static Vector3[] surfaceDimensions = new Vector3[6]
    {
        new Vector3(0,1,1),
        new Vector3(1,1,0),
        new Vector3(0,1,1),
        new Vector3(1,1,0),
        new Vector3(1,0,1),
        new Vector3(1,0,1)
    };

    private static int[] surfaceIndexToVertices = new int[24]
    {
        7,4,0,3,
        4,0,1,5,
        5,6,1,2,
        7,6,3,2,
        0,1,3,2,
        4,5,7,6
    };

    public void DrawCube(Color c, float time = 100f)
    {
        Vector3[] vertices = this.getVerticesForCube();
        Debug.DrawLine(vertices[0], vertices[1], c, time);
        Debug.DrawLine(vertices[0], vertices[3], c, time);
        Debug.DrawLine(vertices[0], vertices[4], c, time);

        Debug.DrawLine(vertices[2], vertices[1], c, time);
        Debug.DrawLine(vertices[2], vertices[3], c, time);
        Debug.DrawLine(vertices[2], vertices[6], c, time);

        Debug.DrawLine(vertices[7], vertices[4], c, time);
        Debug.DrawLine(vertices[7], vertices[6], c, time);
        Debug.DrawLine(vertices[7], vertices[3], c, time);

        Debug.DrawLine(vertices[5], vertices[4], c, time);
        Debug.DrawLine(vertices[5], vertices[6], c, time);
        Debug.DrawLine(vertices[5], vertices[1], c, time);
    }
}

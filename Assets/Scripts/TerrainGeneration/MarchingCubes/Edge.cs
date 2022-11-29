using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector3 Position;
    public int Index;

    public Edge(Vertex vertex1, Vertex vertex2, int meshIndex = 0)
    {
        Position = (vertex1.Position + vertex2.Position) / 2f;
        Index = meshIndex;
    }
}

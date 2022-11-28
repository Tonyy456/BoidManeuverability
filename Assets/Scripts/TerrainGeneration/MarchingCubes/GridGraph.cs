using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGraph
{

    //define variables related to the space this graph takes
    private Vector3 _position;
    private float _seperation;
    private Vector3Int _dimensions;
    private Vector3 size;
    private Vector3 startPoint; //the point furthest in the negative direction

    //define all vertices and edges
    private Vertex[,,] vertices;
    private Edge[,,] Xparallel;
    private Edge[,,] Yparallel;
    private Edge[,,] Zparallel;

    private bool _verticesGenerated = false;
    public GridGraph(Vector3 center, Vector3Int dimensions,  float seperation)
    {
        _dimensions = dimensions;
        _position = center;
        _seperation = seperation;
        size = new Vector3((dimensions.x - 1) * seperation, (dimensions.y - 1) * seperation, (dimensions.z - 1) * seperation);

        startPoint = new Vector3(-size.x / 2 + center.x, -size.y / 2 + center.y, -size.z / 2 + center.z);

        vertices = new Vertex[dimensions.x, dimensions.y, dimensions.z];
        Xparallel = new Edge[dimensions.x - 1, dimensions.y, dimensions.z];
        Yparallel = new Edge[dimensions.x, dimensions.y - 1, dimensions.z];
        Zparallel = new Edge[dimensions.x, dimensions.y, dimensions.z - 1];

        GenerateVertices();
        GenerateEdges();
    }

    public void DrawBounds(float time, Color c)
    {
        Debug.DrawLine(vertices[0, 0, 0].Position, vertices[0, _dimensions.y - 1, 0].Position, c, time);
        Debug.DrawLine(vertices[0, 0, 0].Position, vertices[_dimensions.x - 1, 0, 0].Position, c, time);
        Debug.DrawLine(vertices[0, 0, 0].Position, vertices[0, 0, _dimensions.z - 1].Position, c, time);

        Debug.DrawLine(vertices[_dimensions.x - 1, 0, _dimensions.z - 1].Position, vertices[0, 0, _dimensions.z - 1].Position, c, time);
        Debug.DrawLine(vertices[_dimensions.x - 1, 0, _dimensions.z - 1].Position, vertices[_dimensions.x - 1, _dimensions.y - 1, _dimensions.z - 1].Position, c, time);
        Debug.DrawLine(vertices[_dimensions.x - 1, 0, _dimensions.z - 1].Position, vertices[_dimensions.x - 1, 0, 0].Position, c, time);

        Debug.DrawLine(vertices[0, _dimensions.y - 1, _dimensions.z - 1].Position, vertices[_dimensions.x - 1, _dimensions.y - 1, _dimensions.z - 1].Position, c, time);
        Debug.DrawLine(vertices[0, _dimensions.y - 1, _dimensions.z - 1].Position, vertices[0, 0, _dimensions.z - 1].Position, c, time);
        Debug.DrawLine(vertices[0, _dimensions.y - 1, _dimensions.z - 1].Position, vertices[0, _dimensions.y - 1, 0].Position, c, time);

        Debug.DrawLine(vertices[_dimensions.x - 1, _dimensions.y - 1, 0].Position, vertices[0, _dimensions.y - 1, 0].Position, c, time);
        Debug.DrawLine(vertices[_dimensions.x - 1, _dimensions.y - 1, 0].Position, vertices[_dimensions.x - 1, 0, 0].Position, c, time);
        Debug.DrawLine(vertices[_dimensions.x - 1, _dimensions.y - 1, 0].Position, vertices[_dimensions.x - 1, _dimensions.y - 1, _dimensions.z - 1].Position, c, time);
    }

    private void GenerateVertices()
    {
        for(int x = 0; x < _dimensions.x; x++)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int z = 0; z < _dimensions.z; z++)
                {
                    Vector3 vertexPosition = new Vector3(startPoint.x + _seperation * x, startPoint.y + _seperation * y, startPoint.z + _seperation * z);
                    vertices[x, y, z] = new Vertex(vertexPosition, false);
                }
            }
        }
        _verticesGenerated = true;
    }

    private void GenerateEdges()
    {
        if (!_verticesGenerated) throw new System.Exception("Generating edges before vertices generated");
        int index = 0;
        //create x parallel edges
        for (int x = 0; x < _dimensions.x - 1; x++)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int z = 0; z < _dimensions.z; z++)
                {
                    Edge e = new Edge(vertices[x, y, z], vertices[x + 1, y, z], index++);
                    Xparallel[x, y, z] = e;
                }
            }
        }

        //create y parallel edges
        for (int x = 0; x < _dimensions.x; x++)
        {
            for (int y = 0; y < _dimensions.y - 1; y++)
            {
                for (int z = 0; z < _dimensions.z; z++)
                {
                    Edge e = new Edge(vertices[x, y, z], vertices[x, y + 1, z], index++);
                    Yparallel[x, y, z] = e;
                }
            }
        }

        //Create z parallel edges
        for (int x = 0; x < _dimensions.x; x++)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int z = 0; z < _dimensions.z - 1; z++)
                {
                    Edge e = new Edge(vertices[x, y, z], vertices[x, y, z + 1], index++);
                    Zparallel[x, y, z] = e;
                }
            }
        }
        Debug.Log($"Created {index} edges");
    }



    public List<Edge> getEdges()
    {
        List<Edge> edges = new List<Edge>();
        return edges;
    }

    public List<Vertex> getVertices()
    {
        List<Vertex> vertices = new List<Vertex>();
        foreach (Vertex v in this.vertices) vertices.Add(v);
        return vertices;
    }

    private GameObject PlaceCircle(Vector3 position, Color c, string name)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.GetComponent<MeshRenderer>().material.color = c;
        go.transform.position = position;
        return go;
    }
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
     */
    public Vertex[] getVerticesForCube(int x, int y, int z)
    {
        Vertex[] vertices = new Vertex[8];

        vertices[0] = this.vertices[x, y, z + 1];
        vertices[1] = this.vertices[x + 1, y, z + 1];
        vertices[2] = this.vertices[x + 1, y, z];
        vertices[3] = this.vertices[x, y, z];
        vertices[4] = this.vertices[x, y + 1, z + 1];
        vertices[5] = this.vertices[x + 1, y + 1, z + 1];
        vertices[6] = this.vertices[x + 1, y + 1, z];
        vertices[7] = this.vertices[x, y + 1, z];
        return vertices;
    }
    /*
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
     *       +------2-----+
     * 
     */

    public Vector3[] getMeshVertices()
    {
        Vector3[] vs = new Vector3[Xparallel.Length + Yparallel.Length + Zparallel.Length];
        foreach (Edge e in Xparallel) vs[e.MeshIndex] = e.position;
        foreach (Edge e in Yparallel) vs[e.MeshIndex] = e.position;
        foreach (Edge e in Zparallel) vs[e.MeshIndex] = e.position;
        return vs;
    }
    public Edge[] getEdgesForCube(int x, int y, int z)
    {
        Edge[] edges = new Edge[12];
        edges[0]  = Xparallel[x, y, z + 1];
        edges[1]  = Zparallel[x + 1, y, z];
        edges[2]  = Xparallel[x, y, z];
        edges[3]  = Zparallel[x, y, z];
        edges[4]  = Xparallel[x, y + 1, z + 1];
        edges[5]  = Zparallel[x + 1, y + 1, z];
        edges[6]  = Xparallel[x, y + 1, z];
        edges[7]  = Zparallel[x, y + 1, z];
        edges[8]  = Yparallel[x, y, z + 1];
        edges[9]  = Yparallel[x + 1, y, z + 1];
        edges[10] = Yparallel[x + 1, y, z];
        edges[11] = Yparallel[x, y, z];
        return edges;
    }
}

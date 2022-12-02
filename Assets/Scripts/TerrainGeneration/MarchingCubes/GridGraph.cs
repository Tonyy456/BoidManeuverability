using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridGraph
{

    //define variables related to the space this graph takes
    private Vector3 startPoint; //the point furthest in the negative direction
    private Vector3 Position { get; set; }
    private float Seperation { get; set; }
    private Vector3 Size { get; set; }
    public Vector3Int Dimensions { get; set; }

    //define all vertices and edges
    public Vector3[,,] vertices { get; set; }
    public Vector3[,,] Xparallel { get; set; }
    public Vector3[,,] Yparallel { get; set; }
    public Vector3[,,] Zparallel { get; set; }

    private bool _verticesGenerated = false;
    private Dictionary<Vector3, int> meshIndex;
    private int meshVertexCount = 0;
    public GridGraph(Vector3 center, Vector3Int dimensions,  float seperation)
    {
        Dimensions = dimensions;
        Position = center;
        Seperation = seperation;
        Size = new Vector3((dimensions.x - 1) * seperation, (dimensions.y - 1) * seperation, (dimensions.z - 1) * seperation);
        startPoint = new Vector3(-Size.x / 2 + center.x, -Size.y / 2 + center.y, -Size.z / 2 + center.z);

        vertices = new Vector3[dimensions.x, dimensions.y, dimensions.z];
        Xparallel = new Vector3[dimensions.x - 1, dimensions.y, dimensions.z];
        Yparallel = new Vector3[dimensions.x, dimensions.y - 1, dimensions.z];
        Zparallel = new Vector3[dimensions.x, dimensions.y, dimensions.z - 1];

        GenerateVertices();
        GenerateEdges();
    }



    private void GenerateVertices()
    {
        for(int x = 0; x < Dimensions.x; x++) {
            for (int y = 0; y < Dimensions.y; y++) {
                for (int z = 0; z < Dimensions.z; z++) {
                    Vector3 vertexPosition = new Vector3(
                        startPoint.x + Seperation * x, 
                        startPoint.y + Seperation * y, 
                        startPoint.z + Seperation * z);
                    vertices[x, y, z] = vertexPosition;
                }
            }
        }
        _verticesGenerated = true;
    }

    private void GenerateEdges()
    {
        if (!_verticesGenerated) throw new System.Exception("Generating edges before vertices generated");
        meshIndex = new Dictionary<Vector3, int>();
        int index = 0;
        //create x parallel edges
        for (int x = 0; x < Dimensions.x - 1; x++) {
            for (int y = 0; y < Dimensions.y; y++) {
                for (int z = 0; z < Dimensions.z; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x + 1, y, z]) / 2f);
                    Xparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }

        //create y parallel edges
        for (int x = 0; x < Dimensions.x; x++)  {
            for (int y = 0; y < Dimensions.y - 1; y++) {
                for (int z = 0; z < Dimensions.z; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x, y + 1, z]) / 2f);
                    Yparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }

        //Create z parallel edges
        for (int x = 0; x < Dimensions.x; x++)  {
            for (int y = 0; y < Dimensions.y; y++) {
                for (int z = 0; z < Dimensions.z - 1; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x, y, z + 1]) / 2f);
                    Zparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }
        meshVertexCount = index;
    }

    public Vector3[] getMeshVertices()
    {
        Vector3[] vertices = new Vector3[meshVertexCount];
        foreach(var pair in meshIndex)
        {
            vertices[pair.Value] = pair.Key;
        }
        return vertices;
    }

    public int getMeshIndex(Vector3 edgePosition)
    {
        if(meshIndex.TryGetValue(edgePosition, out var index))
        {
            return index;
        }
        throw new System.Exception("Edge is unknown in a cube");
    }

    public List<Vector3> getEdges()
    {
        throw new System.NotImplementedException();
    }

    public List<Vertex> getVertices()
    {
        throw new System.NotImplementedException();
    }

    public void DrawBounds(float time, Color c)
    {
        Debug.DrawLine(vertices[0, 0, 0], vertices[0, Dimensions.y - 1, 0], c, time);
        Debug.DrawLine(vertices[0, 0, 0], vertices[Dimensions.x - 1, 0, 0], c, time);
        Debug.DrawLine(vertices[0, 0, 0], vertices[0, 0, Dimensions.z - 1], c, time);

        Debug.DrawLine(vertices[Dimensions.x - 1, 0, Dimensions.z - 1], vertices[0, 0, Dimensions.z - 1], c, time);
        Debug.DrawLine(vertices[Dimensions.x - 1, 0, Dimensions.z - 1], vertices[Dimensions.x - 1, Dimensions.y - 1, Dimensions.z - 1], c, time);
        Debug.DrawLine(vertices[Dimensions.x - 1, 0, Dimensions.z - 1], vertices[Dimensions.x - 1, 0, 0], c, time);

        Debug.DrawLine(vertices[0, Dimensions.y - 1, Dimensions.z - 1], vertices[Dimensions.x - 1, Dimensions.y - 1, Dimensions.z - 1], c, time);
        Debug.DrawLine(vertices[0, Dimensions.y - 1, Dimensions.z - 1], vertices[0, 0, Dimensions.z - 1], c, time);
        Debug.DrawLine(vertices[0, Dimensions.y - 1, Dimensions.z - 1], vertices[0, Dimensions.y - 1, 0], c, time);

        Debug.DrawLine(vertices[Dimensions.x - 1, Dimensions.y - 1, 0], vertices[0, Dimensions.y - 1, 0], c, time);
        Debug.DrawLine(vertices[Dimensions.x - 1, Dimensions.y - 1, 0], vertices[Dimensions.x - 1, 0, 0], c, time);
        Debug.DrawLine(vertices[Dimensions.x - 1, Dimensions.y - 1, 0], vertices[Dimensions.x - 1, Dimensions.y - 1, Dimensions.z - 1], c, time);
    }
}

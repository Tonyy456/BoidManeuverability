using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GridCubeIndexer
{
    private GridGraph graph;
    private Vector3[,,] vertices { get => graph.vertices; }
    private Vector3[,,] Xparallel { get => graph.Xparallel; }
    private Vector3[,,] Yparallel { get => graph.Yparallel; }
    private Vector3[,,] Zparallel { get => graph.Zparallel; }

    public GridCubeIndexer(GridGraph graph)
    {
        this.graph = graph;
    }

    public Vector3[] getVerticesForCube(int x, int y, int z)
    {
        Vector3[] vertices = new Vector3[8];

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

    public Vector3[] getMeshVertices()
    {
        List<Vector3> vertices = new List<Vector3>();
        for(int z = 0; z < graph.Dimensions.z; z++)
        {
            for(int y = 0; y < graph.Dimensions.y; y++)
            {
                for (int x = 0; x < graph.Dimensions.x; x++)
                {
                    vertices.Add(graph.vertices[x, y, z]);
                }
            }
        }
        return vertices.ToArray();
    }

    private int getMeshindex(int x, int y, int z) =>
        x + graph.Dimensions.x * y + graph.Dimensions.x * graph.Dimensions.y * z;
    public int[] getEdgeIndices(Vector3[] edges)
    {
        int[] edgeIndices = new int[12];
        for(int i = 0; i < 12; i++)
        {
            edgeIndices[i] = graph.getMeshIndex(edges[i]);
        }

        return edgeIndices;
    }
    public Vector3[] getEdgesForCube(int x, int y, int z)
    {
        Vector3[] edges = new Vector3[12];
        edges[0] = Xparallel[x, y, z + 1];
        edges[1] = Zparallel[x + 1, y, z];
        edges[2] = Xparallel[x, y, z];
        edges[3] = Zparallel[x, y, z];
        edges[4] = Xparallel[x, y + 1, z + 1];
        edges[5] = Zparallel[x + 1, y + 1, z];
        edges[6] = Xparallel[x, y + 1, z];
        edges[7] = Zparallel[x, y + 1, z];
        edges[8] = Yparallel[x, y, z + 1];
        edges[9] = Yparallel[x + 1, y, z + 1];
        edges[10] = Yparallel[x + 1, y, z];
        edges[11] = Yparallel[x, y, z];
        return edges;
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
}

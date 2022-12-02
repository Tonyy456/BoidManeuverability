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
 * GridCubeIndexer is a class that takes a graph and indexes vertices and edges
 * based on the subindex of a cube.
 * 
 * a 3d array of points equally seperated forms smaller cubes.
 * Cube(0,0,0) starts at the most negative coordinates.
 */
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GridCubeIndexer
{
    /*
     *  Static list makes code more understanble. Clear to see how the indexing works.
     *  vertexIndexer: convert vertex index to the relative vertex index into 3d array based on index of the cube.
     *  edgeIndexer: tuple contains the relative index of the edge like above... and the axis that edge is parallel to.
     */
    private static Vector3Int[] vertexIndexer = new Vector3Int[8]
    {
        new Vector3Int(0,0,1),
        new Vector3Int(1,0,1),
        new Vector3Int(1,0,0),
        new Vector3Int(0,0,0),
        new Vector3Int(0,1,1),
        new Vector3Int(1,1,1),
        new Vector3Int(1,1,0),
        new Vector3Int(0,1,0)
    };
    private static (Vector3Int relativeIndex, int axis)[] edgeIndexer = new (Vector3Int relativeIndex, int axis)[12]
    {   
        (new Vector3Int(0,0,1), 1),
        (new Vector3Int(1,0,0), 3),
        (new Vector3Int(0,0,0), 1),
        (new Vector3Int(0,0,0), 3),
        (new Vector3Int(0,1,1), 1),
        (new Vector3Int(1,1,0), 3),
        (new Vector3Int(0,1,0), 1),
        (new Vector3Int(0,1,0), 3),
        (new Vector3Int(0,0,1), 2),
        (new Vector3Int(1,0,1), 2),
        (new Vector3Int(1,0,0), 2),
        (new Vector3Int(0,0,0), 2)
    };

    private GridGraph graph;
    private Vector3[,,] gridVertices { get => graph.vertices; }
    private Vector3[,,] XEdges { get => graph.Xparallel; }
    private Vector3[,,] YEdges { get => graph.Yparallel; }
    private Vector3[,,] ZEdges { get => graph.Zparallel; }

    /*
     * Constructor
     */
    public GridCubeIndexer(GridGraph graph) => this.graph = graph;

    /*
     * Get the vertices for a cube at index x,y,z
     * where 0,0,0 is the cube furthest in the negative x,y,z direction.
     */
    public Vector3[] getVerticesForCube(int x, int y, int z)
    {
        Vector3[] cubeVertices = new Vector3[8];
        for(int i = 0; i < 8; i++)
        {
            Vector3Int ind = vertexIndexer[i] + new Vector3Int(x,y,z);
            cubeVertices[i] = gridVertices[ind.x, ind.y, ind.z];
        }
        return cubeVertices;
    }

    /*
     * Get the edges in a list in the correct way defined at the top of this file.
     */
    public Vector3[] getEdgesForCube(int x, int y, int z)
    {
        Vector3[] edges = new Vector3[12];
        for(int i = 0; i < 12; i++)
        {
            var relIndex = edgeIndexer[i];
            Vector3Int ind = relIndex.relativeIndex + new Vector3Int(x,y,z);
            edges[i] = relIndex.axis switch
            { //index into the correct array of edges based on the axis
                1 => XEdges[ind.x, ind.y, ind.z],
                2 => YEdges[ind.x, ind.y, ind.z],
                _ => ZEdges[ind.x, ind.y, ind.z],
            };
        }
        return edges;
    }

    /*
     * Get the mesh indices for each edge in a parallel array.
     */
    public int[] getEdgeIndices(Vector3[] edges)
    {
        int[] edgeIndices = new int[12];
        for(int i = 0; i < 12; i++)
            edgeIndices[i] = graph.GetMeshIndex(edges[i]);
        return edgeIndices;
    }

}

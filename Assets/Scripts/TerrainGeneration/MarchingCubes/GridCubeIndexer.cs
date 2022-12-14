 /*
 * GridCubeIndexer is a class that takes a graph and indexes vertices and edges
 * based on the subindex of a cube.
 * 
 * a 3d array of points equally seperated forms smaller cubes.
 * Cube(0,0,0) starts at the most negative coordinates.
 */
 /* 
  * Written By: Tony D'Alesandro
  */
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GridCubeIndexer
{
    private GridGraph graph;

    /*
     * Constructor
     */
    public GridCubeIndexer(GridGraph graph)
    {
        this.graph = graph;
    }

    public Vector3[] getVerticesForCube(int x, int y, int z)
    {
        return graph.subCubes[x,y,z].getVerticesForCube();
    }

    public Vector3[] getEdgesForCube(int x, int y, int z)
    {
        return graph.subCubes[x,y,z].getEdgesForCube();
    }

    public int[] getEdgeIndices(Vector3[] edges)
    {
        List<int> indicies = new List<int>();
        foreach(var e in edges)
        {
            indicies.Add(graph.GetMeshIndex(e));
        }
        return indicies.ToArray();
    }

}

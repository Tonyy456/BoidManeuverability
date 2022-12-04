using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MCCubes
{
    private VertexStatusGenerator generator;
    public GridGraph graph { get; set; }
    private Vector3Int dimensions { get => graph.Resolution; }
    public Vector3Int cubeCount { get; set; }

    MCCube[,,] cubes;

    public MCCubes(GridGraph graph, VertexStatusGenerator generator)
    {
        this.graph = graph;
        this.generator = generator;
        cubeCount = new Vector3Int(dimensions.x - 1, dimensions.y - 1, dimensions.z - 1);
        cubes = new MCCube[cubeCount.x, cubeCount.y, cubeCount.z];
        CreateCubes();
    }

    private void CreateCubes()
    {
        GridCubeIndexer indexer = new GridCubeIndexer(graph);
        for (int i = 0; i < dimensions.x - 1; i++)
        {
            for(int j = 0; j < dimensions.y - 1; j++)
            {
                for(int k = 0; k < dimensions.z - 1; k++)
                {
                    Vector3[] vertices = indexer.getVerticesForCube(i, j, k);
                    bool[] status = generator.getStatusForCube(i, j, k);
                    Vector3[] edges = indexer.getEdgesForCube(i, j, k);
                    int[] meshIndex = indexer.getEdgeIndices(edges);
                    MCCube cube = new MCCube(vertices, status, edges, meshIndex);
                    cubes[i, j, k] = cube;
                }
            }
        }
    }

    public IEnumerable<int> MarchEnumerator()
    {
        foreach (MCCube cube in cubes)
        {
            cube.March();
            foreach (int i in cube.triangles)
                yield return i;
        }
    }

    public int[] March()
    {
        List<int> triangles = new List<int>();
        foreach(MCCube cube in cubes)
        {
            cube.March();
            foreach (int i in cube.triangles)
                triangles.Add(i);
        }
        return triangles.ToArray();
    }
}

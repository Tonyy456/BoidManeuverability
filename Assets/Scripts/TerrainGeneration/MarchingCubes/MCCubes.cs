using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MCCubes
{
    private Vector3 center;
    private Vector3Int dimensions;
    private float seperation;

    MCCube[,,] cubes;
    List<MCCube> lc;
    public int[] Triangles
    {
        get
        {
            List<int> triangles = new List<int>();
            foreach (var cube in cubes)
            {
                foreach (int item in cube.getTriangles())
                    triangles.Add(item);
            }
            return triangles.ToArray();
        }
    }

    public GridGraph graph { get; set; }

    public MCCubes(Vector3 center, Vector3Int dimensions, float seperation)
    {
        this.center = center;
        this.dimensions = dimensions;
        this.seperation = seperation;
        graph = new GridGraph(center, dimensions, seperation);
        cubes = new MCCube[dimensions.x - 1, dimensions.y - 1, dimensions.z - 1];
        lc = new List<MCCube>();
        CreateCubes();
    }

    private void CreateCubes()
    {
        for(int i = 0; i < dimensions.x - 1; i++)
        {
            for(int j = 0; j < dimensions.y - 1; j++)
            {
                for(int k = 0; k < dimensions.z - 1; k++)
                {
                    MCCube cube = new MCCube(graph.getVerticesForCube(i, j, k), graph.getEdgesForCube(i, j, k));
                    cubes[i, j, k] = cube;
                    lc.Add(cube);
                }
            }
        }
    }

    public void March()
    {
        foreach(MCCube cube in cubes)
        {
            cube.March();
        }       
    }

    public int CubeCount { get { return (dimensions.x - 1) * (dimensions.y - 1) * (dimensions.z - 1); } }

    public List<int> March(int index)
    {
        MCCube cube = lc[index];
        cube.March();
        return cube.getTriangles();
    }
}

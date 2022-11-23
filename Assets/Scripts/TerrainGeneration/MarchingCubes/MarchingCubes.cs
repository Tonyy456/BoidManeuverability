using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MarchingCubes
{
    private Vector3 center;
    private Vector3Int dimensions;
    private float seperation;

    MCCube[,,] cubes; 
    GridGraph graph;
    private int[] Triangles
    {
        get
        {
            List<int> triangles = new List<int>();
            foreach (var cube in cubes)
            {
                cube.March();
                foreach (int item in cube.getTriangles())
                    triangles.Add(item);
            }
            return triangles.ToArray();
        }
    }

    private Vector3[] Vertices
    {
        get { return graph.getMeshVertices();  }
    }


    public MarchingCubes(Vector3 center, Vector3Int dimensions, float seperation)
    {
        this.center = center;
        this.dimensions = dimensions;
        this.seperation = seperation;

        graph = new GridGraph(center, dimensions, seperation);
        cubes = new MCCube[dimensions.x - 1, dimensions.y - 1, dimensions.z - 1];

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
                }
            }
        }
    }

    public void March(MeshFilter filter, float frequency = 0.05f, float surfaceValue = 0.5f)
    {
        Mesh mesh = new Mesh();
        GiveVerticesNoise(frequency, surfaceValue);
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        filter.mesh = mesh;     
    }

    public void DisplayVertices(float scale)
    {
        foreach(var v in graph.getVertices())
        {
            CreateSphere(v.Position, v.IsOn ? Color.white : Color.black, $"ison: {v.IsOn}", scale);
        }
    }

    private void GiveVerticesNoise(float frequency, float surfaceValue)
    {
        foreach(var vertex in graph.getVertices())
        {
            float value = PerlinNoise.get3DPerlinNoise(vertex.Position, frequency);
            if (value < surfaceValue)
                vertex.IsOn = true;
        }
    }

    private GameObject parent = new GameObject();
    private void CreateSphere(Vector3 position, Color c, string name = "sphere", float scale = 1f)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = position;
        go.GetComponent<MeshRenderer>().material.color = c;
        go.name = name;
        go.transform.localScale = new Vector3(scale, scale, scale);
        go.transform.parent = parent.transform;
    }
}

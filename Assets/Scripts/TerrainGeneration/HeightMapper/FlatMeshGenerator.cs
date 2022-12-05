using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlatMeshGenerator
{
    private Vector3 center;
    private Vector3Int resolution;
    private float pointSeperation;
    public FlatMeshGenerator(Vector3 center, Vector3Int resolution, float pointSeperation)
    {
        this.center = center;
        this.resolution = resolution;
        this.pointSeperation = pointSeperation;
    }

    public (Vector3[] vertices, int[] triangles) getMeshDefintion()
    {
        Vector3[] vertices = generateVertices();
        int[] indices = generateTriangles();
        return (vertices, indices);
    }

    public Mesh getMesh()
    {
        Mesh mesh = new Mesh();
        var definition = getMeshDefintion();
        mesh.vertices = definition.vertices;
        mesh.triangles = definition.triangles;
        return mesh;
    }

    private Vector3[] generateVertices()
    {
        // Generating vertices
        Vector3 start = new Vector3(
            center.x - (resolution.x - 1) * pointSeperation / 2,
            center.y,
            center.z - (resolution.z - 1) * pointSeperation / 2
            );

        List<Vector3> meshPoints = new List<Vector3>();
        for (int x = 0; x < resolution.x; x++)
        {
            for (int z = 0; z < resolution.z; z++)
            {
                Vector3 pos = new Vector3(
                    start.x + pointSeperation * x,
                    start.y,
                    start.z + pointSeperation * z);
                meshPoints.Add(pos);
            }
        }
        return meshPoints.ToArray();
    }

    private int[] generateTriangles()
    {
        List<int> indices = new List<int>();
        int width = resolution.x;
        for (int x = 0; x < resolution.x - 1; x++)
        {
            for (int y = 0; y < resolution.y - 1; y++)
            {
                int vertex = x + width * y;
                indices.Add(vertex + width + 1);
                indices.Add(vertex + width);
                indices.Add(vertex);

                indices.Add(vertex);
                indices.Add(vertex + 1);
                indices.Add(vertex + width + 1);
            }
        }
        return indices.ToArray();
    }
}

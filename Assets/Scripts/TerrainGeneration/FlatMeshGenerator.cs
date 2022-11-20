using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatMeshGenerator
{
    public FlatMeshGenerator() { }

    public Mesh GenerateMesh(float distance, int resolution, Vector3 center)
    {
        Mesh mesh = new Mesh();
        float distBetweenPoints = distance / resolution;

        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Vector3 position = new Vector3(
                    -distance / 2 + distBetweenPoints * i,
                    0,
                    -distance / 2 + distBetweenPoints * j
                    );
                vertices.Add(position);
            }
        }

        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int index = 0;
        for(int i = 0; i < resolution - 1; i++)
        {
            for(int j = 0; j < resolution - 1; j++)
            {
                int vertex = i + resolution * j;
                triangles[index++] = vertex + resolution + 1;
                triangles[index++] = vertex + resolution;
                triangles[index++] = vertex;

                //TopRight triangle of quad in correct winding order
                triangles[index++] = vertex;
                triangles[index++] = vertex + 1;
                triangles[index++] = vertex + resolution + 1;
                
            }
        }

        mesh.SetVertices(vertices);
        mesh.triangles = triangles;
        return mesh;
    }
}

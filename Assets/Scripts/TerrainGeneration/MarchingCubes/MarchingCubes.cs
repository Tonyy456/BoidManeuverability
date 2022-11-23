using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MarchingCubes
{
    private Vector3 center;
    private float distance;
    private int resolution;

    List<MCCube> marchingCubes = new List<MCCube>();
    private Edge[,,] ex;
    private Edge[,,] ey;
    private Edge[,,] ez;
    private Vertex[,,] vertices;

    private Mesh mesh;
    public MarchingCubes(Vector3 center, float distance, int resolution)
    {
        this.center = center;
        this.distance = distance;
        this.resolution = resolution;

        ex = new Edge[resolution - 1, resolution, resolution];
        ey = new Edge[resolution, resolution - 1, resolution];
        ez = new Edge[resolution, resolution, resolution - 1];
        vertices = new Vertex[resolution, resolution, resolution];
        mesh = new Mesh();
    }

    public void ShowVertices()
    {
        foreach (var v in vertices)
            v.ToggleVertexOn();
        
    }

    public void MassiveMarch(MeshFilter filter)
    {
        mesh.Clear();
        mesh.vertices = GatherMeshVertices();
        mesh.triangles = GatherTriangles();
        mesh.RecalculateNormals();
        filter.mesh = mesh;
    }

    public void March(MeshFilter filter, float frequency = 0.05f, float surfaceValue = 0.5f)
    {
        Initialize();
        mesh.Clear();
        mesh.vertices = GatherMeshVertices();
        GiveVerticesNoise(frequency, surfaceValue);
        mesh.triangles = GatherTriangles();
        mesh.RecalculateNormals();
        filter.mesh = mesh;        
    }

    private void GiveVerticesNoise(float frequency, float surfaceValue)
    {
        foreach(var vertex in vertices)
        {
            float value = PerlinNoise.PerlinNoise3D(vertex.Position.x, vertex.Position.y, vertex.Position.z, frequency);
            if (value < surfaceValue)
                vertex.IsOn = true;
        }
    }

    private int[] GatherTriangles()
    {
        List<int> triangles = new List<int>();
        foreach(var cube in marchingCubes)
        {
            cube.March();
            foreach (int item in cube.getTriangles())
                triangles.Add(item);
        }
        return triangles.ToArray();
    }

    private Vector3[] GatherMeshVertices()
    {
        List<Vector3> verts = new List<Vector3>();
        foreach (var x in ex) verts.Add(x.position);
        foreach (var y in ey) verts.Add(y.position);
        foreach (var z in ez) verts.Add(z.position);
        return verts.ToArray();
    }

    private void Initialize()
    {
        CreateVertices();
        CreateEdges();
        CreateCubes();
    }

    private void CreateVertices()
    {
        float scale = distance / resolution;
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    vertices[x, y, z] = new Vertex(new Vector3(x * scale, y * scale, z * scale), false);
                }
            }
        }
    }

    private void CreateEdges()
    {
        int meshIndex = 0;
        for (int pla = 0; pla < resolution - 1; pla++) // pla = parallel axis
        {
            for (int pa1 = 0; pa1 < resolution; pa1++) // pa1 = perpendicular axis 1
            {
                for (int pa2 = 0; pa2 < resolution; pa2++) // pa2 = perpendicular axis 2
                {
                    Edge x_parallel_edge = new Edge(vertices[pla, pa1, pa2], vertices[pla + 1, pa1, pa2], meshIndex++);
                    Edge y_parallel_edge = new Edge(vertices[pa1, pla, pa2], vertices[pa1, pla + 1, pa2], meshIndex++);
                    Edge z_parallel_edge = new Edge(vertices[pa1, pa2, pla], vertices[pa1, pa2, pla + 1], meshIndex++);

                    ex[pla, pa1, pa2] = x_parallel_edge;
                    ey[pa1, pla, pa2] = y_parallel_edge;
                    ez[pa1, pa2, pla] = z_parallel_edge;
                }
            }
        }
    }

    private void CreateCubes()
    {
        for (int x = 0; x < resolution - 1; x++)
        {
            for (int y = 0; y < resolution - 1; y++)
            {
                for (int z = 0; z < resolution - 1; z++)
                {
                    Vertex[] cubeVertices = new Vertex[8];
                    cubeVertices[0] = vertices[x, y, z + 1];
                    cubeVertices[1] = vertices[x + 1, y, z + 1];
                    cubeVertices[2] = vertices[x + 1, y, z];
                    cubeVertices[3] = vertices[x, y, z];
                    cubeVertices[4] = vertices[x, y + 1, z + 1];
                    cubeVertices[5] = vertices[x + 1, y + 1, z + 1];
                    cubeVertices[6] = vertices[x + 1, y + 1, z];
                    cubeVertices[7] = vertices[x, y + 1, z];

                    Edge[] cubeEdges = new Edge[12];
                    cubeEdges[0] = ex[x , y , z + 1];
                    cubeEdges[1] = ez[x + 1, y , z ];
                    cubeEdges[2] = ex[x, y , z];
                    cubeEdges[3] = ez[x, y , z];
                    cubeEdges[4] = ex[x, y + 1, z + 1];
                    cubeEdges[5] = ez[x + 1, y + 1, z];
                    cubeEdges[6] = ex[x, y + 1, z];
                    cubeEdges[7] = ez[x, y + 1, z];
                    cubeEdges[8] = ey[x , y, z + 1];
                    cubeEdges[9] = ey[x + 1 , y, z + 1];
                    cubeEdges[10] = ey[x + 1, y, z];
                    cubeEdges[11] = ey[x, y, z];

                    MCCube cube = new MCCube(cubeVertices, cubeEdges);
                    marchingCubes.Add(cube);
                }
            }
        }
    }

    private void CreateSphere(Vector3 position, Color c, string name = "sphere")
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = position;
        go.GetComponent<MeshRenderer>().material.color = c;
        go.name = name;
    }
}

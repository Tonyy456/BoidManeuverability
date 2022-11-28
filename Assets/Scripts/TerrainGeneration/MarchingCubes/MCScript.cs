using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Jobs;

public class MCGeneration : ITerrainAlgorithm
{
    private GenerationSettings _settings;
    private MeshFilter filter;
    private MarchingCubes cubes;
    public MCGeneration(GenerationSettings settings, MeshFilter filter)
    {
        _settings = settings;
        this.filter = filter;
    }

    public void Generate(Vector3 position)
    {
        March();
    }

    public void March()
    {
        cubes = new MarchingCubes(_settings.center, _settings.resolution, _settings.pointSeperation);
        GridGraph graph = cubes.graph;
        
        foreach (Vertex v in graph.getVertices())
        {
            float value = PerlinNoise.get3DPerlinNoise(v.Position, _settings.frequency);
            if (value < _settings.surface)
                v.IsOn = true;
        }
        

        /*
        generateNoiseForVertex job = new generateNoiseForVertex();
        List<Vertex> vertices = graph.getVertices();
        job.vs = vertices;
        job.frequency = _settings.frequency;
        job.surface = _settings.surface;
        JobHandle handle = job.Schedule<generateNoiseForVertex>(vertices.Count,1);
        handle.Complete();
        */
        

        cubes.March();

        Mesh mesh = new Mesh();
        mesh.vertices = graph.getMeshVertices();
        mesh.triangles = cubes.Triangles;
        mesh.RecalculateNormals();
        Color[] colors = new Color[mesh.vertices.Length];
        float min = _settings.center.y - (_settings.resolution.y * _settings.pointSeperation) / 2f;
        float max = _settings.center.y + (_settings.resolution.y * _settings.pointSeperation) / 2f;
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float time = (mesh.vertices[i].y - min) / (_settings.pointSeperation * _settings.resolution.y);
            colors[i] = _settings.HeightColorGradient.Evaluate(time);
        }
        mesh.colors = colors;
        filter.mesh = mesh;
        cubes.graph.DrawBounds(1000000f, Color.gray);
        
    }
}

struct generateNoiseForVertex : IJobParallelFor
{
    public List<Vertex> vs;
    public float frequency;
    public float surface;
    public void Execute(int index)
    {
        Vertex v = vs[index];
        float value = PerlinNoise.get3DPerlinNoise(v.Position, frequency);
        if (value < surface)
            v.IsOn = true;
    }
}

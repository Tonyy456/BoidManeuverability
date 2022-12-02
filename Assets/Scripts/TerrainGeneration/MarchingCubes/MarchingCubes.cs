using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Jobs;

public class MarchingCubes : ITerrainAlgorithm
{
    private MeshFilter filter;
    private Mesh mesh;
    private GenerationSettings settings;

    public TMPro.TMP_Text text { get; set; }
    public MarchingCubes(MeshFilter filter)
    {
        this.filter = filter;
    }

    public void Generate(Vector3 position, GenerationSettings settings)
    {
        this.settings = settings;
    }

    public IEnumerator StartMarches()
    {
        GridGraph graph = new GridGraph(settings.center, settings.resolution, settings.pointSeperation);
        NoiseStatusGenerator generator = new NoiseStatusGenerator(graph, settings.frequency, settings.surface, settings.seed);
        MCCubes marchingCubes = new MCCubes(graph, generator);

        Mesh mesh = new Mesh();
        mesh.vertices = generator.getVertices();
        mesh.triangles = marchingCubes.March();

        mesh.RecalculateNormals();
        filter.mesh = mesh;

        yield return null;
    }
}

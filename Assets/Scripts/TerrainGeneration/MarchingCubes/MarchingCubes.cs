using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;

/*
 * Unity script that 
 */
public class MarchingCubes : ITerrainAlgorithm
{
    private MeshFilter filter;
    private GenerationSettings settings;
    private GridGraph graph;

    /*
     * Constructor
     */
    public MarchingCubes(GenerationSettings settings, MeshFilter filter)
    {
        this.settings = settings;
        this.filter = filter;
    }

    /*
     * Generate is used for a coroutine that will create the entire mesh.
     * 
     * Using this kind of interface allows me to yield return if I want to save
     * processing power during the initial creation.
     *  
     */
    public IEnumerator Generate()
    {
        GridGraph graph = new GridGraph(settings.center, settings.resolution, settings.pointSeperation);
        NoiseStatusGenerator generator = new NoiseStatusGenerator(graph, settings.frequency, settings.surface, settings.seed);
        MCCubes marchingCubes = new MCCubes(graph, generator);

        Mesh mesh = new Mesh();
        mesh.vertices = graph.GetMeshIndicies();
        mesh.triangles = marchingCubes.March();

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        mesh.Optimize();

        IMeshColorer colorer = new NormalMeshColorer(mesh, settings.HeightColorGradient);
        colorer.Color();

        filter.mesh = mesh;

        yield return null;
    }

    public void DrawBounds()
    {
        graph.DrawBounds(Color.magenta);
    }
}

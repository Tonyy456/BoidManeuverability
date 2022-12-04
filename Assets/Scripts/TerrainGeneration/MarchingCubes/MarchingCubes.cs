using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;

/*
 * Unity script that 
 */
public class MarchingCubes : ITerrainAlgorithm
{
    private GenerationSettings settings;
    private List<GridGraph> graphs = new List<GridGraph>();

    /*
     * Constructor
     */
    public MarchingCubes(GenerationSettings settings)
    {
        this.settings = settings;
    }

    /*
 * Generate is used for a coroutine that will create the entire mesh.
 * 
 * Using this kind of interface allows me to yield return if I want to save
 * processing power during the initial creation.
 *  
 */
    public IEnumerator Generate(Vector3Int chunk, MeshFilter filter)
    {
        GridGraph graph = new GridGraph(settings.chunkCenter(chunk), settings.chunkDimensions, settings.pointSeperation);
        graphs.Add(graph);
        NoiseStatusGenerator generator = new NoiseStatusGenerator(graph, settings.frequency, settings.surface, settings.seed);
        MCCubes marchingCubes = new MCCubes(graph, generator);

        Mesh mesh = new Mesh();
        mesh.vertices = graph.GetMeshIndicies();

        List<int> triangles = new List<int>();
        foreach (int i in marchingCubes.MarchEnumerator())
        {
            triangles.Add(i);
        }
        mesh.triangles = triangles.ToArray();

        yield return new WaitForFixedUpdate();

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        mesh.Optimize();

        IMeshColorer colorer = new NormalMeshColorer(mesh, settings.ColorGradient);
        colorer.Color();

        filter.mesh = mesh;

        yield return null;
    }



    public void DrawBounds()
    {
        foreach (GridGraph graph in graphs)
        {
            graph.DrawBounds(Color.magenta);
        }
    }
}

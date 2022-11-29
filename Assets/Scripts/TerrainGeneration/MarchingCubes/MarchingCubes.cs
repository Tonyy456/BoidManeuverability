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
    private MCCubes cubes;
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
        March(settings);
    }

    private float freq = 0.05f;
    private float surface = 0.5f;
    public void March(GenerationSettings settings)
    {
        freq = settings.frequency;
        surface = settings.surface;
        cubes = new MCCubes(settings.center, settings.resolution, settings.pointSeperation);
        GridGraph graph = cubes.graph;

        mesh = new Mesh();
        mesh.vertices = graph.getMeshVertices();
        cubes.graph.DrawBounds(1000000f, Color.gray);    }

    public IEnumerator StartMarches()
    {
        filter.mesh = mesh;

        List<Vertex> vertices = cubes.graph.getVertices();
        float totalIterations = vertices.Count + cubes.CubeCount + mesh.vertexCount;
        float currentIteration = 0;
        /*
         * Set vertices on/off based on noise
         */
        foreach(Vertex v in vertices)
        {
            float value = PerlinNoise.get3DPerlinNoise(v.Position, freq);
            if (value < surface)
                v.IsOn = true;
            currentIteration += 1;
            text.text = $"loading: {currentIteration / totalIterations * 100f}%";
            if (totalIterations % 30 == 0)
                yield return new WaitForEndOfFrame();
        }
        vertices = null;
        yield return new WaitForSeconds(0.1f);

        /*
         * grab triangles from cubes and assign to mesh
         */
        List<int> triangles = new List<int>();
        for (int i = 0; i < cubes.CubeCount; i++)
        {
            List<int> newTs = cubes.March(i);
            foreach (int item in newTs)
                triangles.Add(item);
            mesh.triangles = triangles.ToArray();

            currentIteration += 1;
            text.text = $"loading: {currentIteration / totalIterations * 100f}%";
            if (totalIterations % 30 == 0)
                yield return new WaitForEndOfFrame();
        }
        mesh.RecalculateNormals();

        /*
         * Color terrain
         */
        Color[] colors = new Color[mesh.vertexCount];
        for(int i = 0; i < mesh.vertexCount; i++)
        {
            float angle = Vector3.Angle(new Vector3(0, 1, 0), mesh.normals[i]);
            colors[i] = settings.HeightColorGradient.Evaluate(angle / 180f);

            currentIteration += 1;
            text.text = $"loading: {currentIteration / totalIterations * 100f}%";
            if (totalIterations % 30 == 0)
                yield return new WaitForEndOfFrame();
        }
        mesh.SetColors(colors);
        mesh.Optimize();
        filter.mesh = mesh;
    }
}

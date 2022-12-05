using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;

/*
 * Unity script that executes the marching cubes algorithm
 */
public class MarchingCubes : ITerrainAlgorithm
{
    private GenerationSettings settings;
    private List<GridGraph> graphs = new List<GridGraph>();

    public delegate void Complete();
    public Complete OnGenerationComplete;

    /*
     * Constructor
     */
    public MarchingCubes(GenerationSettings settings)
    {
        this.settings = settings;
    }

    /*
     * Creates a shell around the entire play area
     */
    public void CreateBounds(Material material)
    {
        Cube cube = new Cube(settings.gridCenter, settings.gridDimensions.x);
        foreach(var pair in cube.getSurfaces())
        { //creates cube at the specified surface scale in the cube surface
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var comp = go.transform.GetComponent<MeshRenderer>();
            comp.material = material;
            Vector3 scale = Vector3.Scale(pair.scale, settings.gridDimensions);
            if (scale.x == 0) scale.x = 1;
            if (scale.y == 0) scale.y = 1;
            if (scale.z == 0) scale.z = 1;
            go.transform.localScale = scale;
            go.transform.position = pair.center;
        }
    }

    /*
     * Draws lines around the play area cube.
     */
    public void DrawBounds()
    {
        Cube cube = new Cube(settings.gridCenter, settings.gridDimensions.x);
        cube.DrawCube(Color.magenta);
    }

    /*
     * Generate is used for a coroutine that will create the entire mesh.
     * 
     * Using this kind of interface allows me to yield return if I want to save
     * processing power during the initial creation.
     */
    public IEnumerator Generate(Vector3Int chunk, MeshFilter filter, MeshCollider collider, bool signal = false)
    {
        GridGraph graph = new GridGraph(settings.chunkCenter(chunk), settings.resolution, settings.edgeLen);
        NoiseStatusGenerator generator = new NoiseStatusGenerator(graph, settings.frequency, settings.surface, settings.seed);
        MCCubes marchingCubes = new MCCubes(graph, generator);
        graphs.Add(graph);

        Mesh mesh = CreateMesh(graph, marchingCubes);
        PostMeshCreation(mesh, collider);
        filter.mesh = mesh;

        if (signal && OnGenerationComplete != null) OnGenerationComplete();

        yield return null;
    }


    /*
     * Creates a mesh for the marching cubes algorithm
     */
    private Mesh CreateMesh(GridGraph graph, MCCubes marchingCubes)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = graph.GetMeshIndicies();

        List<int> triangles = new List<int>();
        foreach (int i in marchingCubes.MarchEnumerator())
        {
            triangles.Add(i);
        }
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    /*
     * Optimizes mesh, adds the mesh to the collider and color it
     */
    private void PostMeshCreation(Mesh mesh, MeshCollider collider)
    {
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        mesh.Optimize();

        collider.sharedMesh = mesh;

        IMeshColorer colorer = new NormalMeshColorer(mesh, settings.ColorGradient);
        colorer.Color();
    }
}

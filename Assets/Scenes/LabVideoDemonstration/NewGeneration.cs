using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using TerrainGeneration.Version3;
using Unity.VisualScripting;
using UnityEngine;

public class NewGeneration : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] private GameObject parent;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Vector3Int chunk;
    [SerializeField] private Material material;

    [Header("GENERATION SETTINGS")]
    [SerializeField] private bool drawEdges;
    [SerializeField] private float frequency;
    [Range(0f, 1f)]
    [SerializeField] private float surface;
    [SerializeField] private float heightMult;
    [SerializeField] private float seed;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3Int cubesPerChunk;
    [SerializeField] private float edgeLength;

    public void Start()
    {
        CreateBounds(material);
    }
    public void OnValidate()
    {
        if(Application.isPlaying)
        {
            CreateMesh();
        }
    }

    public void FixedUpdate()
    {
        if (!drawEdges) return;
        Mesh m = meshFilter.mesh;
        Vector3[] vertices = m.vertices;
        int[] triangles = m.triangles;
        for(int i = 0; i < triangles.Length; i+=3)
        {
            Debug.DrawLine(vertices[triangles[i]], vertices[triangles[i + 1]], Color.black);
            Debug.DrawLine(vertices[triangles[i + 1]], vertices[triangles[i + 2]], Color.black);
            Debug.DrawLine(vertices[triangles[i + 2]], vertices[triangles[i]], Color.black);
        }
    }

    private void CreateBounds(Material material)
    {
        GenerationSettings settings = getSettings();
        Cube cube = new Cube(settings.gridCenter, settings.gridDimensions.x);
        cube.DrawCube(Color.magenta);
    }

    private void CreateMesh()
    {
        Debug.Log("Creating");
        GenerationSettings settings = getSettings();
        GridGraph graph = new GridGraph(settings.chunkCenter(chunk), settings.resolution, settings.edgeLen);
        NoiseStatusGenerator generator = new NoiseStatusGenerator(graph, settings.frequency, settings.surface, settings.seed);
        MCCubes marchingCubes = new MCCubes(graph, generator);

        Mesh mesh = CreateMesh(graph, marchingCubes);
        PostMeshCreation(mesh, settings);
        meshFilter.mesh = mesh;
    }

    private GenerationSettings getSettings()
    {
        GenerationSettings settings = ScriptableObject.CreateInstance("GenerationSettings") as GenerationSettings;
        settings.frequency = frequency;
        settings.surface = surface;
        settings.heightMultipler = heightMult;
        settings.seed = seed;
        settings.ColorGradient = gradient;
        settings.gridCenter = center;
        settings.chunksPerGrid = new Vector3Int(1,1,1);
        settings.cubesPerChunk = cubesPerChunk;
        settings.edgeLen = edgeLength;
        return settings;
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
    private void PostMeshCreation(Mesh mesh, GenerationSettings settings)
    {
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        mesh.Optimize();

        IMeshColorer colorer = new NormalMeshColorer(mesh, settings.ColorGradient);
        colorer.Color();
    }
}

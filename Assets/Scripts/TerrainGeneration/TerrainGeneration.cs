using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private bool regenerate = false;
    [SerializeField] private GenerationType gentype;

    [Header("GEOMETRY")]
    [SerializeField] private int verticesWide = 10;
    [SerializeField] private int verticesLong = 10;
    [SerializeField] private float vertexSeperation = 10f;
    [SerializeField] private float heightScalar = 100f;
    [SerializeField] private Gradient gradient;

    [Header("NOISE")]
    [SerializeField] private float frequency = 0.01f;
    [SerializeField] private float seed = 0f;

    public void Start()
    {
        GenerateMesh();
    }

    public void OnValidate()
    {
        if (!regenerate) return;
        regenerate = false;
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        MeshGenerator generator = new MeshGenerator(verticesWide, verticesLong, vertexSeperation, new Vector3(0, 0, 0));
        TerrainHeightGenerator terrain = new TerrainHeightGenerator(generator.GetMesh(), heightScalar, frequency, this.seed);
        terrain.GenerateHeight(gentype);
        terrain.ColorTerrain(TerrainColoringType.BrownToGreen, gradient);
        GetComponent<MeshFilter>().mesh = terrain.GetMesh();
    }
}

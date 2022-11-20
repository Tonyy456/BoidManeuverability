using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Algorithm generationAlgorithm;
    [SerializeField] private float distance;
    [SerializeField] private int resolution;
    [SerializeField] private float frequency;
    [SerializeField] private float seed;
    [SerializeField] private float maxHeight;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Gradient gradient;

    private Algorithm algorithm;
    private ITerrainAlgorithm terrainGeneration;

    public void Start()
    {
        if (terrainGeneration == null) return;
        Mesh mesh = terrainGeneration.GenerateAndColor(gradient);
        meshFilter.mesh = mesh;
    }

    public void Awake()
    {
        switch(algorithm)
        {
            case Algorithm.HeightMapper:
                terrainGeneration = new HeightMapper(distance, resolution, frequency, maxHeight, seed);
                break;
            case Algorithm.MachingCubes:
                terrainGeneration = new HeightMapper(distance, resolution, frequency, maxHeight, seed);
                break;
            default:
                terrainGeneration = new HeightMapper(distance, resolution, frequency, maxHeight, seed);
                break;
        }
    }
}

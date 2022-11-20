using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 camPos = new Vector3(0, 0, 0);
    [SerializeField] private Quaternion rotation;

    [Header("GEOMETRY")]
    [SerializeField] private int resolution = 10;
    [SerializeField] private int size = 100;
    [SerializeField] private float heightScalar = 100f;

    [Header("SETTINGS")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private float frequency = 0.01f;
    [SerializeField] private float seed = 0f;

    public void Start()
    {
        GenerateMesh(new Vector3(0,0,0));
    }

    public void Update()
    {
        cam.transform.position = camPos;
        cam.transform.rotation = rotation;
    }

    private void GenerateMesh(Vector3 center) // works only once as of now
    {
        
        float vertexSeperation = ((float)size) / ((float)resolution);
        MeshGenerator generator = new MeshGenerator(resolution, resolution, vertexSeperation, center);
        TerrainHeightGenerator terrain = new TerrainHeightGenerator(generator.GetMesh(), heightScalar, frequency, this.seed);
        terrain.GenerateHeight(GenerationType.XSquared);
        terrain.ColorTerrain(gradient);
        GetComponent<MeshFilter>().mesh = terrain.GetMesh();
    }
}

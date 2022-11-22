using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(CubeOfCubesTest))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CubeOfCubesTest myScript = (CubeOfCubesTest)target;
        if (GUILayout.Button("Load"))
        {
            myScript.Reload();
        }
    }
}


public class CubeOfCubesTest : MonoBehaviour
{
    [SerializeField] private int resolution;
    [SerializeField] private float scale = 3;
    [SerializeField] private float frequency = 0.05f;
    [SerializeField] private float surface = 0.5f;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private MeshFilter filter;

    TriangulationCube[,,] cubes;
    float[,,] noise;
    Vertex[,,] vertices;

    public void Reload()
    {
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    noise[x, y, z] = PerlinNoise.PerlinNoise3D(x, y, z, frequency);
                    Color c = (noise[x, y, z] > surface) ? Color.white : Color.black;
                    vertices[x, y, z].SetColor(c);
                }
            }
        }
    }

    private void SetCubes()
    {

    }

    public void Start()
    {
        cubes = new TriangulationCube[resolution - 1, resolution - 1, resolution - 1];
        noise = new float[resolution, resolution, resolution];
        vertices = new Vertex[resolution, resolution, resolution];
        for(int x = 0; x < resolution - 1; x++)
        {
            for(int y = 0; y < resolution - 1; y++)
            {
               for (int z = 0; z < resolution - 1; z++)
                {
                    cubes[x, y, z] = new TriangulationCube(new Vector3(x * scale + scale/2, y * scale + scale/2, z * scale + scale/2), scale);;
                }
            }
        }

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    noise[x,y,z] = PerlinNoise.PerlinNoise3D(x , y , z , frequency);
                    var go = Instantiate(prefab);
                    go.transform.parent = parent.transform;
                    go.transform.position = new Vector3(x * scale, y * scale, z * scale);
                    vertices[x, y, z] = new Vertex(go);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GenerationSettings settings;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;

    List<Vector3> points = new List<Vector3>();
    public void Start()
    {
        
    }

    public void GenerateLots()
    {
        float pointSeperation = settings.distance / settings.resolution;
        for (int x = 0; x < settings.resolution; x++)
        {
            for (int y = 0; y < settings.resolution; y++)
            {
                for (int z = 0; z < settings.resolution; z++)
                {
                    points.Add(new Vector3(pointSeperation * x, pointSeperation * y, pointSeperation * z));
                }
            }
        }

        MarchingSquares squares = new MarchingSquares(settings, null);
        foreach (var p in points)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p;
            go.transform.parent = parent;
            float perlin = squares.PerlinNoise3D(p.x, p.y, p.z);
            float alpha = go.GetComponent<MeshRenderer>().material.color.a;
            go.GetComponent<MeshRenderer>().material.color = new Color(perlin, perlin, perlin, alpha);
        }
    }
}

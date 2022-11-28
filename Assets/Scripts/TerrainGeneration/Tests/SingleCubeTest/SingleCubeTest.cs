using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;
using UnityEditor;
using UnityEngine.Profiling;


[CustomEditor(typeof(SingleCubeTest))]
public class customButton2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SingleCubeTest myScript = (SingleCubeTest)target;
        if (GUILayout.Button("Load"))
        {
            myScript.Load();
        }
    }
}
public class SingleCubeTest : MonoBehaviour
{
    [Header("CUBE SETTINGS")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3Int dimensions;
    [SerializeField] private float seperation;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GameObject vertex;

    MarchingCubes mc;
    public void Start()
    {
        mc = new MarchingCubes(position, dimensions, seperation);
        GameObject parent = new GameObject("vertices");
        foreach (Vertex v in mc.graph.getVertices())
        {
            GameObject circle = Instantiate(vertex);
            circle.transform.position = v.Position;
            ToggleSphere comp = circle.AddComponent<ToggleSphere>();
            comp.Initialize(v, this);
            circle.transform.parent = parent.transform;
        }

        meshFilter.mesh = new Mesh();
        meshFilter.mesh.vertices = mc.graph.getMeshVertices();

        Load();  
    }

    public void Load()
    {
        mc.March();
        meshFilter.mesh.triangles = mc.Triangles;
        meshFilter.mesh.RecalculateNormals();
    }
}

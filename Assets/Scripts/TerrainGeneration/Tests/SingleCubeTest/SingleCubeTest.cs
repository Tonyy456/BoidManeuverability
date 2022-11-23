using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;
using UnityEditor;


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

    [Header("NOISE SETTINGS")]
    [SerializeField] private float frequency = 0.05f;
    [SerializeField] private float surface = 0.5f;

    public void Start()
    {
        Load();  
    }

    public void Load()
    {
        MarchingCubes mc = new MarchingCubes(position, dimensions, seperation);
        mc.March(meshFilter, frequency, surface);
        //mc.DisplayVertices(1f);
    }
}

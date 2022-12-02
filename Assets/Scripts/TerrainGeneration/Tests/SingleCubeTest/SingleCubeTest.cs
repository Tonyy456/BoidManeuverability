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

    MCCubes mc;
    public void Start()
    {

    }

    public void Load()
    {

    }
}

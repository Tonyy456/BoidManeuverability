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

    private MarchingCubes MC;
    public void Start()
    {
        MC = new MarchingCubes(this.transform.position, resolution * scale, resolution);
    }

    public void Reload()
    {
        MC.March(filter, frequency, surface);
        MC.ShowVertices();
    }

}

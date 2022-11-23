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
    public void Reload()
    {

    }
}

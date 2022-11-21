using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    private GameObject go;
    public Vertex(GameObject go)
    {
        this.go = go;
    }

    public void SetColor(Color c)
    {
        go.GetComponent<MeshRenderer>().material.color = c;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVertex : MonoBehaviour
{
    [SerializeField] private bool isOn = false;
    Vertex vertex;
    public void Init(Vertex v)
    {
        vertex = v;
    }

    public void Update()
    {
        if(vertex != null)
            vertex.IsOn = isOn;
    }
}

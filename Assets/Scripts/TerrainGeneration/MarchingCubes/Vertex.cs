using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public bool IsOn;
    public Vector3 Position;
    public Vertex(Vector3 position, bool isOn)
    {
        Position = position;
        IsOn = isOn;
    }

}

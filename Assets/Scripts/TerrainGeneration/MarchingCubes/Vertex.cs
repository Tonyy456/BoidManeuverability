using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public bool IsOn { get; set; }
    public Vector3 Position { get; set; }

    public Vertex(Vector3 position, bool isOn)
    {
        Position = position;
        IsOn = isOn;
    }

}

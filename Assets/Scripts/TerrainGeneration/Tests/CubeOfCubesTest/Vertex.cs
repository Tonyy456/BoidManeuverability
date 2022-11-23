using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    private GameObject go;
    private Vector3 position;
    private bool isOn;

    public Vertex(Vector3 position, bool isOn)
    {
        this.position = position;
        this.isOn = isOn;
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public bool IsOn {
        get
        {
            return isOn;
        }
        set
        {
            isOn = value;
            UpdateVertexColor();
        }
    }

    public void ToggleVertexOn(float sphereSize = 1f)
    {
        if (go == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            go.transform.position = position;
            var comp = go.AddComponent<AVertex>();
            comp.Init(this);
        }
        else
        {
            GameObject.Destroy(go);
        }
        UpdateVertexColor();
    }

    private void UpdateVertexColor()
    {
        if (go == null) return;
        Color c = isOn ? Color.white : Color.black;
        go.transform.GetComponent<MeshRenderer>().material.color = c;
    }


}

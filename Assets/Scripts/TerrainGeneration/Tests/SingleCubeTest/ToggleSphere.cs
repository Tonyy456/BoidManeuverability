using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSphere : MonoBehaviour
{
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.black;
    [SerializeField] private bool isOn;

    private Vertex v;
    private SingleCubeTest testScript;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            OnValueChange();
        }
    }
    public void OnValidate()
    {
        OnValueChange();
    }

    public void Initialize(Vertex v, SingleCubeTest testScript)
    {
        this.v = v;
        isOn = false;
        v.IsOn = false;
        this.testScript = testScript;
        OnValueChange();
    }

    private void OnMouseDown()
    {
        IsOn = !IsOn;
    }

    private void OnValueChange()
    {
        if (isOn)
            this.GetComponent<MeshRenderer>().material.color = onColor;
        else
            this.GetComponent<MeshRenderer>().material.color = offColor;
        if(v != null)
        {
            v.IsOn = isOn;
            testScript.Load();
        }
    }
}

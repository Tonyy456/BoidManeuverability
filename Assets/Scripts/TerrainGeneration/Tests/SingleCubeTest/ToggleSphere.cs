using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSphere : MonoBehaviour
{
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.black;
    [SerializeField] private bool isOn;
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

    public void Initialize(SingleCubeTest testScript)
    {

        isOn = false;
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
        if(testScript != null)
            testScript.Load();
    }
}

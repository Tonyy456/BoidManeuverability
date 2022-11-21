using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVertex : MonoBehaviour
{
    [SerializeField] private bool isOn = false;
    public int IsOn
    {
        get
        {
            return isOn ? 1 : 0;
        }
    }

    public int index = 0;


    MeshRenderer rend;
    public void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        if (isOn)
        {
            rend.material.color = Color.white;
        } else
        {
            rend.material.color = Color.black;
        }
    }
}

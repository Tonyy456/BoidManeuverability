using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By: Taylor Liu
public class FollowCam : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float distanceAway;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (!target)
            FindTarget();
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    void FindTarget() {
        target = GameObject.FindWithTag("Soid").GetComponent<Transform>();
    }
}

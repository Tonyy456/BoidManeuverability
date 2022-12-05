using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float distanceAway;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lookTime;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!target)
            FindTarget();
        transform.position = target.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, lookTime);
    }

    void FindTarget() {
        target = GameObject.FindWithTag("Soid").GetComponent<Transform>();
    }
}

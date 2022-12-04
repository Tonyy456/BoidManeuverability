using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distanceAway;
    [SerializeField] Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
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

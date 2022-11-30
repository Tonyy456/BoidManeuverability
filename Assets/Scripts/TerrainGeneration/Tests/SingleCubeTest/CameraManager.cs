using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("0 = top left, 1 = top right, 2 = bottom left, 3 = bottom right")]
    [SerializeField] private List<GameObject> cams;
    public void EnableTopLeft()
    {
        foreach (var cam in cams) SetCamera(cam, false);
        SetCamera(cams[0], true);
    }

    public void EnableTopRight()
    {
        foreach (var cam in cams) SetCamera(cam, false);
        SetCamera(cams[1], true);
    }

    public void EnableBottomLeft()
    {
        foreach (var cam in cams) SetCamera(cam, false);
        SetCamera(cams[2], true);
    }

    public void EnableBottomRight()
    {
        foreach (var cam in cams) SetCamera(cam, false);
        SetCamera(cams[3], true);
    }
    private void SetCamera(GameObject cam, bool on)
    {
        cam.SetActive(true);
    }
}

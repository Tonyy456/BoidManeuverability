using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void MarchingCubes()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CubesScene");
    }

    public void HeightMapper()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HeightMapper");
    }
}

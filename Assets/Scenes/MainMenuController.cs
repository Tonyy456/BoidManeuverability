using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartSimulation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TerrainGeneration");
    }
}

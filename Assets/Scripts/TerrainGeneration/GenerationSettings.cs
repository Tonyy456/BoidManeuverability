using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("Prefab with Mesh Filter and Mesh Renderer")]
        [SerializeField] public GameObject meshObject;

        [Header("GENERATION")]
        [SerializeField] public Algorithm generationAlgorithm;
        [SerializeField] public Gradient HeightColorGradient;
        [SerializeField] public float distance;
        [SerializeField] public int resolution;

        [Header("NOISE")]
        [SerializeField] public float frequency;
        [SerializeField] public float maxHeight;
        [SerializeField] public float seed;
    }
}

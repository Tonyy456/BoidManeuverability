using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("GENERATION")]
        [SerializeField] public Gradient HeightColorGradient;
        [SerializeField] public float distance = 100;
        [SerializeField] public int resolution = 10;

        [Header("NOISE")]
        [SerializeField] public float frequency = 0.005f;
        [SerializeField] public float maxHeight = 100;
        [SerializeField] public float seed = 10000;
    }
}

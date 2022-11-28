using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [SerializeField] public Gradient HeightColorGradient;
        [SerializeField] public Vector3 center;
        [SerializeField] public Vector3Int resolution;
        [SerializeField] public float pointSeperation;
        [SerializeField] public float frequency = 0.005f;
        [SerializeField] public float surface = 0.5f;
        [SerializeField] public float maxHeight = 100;
        [SerializeField] public float seed = 10000;
    }
}

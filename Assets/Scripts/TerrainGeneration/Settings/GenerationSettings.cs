using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("CHUNK DEFINITION")]
        [Tooltip("The center of the 0,0,0 Chunk")]
        [SerializeField] public Vector3 center;
        [Tooltip("The number of points in each dimensions that a chunk is defined as")]
        [SerializeField] public Vector3Int chunkDimensions;
        [Tooltip("The distance between each point")]
        [SerializeField] public float pointSeperation;

        [Header("NOISE SETTINGS")]
        [SerializeField] public float frequency = 0.005f;
        [SerializeField] public float surface = 0.5f;
        [SerializeField] public float heightMultipler = 100;
        [SerializeField] public float seed = 10000;

        [Header("OLD SETTINGS, SAVED FOR EMERGENCY")]
        [SerializeField] public Gradient HeightColorGradient;
        [SerializeField] public Vector3Int resolution;

        public Vector3 chunkSize()
        {
            Vector3 size = chunkDimensions;
            size *= pointSeperation;
            return size;
        }
    }
}

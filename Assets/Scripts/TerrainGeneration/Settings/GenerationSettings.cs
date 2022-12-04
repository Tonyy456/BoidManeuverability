using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [SerializeField] public Gradient ColorGradient;
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

        public Vector3 chunkSize()
        {
            Vector3 size = chunkDimensions - new Vector3Int(1,1,1);
            size *= pointSeperation;
            return size;
        }

        public Vector3 chunkCenter(Vector3Int chunk)
        {
            Vector3 center = this.center + Vector3.Scale(chunk, chunkDimensions - new Vector3Int(1,1,1)) * pointSeperation;
            return center;
        }

        public Vector3Int chunkAtVector(Vector3 position)
        {
            int x = Mathf.RoundToInt((position.x - center.x) / pointSeperation / (chunkDimensions.x - 1));
            Debug.Log(x);
            return new Vector3Int();
        }
    }
}

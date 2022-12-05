using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    [CreateAssetMenu(fileName = "generationSettings", menuName = "Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("NOISE SETTINGS")]
        [SerializeField] public float frequency = 0.005f;
        [SerializeField] public float surface = 0.5f;
        [SerializeField] public float heightMultipler = 100;
        [SerializeField] public float seed = 10000;
        [SerializeField] public Gradient ColorGradient;

        [Header("CHUNK DEFINITION")]
        [SerializeField] public Vector3 gridCenter;
        [SerializeField] public Vector3Int cubesPerChunk;
        [SerializeField] public Vector3Int chunksPerGrid = new Vector3Int(5, 5, 5);
        [SerializeField] public float edgeLen;


        public Vector3 gridDimensions
        {
            get => Vector3.Scale(cubesPerChunk, chunksPerGrid) * edgeLen;
        }

        public Vector3 minimumCorner
        {
            get => this.gridCenter - gridDimensions / 2f;
        }

        public Vector3 chunkSize
        {
            get => (Vector3)cubesPerChunk * edgeLen;
        }

        public Vector3Int resolution
        {
            get => cubesPerChunk + new Vector3Int(1, 1, 1);
        }

        public Vector3 chunkCenter(Vector3Int chunk)
        {
            Vector3 center = minimumCorner + Vector3.Scale((Vector3) chunk, cubesPerChunk) * edgeLen + chunkSize / 2f;
            return center;
        }
    }
}

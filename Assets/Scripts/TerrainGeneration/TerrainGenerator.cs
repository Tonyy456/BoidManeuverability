using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GenerationSettings settings;
        [SerializeField] private MeshFilter meshObject;
        [SerializeField] private Algorithm generationType = Algorithm.HeightMapper;

        private ITerrainAlgorithm algorithm;

        public void Start()
        {
            switch (generationType)
            {
                default:
                    algorithm = new HeightMapper(settings, meshObject);
                    break;
            }
            algorithm.Generate(new Vector3(0, 0, 0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshObject;
        [SerializeField] private Algorithm generationType = Algorithm.HeightMapper;
        [SerializeField] private GenerationSettings HeightMapSettings;
        [SerializeField] private GenerationSettings CubeSettings;

        private ITerrainAlgorithm algorithm;
        private GenerationSettings settings;

        public void Start()
        {
            switch (generationType)
            {
                case Algorithm.HeightMapper:
                    settings = HeightMapSettings;
                    algorithm = new HeightMapper(settings, meshObject, new Vector3());
                    break;
                case Algorithm.MachingCubes:
                    settings = CubeSettings;
                    algorithm = new MarchingCubes(settings, meshObject);
                    break;           
            }
            IEnumerator routine = algorithm.Generate();
            StartCoroutine(routine);
        }
    }
}

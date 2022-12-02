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
                    algorithm = new HeightMapper(settings, meshObject, new Vector3());
                    settings = HeightMapSettings;
                    break;
                case Algorithm.MachingCubes:
                    algorithm = new MarchingCubes(settings, meshObject);
                    settings = CubeSettings;
                    break;           
            }
            IEnumerator routine = algorithm.Generate();
            StartCoroutine(routine);
        }
    }
}

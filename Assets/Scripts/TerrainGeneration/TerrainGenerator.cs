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

        public void Start()
        {
            switch (generationType)
            {
                case Algorithm.HeightMapper:
                    algorithm = new HeightMapper(HeightMapSettings, meshObject);
                    break;
                case Algorithm.MachingCubes:
                    algorithm = new MCGeneration(CubeSettings, meshObject);
                    break;
                
            }
            algorithm.Generate(new Vector3());
        }
    }
}

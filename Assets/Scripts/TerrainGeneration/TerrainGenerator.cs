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
        [SerializeField] private TMPro.TMP_Text text;

        private ITerrainAlgorithm algorithm;
        private GenerationSettings settings;
        private IEnumerator enumerator;

        public void Start()
        {
            switch (generationType)
            {
                case Algorithm.HeightMapper:
                    algorithm = new HeightMapper(meshObject);
                    settings = HeightMapSettings;
                    break;
                case Algorithm.MachingCubes:
                    MarchingCubes mc = new MarchingCubes(meshObject)
                    {
                        text = this.text
                    };
                    algorithm = mc;
                    settings = CubeSettings;
                    enumerator = mc.StartMarches();
                    break;           
            }
            algorithm.Generate(new Vector3(), settings);
            if(enumerator != null)
            {
                StartCoroutine(enumerator);
            }
        }
    }
}

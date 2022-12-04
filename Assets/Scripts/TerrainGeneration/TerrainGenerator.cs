using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshObject;
        [SerializeField] private GameObject meshPrefab;
        [SerializeField] private Algorithm generationType = Algorithm.HeightMapper;
        [SerializeField] private GenerationSettings HeightMapSettings;
        [SerializeField] private GenerationSettings CubeSettings;
        [SerializeField] private Vector3Int chunksInEachDimension;

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
            Mesh mesh = new Mesh();
            int index = 0;
            for(int i = 0; i < chunksInEachDimension.x; i++)
            {
                for(int j = 0; j < chunksInEachDimension.y; j++)
                {
                    for(int k = 0; k < chunksInEachDimension.z; k++)
                    {
                        GameObject prefab = Instantiate(meshPrefab);
                        MeshFilter filter = prefab.GetComponent<MeshFilter>();
                        Vector3Int chunk = new Vector3Int(i, j, k);        
                        IEnumerator enumer = algorithm.Generate(chunk, filter);
                        StartCoroutine(enumer);
                    }
                }
            }
        }
    }
}

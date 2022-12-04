using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject meshPrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private Algorithm generationType = Algorithm.HeightMapper;
        [SerializeField] private GenerationSettings HeightMapSettings;
        [SerializeField] private GenerationSettings CubeSettings;
        [SerializeField] private Vector3Int chunksInEachDimension;

        [SerializeField] private Transform objToWatch;
        [SerializeField] private int distanceToLoad;

        private ITerrainAlgorithm algorithm;
        private GenerationSettings settings;

        private Dictionary<Vector3Int, GameObject> gameObjects;

        public void Start()
        {
            switch (generationType)
            {
                case Algorithm.HeightMapper:
                    settings = HeightMapSettings;
                    algorithm = new HeightMapper(settings, null, new Vector3());
                    break;
                case Algorithm.MachingCubes:
                    settings = CubeSettings;
                    algorithm = new MarchingCubes(settings);
                    break;           
            }

            Mesh mesh = new Mesh();
            for(int i = 0; i < chunksInEachDimension.x; i++)
            {
                for(int j = 0; j < chunksInEachDimension.y; j++)
                {
                    for(int k = 0; k < chunksInEachDimension.z; k++)
                    {
                        GameObject prefab = Instantiate(meshPrefab);
                        MeshFilter filter = prefab.GetComponent<MeshFilter>();
                        prefab.transform.parent = transform;
                        prefab.name = $"{i},{j},{k}";
                        Vector3Int chunk = new Vector3Int(i, j, k);        
                        IEnumerator enumer = algorithm.Generate(chunk, filter);
                        StartCoroutine(enumer);
                    }
                }
            }
        }

        public void Update()
        {
            settings.chunkAtVector(transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject chunkMeshPrefab;
        [SerializeField] private Transform meshParent;
        [SerializeField] private Algorithm generationType = Algorithm.HeightMapper;
        [SerializeField] private GenerationSettings settings;
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private Transform player;
        [SerializeField] private Material boundsMaterial;
        [SerializeField] private float delay;

        private ITerrainAlgorithm algorithm;

        public void Start()
        {
            switch (generationType)
            {
                case Algorithm.HeightMapper:
                    var map = new HeightMapper(settings);
                    map.OnGenerationComplete += HeightMapColor;
                    map.OnGenerationComplete += DrawCubes;
                    algorithm = map;
                    break;
                case Algorithm.MachingCubes:
                    var cubes = new MarchingCubes(settings);
                    cubes.OnGenerationComplete += DrawCubes;
                    algorithm = cubes;
                    break;           
            }
            StartCoroutine(GenerateTerrain());
        }
        private void HeightMapColor()
        {
            HeightMapper mapper = (HeightMapper)algorithm;
            for(int i = 0; i < meshParent.childCount; i++)
            {
                var child = meshParent.GetChild(i);
                Mesh mesh = child.GetComponent<MeshFilter>().mesh;
                IMeshColorer colorer = new HeightMeshColorer(
                    mesh,
                    settings.ColorGradient, (mapper.minimumPoint,
                    mapper.maximumPoint));
                mesh.Optimize();
                colorer.Color();
            }
        }
        private void DrawCubes()
        {
            player.transform.position = spawnPoint;
            algorithm.CreateBounds(boundsMaterial);
        }

        public IEnumerator GenerateTerrain()
        {
            Mesh mesh = new Mesh();
            Vector3Int dim = settings.chunksPerGrid;
            for (int i = 0; i < dim.x; i++)
            {
                for (int j = 0; j < dim.y; j++)
                {
                    for (int k = 0; k < dim.z; k++)
                    {
                        GameObject prefab = Instantiate(chunkMeshPrefab);
                        MeshFilter filter = prefab.GetComponent<MeshFilter>();
                        prefab.transform.parent = meshParent;
                        prefab.name = $"Mesh:{i},{j},{k}";

                        Vector3Int chunk = new Vector3Int(i, j, k);
                        IEnumerator enumer;
                        if (i == dim.x - 1 && j == dim.y - 1 && k == dim.z - 1)
                             enumer = algorithm.Generate(chunk, filter, prefab.GetComponent<MeshCollider>(), true);
                        else 
                            enumer = algorithm.Generate(chunk, filter, prefab.GetComponent<MeshCollider>());
                        StartCoroutine(enumer);

                        yield return new WaitForSeconds(delay);
                    }
                }
            }
            yield return null;
        }
    }
}

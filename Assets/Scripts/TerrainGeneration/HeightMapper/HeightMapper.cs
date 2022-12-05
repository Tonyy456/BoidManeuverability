using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private GenerationSettings settings;
        public HeightMapper(GenerationSettings settings)
        {
            this.settings = settings;
        }

        public delegate void Complete();
        public Complete OnGenerationComplete;

        public float minimumPoint = float.MaxValue;
	public float maximumPoint = float.MinValue;
        public void DrawBounds() { }
      
        /*
         * Creates a shell around the entire play area
         */
        public void CreateBounds(Material material)
        {
	    float min = minimumPoint;
            float max = maximumPoint;
	    max *= 2f;
            Vector3 center = settings.gridCenter;
            center.y = (min + max)/2f;
            Vector3 dimensions = settings.gridDimensions;
	    dimensions.y = (max - min);
            

            RectangularPrism cube = new RectangularPrism(center, dimensions);
            foreach (var pair in cube.getSurfaces())
            { //creates cube at the specified surface scale in the cube surface
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var comp = go.transform.GetComponent<MeshRenderer>();
                comp.material = material;
                Vector3 scale = pair.scale;
                if (scale.x == 0) scale.x = 1;
                if (scale.y == 0) scale.y = 1;
                if (scale.z == 0) scale.z = 1;
                go.transform.localScale = scale;
                go.transform.position = pair.center;
            }
        }


        public IEnumerator Generate(Vector3Int chunk, MeshFilter filter, MeshCollider collider, bool signal = false)
        {
            FlatMeshGenerator generator = new FlatMeshGenerator(settings.chunkCenter(chunk), settings.cubesPerChunk + new Vector3Int(1,1,1), settings.edgeLen);
            var definition = generator.getMeshDefintion();
            Vector3[] vertices = addNoise(definition.vertices);
            int[] triangles = definition.triangles;

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            foreach(var v in vertices)
            {
                if (v.y < minimumPoint)
                    minimumPoint = v.y;
		if (v.y > maximumPoint)
		    maximumPoint = v.y;
            }
            mesh.triangles = triangles;

            PostMeshCreation(mesh, collider);

            if (signal && OnGenerationComplete != null) OnGenerationComplete();

            filter.mesh = mesh;
            yield return null;
        }

        private void PostMeshCreation(Mesh mesh, MeshCollider collider)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            mesh.Optimize();

            IMeshColorer colorer = new HeightMeshColorer(mesh, settings.ColorGradient);

            collider.sharedMesh = mesh;
            colorer.Color();
        }

        private Vector3[] addNoise(Vector3[] points)
        {
            Vector3[] noise = new Vector3[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                Vector3 point_with_noise = points[i];
                point_with_noise.y = PerlinNoise.Noise2D(point_with_noise, settings.frequency, settings.seed);
                point_with_noise.y *= settings.heightMultipler;
                noise[i] = point_with_noise;
            }
            return noise;
        }
    }
}

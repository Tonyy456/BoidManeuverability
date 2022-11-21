using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
    public static float PerlinNoise3D(float x, float y, float z, float frequency = 1)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y , frequency);
        float xz = _perlin3DFixed(x, z , frequency);
        float yz = _perlin3DFixed(y, z , frequency);
        float yx = _perlin3DFixed(y, x , frequency);
        float zx = _perlin3DFixed(z, x , frequency);
        float zy = _perlin3DFixed(z, y , frequency);
        return xy * xz * yz * yx * zx * zy;
    }
    public static float _perlin3DFixed(float a, float b, float frequency)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a * frequency , b * frequency));
    }
}

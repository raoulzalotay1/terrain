using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainValues : MonoBehaviour
{
    public FastNoiseSIMDUnity myNoise;
    public long seed = 1;
    public int size = 1;
    public float surfaceValue = .5f;
    public float zoom = 1;
    public float heightMultiplier = 1;

    public bool autoUpdate;
    public bool interpolation;
    public bool shading;

    public static FastNoiseSIMDUnity MyNoise;
    public static long Seed = 1;
    public static int Size = 1;
    public static float SurfaceValue = .5f;
    public static float Zoom = 1;
    public static float HeightMultiplier = 1;

    public static bool AutoUpdate;
    public static bool Interpolation;
    public static bool Shading;

    void Start()
    {
        MyNoise = myNoise;
        Seed = seed;
        Size = size;
        SurfaceValue = surfaceValue;
        Zoom = zoom;
        HeightMultiplier = heightMultiplier;
        AutoUpdate = autoUpdate;
        Interpolation = interpolation;
        Shading = shading;
    }
}

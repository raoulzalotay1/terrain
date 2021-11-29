using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class CellarNoise
{
    private long seed;

    public CellarNoise(long seed)
    {
        this.seed = seed;
    }

    public float GetCellularNoise(Vector3 pos)
    {
        Vector3[][][] points = this.CreateArray();
        Random rand = new Random((int)(this.seed));

        for (int z = 0; z < 3; z++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    points[x][y][z] = new Vector3((float)(rand.NextDouble() + x) * x, (float)(rand.NextDouble() + y) * y, (float)(rand.NextDouble() + z) * z);
                }
            }
        }

        return this.GetDistance(points, pos) - .4f;
    }

    private Vector3[][][] CreateArray()
    {
        Vector3[][][] arr = new Vector3[3][][];
        for (int i = 0; i < 3; i++)
        {
            arr[i] = new Vector3[3][];
            for (int j = 0; j < 3; j++)
            {
                arr[i][j] = new Vector3[3];
            }
        }

        return arr;
    }

    private float GetDistance(Vector3[][][] points, Vector3 pos)
    {
        float minValue = 100;

        for (int z = 0; z < 3; z++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    float distance = this.distance(points[x][y][z], pos);
                    if (distance < minValue)
                    {
                        minValue = distance;
                    }
                }
            }
        }

        return minValue;
    }

    float distance(Vector3 pos1, Vector3 pos2)
    {
        return (float)(Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.y - pos2.y, 2) + Math.Pow(pos1.z - pos2.z, 2));
    }
}

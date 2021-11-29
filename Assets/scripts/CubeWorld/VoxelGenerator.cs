using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator
{
    public static void generateMesh(int Size, Vector3 chunkPos, Cube[][][] cubes, out Vector3[] outVertices, out int[] outTriangles, out Color[] outColors)
    {
        List<Vector3> verticeList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        List<Color> colorList = new List<Color>();

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < Size; k++)
                {
                    if (cubes[i][j][k] != null)
                    {
                        setBlock(i, j, k, cubes, verticeList, triangleList, colorList, chunkPos, out verticeList, out triangleList, out colorList);
                    }
                }
            }
        }

        outVertices = verticeList.ToArray();
        outTriangles = triangleList.ToArray();
        outColors = colorList.ToArray();
    }

    public static Cube[][][] noiseBools(int Size, FastNoiseSIMDUnity fastNoise, float surfaceValue, Vector3 ChunkPos, FastNoiseSIMDUnity caveNoise)
    {
        float[] rawNoise = fastNoise.fastNoiseSIMD.GetNoiseSet((int)(ChunkPos.x * Size), (int)(ChunkPos.y * Size), (int)(ChunkPos.z * Size), Size, Size, Size);
        float[] rawCaveNoise = caveNoise.fastNoiseSIMD.GetNoiseSet((int)(ChunkPos.x * Size), (int)(ChunkPos.y * Size), (int)(ChunkPos.z * Size), Size, Size, Size);

        Cube[][][] cubes = new Cube[Size][][];

        for (int i = 0; i < Size; i++)
        {
            cubes[i] = new Cube[Size][];
            for (int j = 0; j < Size; j++)
            {
                cubes[i][j] = new Cube[Size];
            }
        }

        int index = 0;

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < Size; k++)
                {
                    if (-j - ChunkPos.y * Size + rawNoise[index] * 30 >= surfaceValue && rawCaveNoise[index] - .9f < surfaceValue)
                    {
                        cubes[i][j][k] = new Cube(i,j,k);
                    }
                    index++;
                }
            }
        }

        return cubes;
    }

    public static void SetColors(CubeWorld cw)
    {
        for (int i = 0, index = 0; i < cw.Size; i++)
        {
            for (int j = 0; j < cw.Size; j++)
            {
                for (int k = 0; k < cw.Size; k++)
                {
                    if (j != cw.Size - 1)
                    {
                        if (cw.blocks[i][j][k] != null && cw.blocks[i][j + 1][k] != null)
                        {
                            cw.blocks[i][j][k].Color = Color.gray;
                        }
                        index++;
                    }
                    else
                    {
                        if (cw.blocks[i][j][k] != null & CubeWorld.Chunks.TryGetValue(new Vector3(cw.ChunkPos.x, cw.ChunkPos.y + 1, cw.ChunkPos.z), out CubeWorld newCw))
                        {
                            if (newCw.blocks[i][0][k] != null)
                            {
                                cw.blocks[i][j][k].Color = Color.gray;
                            }
                        }
                    }
                }
            }
        }
    }

    private static void setBlock(int x, int y, int z, Cube[][][] cubes, List<Vector3> vertices, List<int> triangles, List<Color> colors, Vector3 chunkPos, out List<Vector3> newVertices, out List<int> newTriangles, out List<Color> newColor)
    {
        if (!tryGetBool(cubes, x, y, z - 1, (int) chunkPos.x, (int) chunkPos.y, (int) chunkPos.z))
        {
            drawSquare(new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z), new Vector3(x + 1, y, z), new Vector3(x, y, z), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }
        if (!tryGetBool(cubes, x, y, z + 1, (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z))
        {
            drawSquare(new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1), new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }
        if (!tryGetBool(cubes, x - 1, y, z, (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z))
        {
            drawSquare(new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x, y, z), new Vector3(x, y, z + 1), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }
        if (!tryGetBool(cubes, x + 1, y, z, (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z))
        {
            drawSquare(new Vector3(x + 1, y + 1, z + 1), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y, z), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }
        if (!tryGetBool(cubes, x, y + 1, z, (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z))
        {
            drawSquare(new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }
        if (!tryGetBool(cubes, x, y - 1, z, (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z))
        {
            drawSquare(new Vector3(x + 1, y, z), new Vector3(x, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x, y, z + 1), cubes[x][y][z], vertices, triangles, colors, out vertices, out triangles, out colors);
        }

        newVertices = vertices;
        newTriangles = triangles;
        newColor = colors;
    }

    private static void drawSquare(Vector3 tr, Vector3 tl, Vector3 br, Vector3 bl, Cube cube, List<Vector3> vertices, List<int> triangles, List<Color> colors, out List<Vector3> newVertices, out List<int> newTriangles, out List<Color> newColors)
    {
        colors.Add(cube.Color);
        colors.Add(cube.Color);
        colors.Add(cube.Color);
        colors.Add(cube.Color);

        int startValue = vertices.Count;

        vertices.Add(tr);
        vertices.Add(tl);
        vertices.Add(br);
        vertices.Add(bl);

        triangles.Add(startValue);
        triangles.Add(startValue + 3);
        triangles.Add(startValue + 1);
        triangles.Add(startValue);
        triangles.Add(startValue + 2);
        triangles.Add(startValue + 3);

        newVertices = vertices;
        newTriangles = triangles;
        newColors = colors;
    }

    private static bool tryGetBool(Cube[][][] table, int x, int y, int z, int chunkX, int chunkY, int chunkZ)
    {
        if (x < 0)
        {
            Vector3 key = new Vector3(chunkX - 1, chunkY, chunkZ);
            if (CubeWorld.Chunks.ContainsKey(key))
            {
                if (CubeWorld.Chunks[key].blocks[table.Length - 1][y][z] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        if (x >= table.Length)
        {
            Vector3 key = new Vector3(chunkX + 1, chunkY, chunkZ);
            if (CubeWorld.Chunks.TryGetValue(key, out CubeWorld cw))
            {
                if (cw.blocks[0][y][z] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        if (y < 0)
        {
            Vector3 key = new Vector3(chunkX, chunkY - 1, chunkZ);
            if (CubeWorld.Chunks.ContainsKey(key))
            {
                if (CubeWorld.Chunks[key].blocks[x][table.Length - 1][z] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        if (y >= table.Length)
        {
            Vector3 key = new Vector3(chunkX, chunkY + 1, chunkZ);
            if (CubeWorld.Chunks.ContainsKey(key))
            {
                if (CubeWorld.Chunks[key].blocks[x][0][z] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        if (z < 0)
        {
            Vector3 key = new Vector3(chunkX, chunkY, chunkZ - 1);
            if (CubeWorld.Chunks.ContainsKey(key))
            {
                if (CubeWorld.Chunks[key].blocks[x][y][table.Length - 1] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        if (z >= table.Length)
        {
            Vector3 key = new Vector3(chunkX, chunkY, chunkZ + 1);
            if (CubeWorld.Chunks.ContainsKey(key))
            {
                if (CubeWorld.Chunks[key].blocks[x][y][0] != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        if (table[x][y][z] != null)
        {
            return true;
        }

        return false;
    }
}

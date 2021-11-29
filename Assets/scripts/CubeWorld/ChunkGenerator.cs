using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public FastNoiseSIMDUnity FastNoise;
    public FastNoiseSIMDUnity CaveNoise;

    public Material GroundColor;
    public Transform Parent;

    public int ChunkSize;

    public GameObject player;

    public int renderDistance;

    void Start()
    {
        //for (int i = -1; i < 3; i++)
        //{
        //    for (int j = -1; j < 3; j++)
        //    {
        //        for (int k = -1; k < 3; k++)
        //        {
        //            new CubeWorld(new Vector3(i, j, k), FastNoise, CaveNoise, 0, GroundColor, Parent, ChunkSize);
        //        }
        //    }
        //}
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 playerChunkPos = new Vector3(Mathf.Floor(playerPos.x / ChunkSize), Mathf.Floor(playerPos.y / ChunkSize), Mathf.Floor(playerPos.z / ChunkSize));

        deleteChunksFarAway(playerChunkPos);

        for (int x = -renderDistance + 1; x < renderDistance; x++)
        {
            for (int y = -renderDistance + 1; y < renderDistance; y++)
            {
                for (int z = -renderDistance + 1; z < renderDistance; z++)
                {
                    Vector3 temp = new Vector3(playerChunkPos.x + x, playerChunkPos.y + y, playerChunkPos.z + z);
                    if (!CubeWorld.Chunks.ContainsKey(temp))
                    {
                        CubeWorld.createChunk(temp, FastNoise, CaveNoise, 0, GroundColor, Parent, ChunkSize);
                    }
                }
            }
        }
    }

    private void deleteChunksFarAway(Vector3 playerChunkPos)
    {
        List<CubeWorld> cwToRemove = new List<CubeWorld>();

        int chunkCount = 0;

        foreach (var chunk in CubeWorld.Chunks)
        {
            if (chunk.Value.ChunkPos.x >= playerChunkPos.x + renderDistance || chunk.Value.ChunkPos.x <= playerChunkPos.x - renderDistance)
            {
                cwToRemove.Add(chunk.Value);
                CubeWorld.Chunks.Remove(chunk.Key);
                chunkCount++;
            }
            if (chunk.Value.ChunkPos.y >= playerChunkPos.y + renderDistance || chunk.Value.ChunkPos.y <= playerChunkPos.y - renderDistance)
            {
                cwToRemove.Add(chunk.Value);
                CubeWorld.Chunks.Remove(chunk.Key);
                chunkCount++;
            }
            if (chunk.Value.ChunkPos.z >= playerChunkPos.z + renderDistance || chunk.Value.ChunkPos.z <= playerChunkPos.z - renderDistance)
            {
                cwToRemove.Add(chunk.Value);
                CubeWorld.Chunks.Remove(chunk.Key);
                chunkCount++;
            }
        }

        foreach (var cubeWorld in cwToRemove.ToArray())
        {
            Destroy(cubeWorld);
        }

        cwToRemove = new List<CubeWorld>();
    }
}

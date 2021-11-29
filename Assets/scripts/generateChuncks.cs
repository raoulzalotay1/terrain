using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets;

using UnityEngine;
using UnityEngine.Animations;
using VoxelEngine;

public class generateChuncks : MonoBehaviour
{
    public static int RenderDistance = 2;
    public static GameObject parent;

    private List<Vector3> chunkList = new List<Vector3>();

    void Start()
    {

    }

    void FixedUpdate()
    {
        terrainValues terrainValues = GameObject.FindGameObjectWithTag("terrainValues").GetComponent<terrainValues>();
        Vector3 player = new Vector3();//GameObject.FindGameObjectWithTag("Player").transform.position;
        CreateChunk chunckGenerator = GameObject.FindGameObjectWithTag("ChunckGenerator").GetComponent<CreateChunk>();

        int x = (player.x < 0 ? (int)player.x - terrainValues.Size : (int)player.x) / terrainValues.Size;
        int y = (int)player.y / terrainValues.Size;
        int z = (player.z < 0 ? (int)player.z - terrainValues.Size : (int)player.z) / terrainValues.Size;

        for (int zCount = -RenderDistance + 1; zCount < RenderDistance; zCount++)
        {
            for (int yCount = -RenderDistance + 1; yCount < RenderDistance; yCount++)
            {
                for (int xCount = -RenderDistance + 1; xCount < RenderDistance; xCount++)
                {
                    GenerateSingleChunk(x + xCount, y + yCount, z + zCount, chunckGenerator, terrainValues);
                }
            }
        }
    }

    public void GenerateSingleChunk(int x, int y, int z, CreateChunk chunckGenerator, terrainValues terrainValues)
    {


        if (chunkList.Contains(new Vector3(x, y, z)))
        {
            //chunk.ChunkGameObject.GetComponent<MarchinCubes>().UpdateChunk();
        }
        else
        {
            chunkList.Add(new Vector3(x, y, z));
            CreateChunk.chunkQueue.Add(new Vector3(x, y, z));
        }
    }
}

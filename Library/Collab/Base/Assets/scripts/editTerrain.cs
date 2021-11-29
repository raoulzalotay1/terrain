using System;
using System.Collections;
using System.Collections.Generic;

using Assets;

using UnityEngine;

public class editTerrain : MonoBehaviour
{
    void Update()
    {
        terrainValues terrainValues = GameObject.FindGameObjectWithTag("terrainValues").GetComponent<terrainValues>();
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        this.getMarchingCubesInPlayer()._valuesGlobal[(int)playerPos.x % terrainValues.Size][(int)playerPos.y % terrainValues.Size][(int)playerPos.z % terrainValues.Size] = 1f;
    }

    MarchinCubes getMarchingCubesInPlayer()
    {
        GameObject terrain = GameObject.FindGameObjectWithTag("terrainValues");

        terrainValues terrainValues = terrain.GetComponent<terrainValues>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        return this.getMarchingCubes(
            (int)player.transform.position.x / terrainValues.Size,
            (int)player.transform.position.y / terrainValues.Size,
            (int)player.transform.position.z / terrainValues.Size,
            terrain);
    }

    MarchinCubes getMarchingCubes(int x, int y, int z, GameObject terrain)
    {
        try
        {
            return GameObject.Find($"Mesh {x} {y} {z}").GetComponent<MarchinCubes>();
        }
        catch (Exception)
        {
            CreateChunk chunk = terrain.GetComponent<CreateChunk>();
            chunk.GenrateChunk(x, y, z, terrain.GetComponent<terrainValues>().Size);
            return GameObject.Find($"Mesh {x} {y} {z}").GetComponent<MarchinCubes>();
        }
    }
}

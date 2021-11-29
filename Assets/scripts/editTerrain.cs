using System;
using System.Collections;
using System.Collections.Generic;

using Assets;

using UnityEngine;

public class editTerrain : MonoBehaviour
{
    private bool edit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            this.edit = this.toggleBool(this.edit);
        }

        if (this.edit)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            MarchinCubes marchinCubes = this.getMarchingCubesInPlayer();
            marchinCubes._valuesGlobal[(int)playerPos.x % terrainValues.Size][(int)playerPos.y % terrainValues.Size - 1][(int)playerPos.z % terrainValues.Size] = 1f;
            marchinCubes._valuesGlobal[(int)playerPos.x % terrainValues.Size][(int)playerPos.y % terrainValues.Size - 1][(int)playerPos.z % terrainValues.Size + 1] = 1f;
            marchinCubes._valuesGlobal[(int)playerPos.x % terrainValues.Size + 1][(int)playerPos.y % terrainValues.Size - 1][(int)playerPos.z % terrainValues.Size] = 1f;
            marchinCubes._valuesGlobal[(int)playerPos.x % terrainValues.Size + 1][(int)playerPos.y % terrainValues.Size - 1][(int)playerPos.z % terrainValues.Size + 1] = 1f;
        }
    }

    bool toggleBool(bool target)
    {
        if (target)
        {
            return false;
        }

        return true;
    }

    MarchinCubes getMarchingCubesInPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        return this.getMarchingCubes(
            (int)player.transform.position.x / terrainValues.Size,
            (int)player.transform.position.y / terrainValues.Size,
            (int)player.transform.position.z / terrainValues.Size);
    }

    MarchinCubes getMarchingCubes(int x, int y, int z)
    {
        return GameObject.Find($"Mesh {x} {y} {z}").GetComponent<MarchinCubes>();
    }
}

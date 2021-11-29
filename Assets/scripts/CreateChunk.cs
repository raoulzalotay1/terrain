using System.Collections.Generic;
using Assets;
using UnityEngine;

public class CreateChunk : MonoBehaviour
{
    public GameObject chunk;
    public int maxThreads = 8;
    public Transform parent;

    [HideInInspector]
    public static List<Vector3> chunkQueue = new List<Vector3>();

    [HideInInspector]
    public static int threadCount = 0;

    void Start()
    {
        parent = transform;
    }

    void LateUpdate()
    {
        LoadChunksFromQueue();
    }

    public void GenerateChunk(Vector3 pos)
    {

        GameObject prefab = Instantiate(chunk, new Vector3(pos.x * terrainValues.Size, pos.y * terrainValues.Size, pos.z * terrainValues.Size), new Quaternion());
        prefab.transform.parent = parent;
        prefab.name = $"Mesh {pos.x} {pos.y} {pos.z}";

        prefab.AddComponent<MarchinCubes>();
    }

    private void LoadChunksFromQueue()
    {
        while (threadCount <= maxThreads)
        {
            // Get the closest chunk location from the queue if one exists
            if (!Dequeue(out var chunkPos))
                break;

            threadCount++;
            GenerateChunk(chunkPos);
        }
    }

    private bool Dequeue(out Vector3 outVector3)
    {
        if (chunkQueue.Count == 0)
        {
            outVector3 = new Vector3();
            return false;
        }

        outVector3 = chunkQueue[0];
        chunkQueue.RemoveAt(0);
        return true;
    }
}

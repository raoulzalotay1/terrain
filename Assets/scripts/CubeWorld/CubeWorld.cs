using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelEngine;
using ThreadPriority = System.Threading.ThreadPriority;

public class CubeWorld : MonoBehaviour
{
    public static Dictionary<Vector3, CubeWorld> Chunks = new Dictionary<Vector3, CubeWorld>();

    public int Size;

    public float SurfaceValue;

    public FastNoiseSIMDUnity FastNoise;
    public FastNoiseSIMDUnity CaveNoise;

    public Vector3 ChunkPos;

    public Cube[][][] blocks;

    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    public static void createChunk(Vector3 chunkPos, FastNoiseSIMDUnity fastNoise, FastNoiseSIMDUnity caveNoise, int surfaceValue, Material mat, Transform Parent, int size = 32)
    {
        GameObject gameObject = new GameObject($"Chunk {chunkPos.x} {chunkPos.y} {chunkPos.z}");
        gameObject.transform.position = new Vector3(chunkPos.x * size, chunkPos.y * size, chunkPos.z * size);
        gameObject.transform.parent = Parent;
        gameObject.layer = 8;

        CubeWorld cubeWorld = gameObject.AddComponent<CubeWorld>();
        gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshCollider>();
        cubeWorld.startChunk(meshRenderer, mat, chunkPos, fastNoise, caveNoise, surfaceValue, size);
    }

    private void startChunk(MeshRenderer meshRenderer, Material mat, Vector3 chunkPos, FastNoiseSIMDUnity fastNoise, FastNoiseSIMDUnity caveNoise, float surfaceValue, int size)
    {
        meshRenderer.material = mat;

        ChunkPos = chunkPos;
        FastNoise = fastNoise;
        CaveNoise = caveNoise;
        SurfaceValue = surfaceValue;
        Size = size;

        StartCoroutine(threadStart());
    }

    private IEnumerator threadStart()
    {
        bool done = false;

        Thread thread = new Thread(() =>
        {
            GenerateNoise();
            updateSelfAndBelow();
            Chunks.Add(ChunkPos, this);
            updateSuroundings();
            done = true;
        })
        {
            Priority = ThreadPriority.BelowNormal
        };

        thread.Start();

        // Corountine waits for the thread to finish before continuing on the main thread
        while (!done)
            yield return null;

        
    }

    public void GenerateNoise()
    {
       blocks = VoxelGenerator.noiseBools(Size, FastNoise, SurfaceValue, ChunkPos, CaveNoise);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            updateMesh();
        }

        if (colors != null)
        {
            Mesh mesh = new Mesh();

            mesh.indexFormat = IndexFormat.UInt32;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.colors = colors;

            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }

    public void updateMesh()
    {
        VoxelGenerator.SetColors(this);
        VoxelGenerator.generateMesh(Size, ChunkPos, blocks, out vertices, out triangles, out colors);
    }

    private void updateSelfAndBelow()
    {
        updateMesh();
        if (Chunks.TryGetValue(new Vector3(ChunkPos.x, ChunkPos.y - 1, ChunkPos.z), out CubeWorld chunkBelow))
        {
            chunkBelow.updateSelfAndBelow();
        }
    }

    private void updateSuroundings()
    {
        if (Chunks.TryGetValue(new Vector3(ChunkPos.x, ChunkPos.y + 1, ChunkPos.z), out CubeWorld chunk))
        {
            chunk.updateMesh();
        }

        if (Chunks.TryGetValue(new Vector3(ChunkPos.x - 1, ChunkPos.y, ChunkPos.z), out chunk))
        {
            chunk.updateMesh();
        }

        if (Chunks.TryGetValue(new Vector3(ChunkPos.x + 1, ChunkPos.y, ChunkPos.z), out chunk))
        {
            chunk.updateMesh();
        }

        if (Chunks.TryGetValue(new Vector3(ChunkPos.x, ChunkPos.y, ChunkPos.z - 1), out chunk))
        {
            chunk.updateMesh();
        }

        if (Chunks.TryGetValue(new Vector3(ChunkPos.x, ChunkPos.y, ChunkPos.z + 1), out chunk))
        {
            chunk.updateMesh();
        }
    }
}

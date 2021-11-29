using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleTerrain : MonoBehaviour
{
    public int size = 20;
    public float heightMultiplier = 1;
    public int mapDetailLevel = 5;
    public bool update = false;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    float[][] points;
    int pointSize;
    void Start()
    {
        this.initialization();
        this.createMesh();
    }

    void Update()
    {
        if (this.update == true)
        {
            this.initialization();
            this.createMesh();
        }
    }

    void initialization()
    {
        this.mesh = new Mesh();

        this.triangles = new int[this.size * this.size * 6];

        this.pointSize = this.size + 1;
        this.vertices = new Vector3[this.pointSize * this.pointSize];
        this.points = new float[this.pointSize][];
        for (int i = 0; i < this.pointSize; i++)
        {
            this.points[i] = new float[this.pointSize];
        }
    }

    void createMesh()
    {
        this.setVertecies();

        this.setTriangles();

        this.setMesh();
    }

    void setVertecies()
    {
        int count = 0;
        for (int i = 0; i < this.pointSize; i++)
        {
            for (int j = 0; j < this.pointSize; j++)
            {
                this.vertices[count] = new Vector3(i, this.getPerlin2D(i, j, this.mapDetailLevel) * this.heightMultiplier, j);
                count++;
            }
        }
    }

    void setTriangles()
    {
        int count = 0;
        int countSecret = 0;
        for (int x = 0; x < this.size * this.size * 6; x += 6)
        {
            if ((countSecret + 1) % (this.pointSize + 1) == 0)
            {
                x -= 6;
                countSecret++;
            }

            this.triangles[x] = count;
            this.triangles[x + 1] = count + 1;
            this.triangles[x + 2] = count + this.pointSize;

            this.triangles[x + 3] = count + 1;
            this.triangles[x + 4] = count + 1 + this.pointSize;
            this.triangles[x + 5] = count + this.pointSize;

            count++;
            countSecret++;
        }
    }

    void setMesh()
    {
        this.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        this.mesh.vertices = this.vertices;
        this.mesh.triangles = this.triangles;

        this.mesh.RecalculateNormals();
        this.GetComponent<MeshFilter>().mesh = this.mesh;
        this.GetComponent<MeshCollider>().sharedMesh = this.mesh;
    }

    float getPerlin2D(int x, int z, int divider)
    {
        return Mathf.PerlinNoise((float)x / divider + this.transform.position.x / this.mapDetailLevel, (float)z / divider + this.transform.position.z / this.mapDetailLevel);
    }
}

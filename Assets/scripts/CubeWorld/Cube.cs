using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public int X;
    public int Y;
    public int Z;

    public Color Color;

    public Cube(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;

        Color = Color.green;
    }
}

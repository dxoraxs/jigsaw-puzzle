using System;
using UnityEngine;

[Serializable]
public class ShardPuzzleData
{
    public Mesh Mesh;
    public Vector3 Position;
    public int HorizontalIndex;
    public int VerticalIndex;

    public ShardPuzzleData(Mesh mesh, Vector3 position, int horizontalIndex, int verticalIndex)
    {
        Mesh = mesh;
        Position = position;
        HorizontalIndex = horizontalIndex;
        VerticalIndex = verticalIndex;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenerateMeshPuzzles
{
    public static void StartGenerateMesh(PuzzlesGridBarriersData puzzlesGridBarriersData,
        SettingsBuilderDTO settingsBuilder)
    {
        var puzzles = puzzlesGridBarriersData.GetArray;
        var meshes = new Mesh[settingsBuilder.CountHorizontal][];
        var shardPuzzle = new ShardPuzzleData[settingsBuilder.CountHorizontal * settingsBuilder.CountVertical];

        var multiplyHeightProportionally = (float)settingsBuilder.Texture.height / settingsBuilder.Texture.width;

        var horizontalOffsetFormCenter = (1f / puzzles.Length) * (-puzzles.Length / 2f);
        var verticalOffsetFormCenter = (1f / puzzles[0].Length) * (-puzzles[0].Length / 2f);
        var offsetFormCenter = new Vector3(horizontalOffsetFormCenter, 0, verticalOffsetFormCenter);

        for (int horizontal = 0; horizontal < puzzles.Length; horizontal++)
        {
            meshes[horizontal] = new Mesh[settingsBuilder.CountVertical];
            for (int vertical = 0; vertical < puzzles[horizontal].Length; vertical++)
            {
                meshes[horizontal][vertical] = CreateMeshObject(puzzles[horizontal][vertical],
                    $"{horizontal}x{vertical}", multiplyHeightProportionally);

                var centerPosition = puzzles[horizontal][vertical].GetCenterPosition();
                shardPuzzle[horizontal * settingsBuilder.CountVertical + vertical] = new ShardPuzzleData(
                    meshes[horizontal][vertical],
                    offsetFormCenter +
                    new Vector3(centerPosition.x, 0, centerPosition.y * multiplyHeightProportionally),
                    horizontal, vertical);
            }
        }

        PuzzleAssetsController.SavePuzzleToFiles(meshes, settingsBuilder);
        PuzzleAssetsController.SaveScriptableObject(shardPuzzle, settingsBuilder);
    }

    private static Mesh CreateMeshObject(PuzzleBarriersData puzzlesData, string name, float multiplyHeight)
    {
        var mesh = CreateMesh(puzzlesData, name, multiplyHeight);
        return mesh;
    }

    private static Mesh CreateMesh(PuzzleBarriersData puzzlesData, string name, float multiplyHeight)
    {
        var points = new List<Vector2>();

        points.AddRange(puzzlesData.GetLeftBarrier);
        points.AddRange(puzzlesData.GetUpBarrier);
        points.AddRange(puzzlesData.GetRightBarrier);
        points.AddRange(puzzlesData.GetBottomBarrier);
        RecalculatePoint(points);

        var centerPosition = puzzlesData.GetCenterPosition();
        points.Insert(0, centerPosition);

        var pointsCount = points.Count;

        var triangles = Triangulator.Generate(points);

        var triangleIndexes = new List<int>();
        foreach (var triangle in triangles)
        {
            triangleIndexes.Add(points.IndexOf(triangle.PointA));
            triangleIndexes.Add(points.IndexOf(triangle.PointMiddle));
            triangleIndexes.Add(points.IndexOf(triangle.PointB));
        }

        var offsetFromCenter = new Vector3(centerPosition.x, 0, centerPosition.y * multiplyHeight);
        var uvs = new Vector2[pointsCount];
        var newNormals = new Vector3[pointsCount];
        var vertices = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            uvs[i] = new Vector2(points[i].x, points[i].y);
            newNormals[i] = Vector3.up;
            vertices[i] = new Vector3(points[i].x, 0, points[i].y * multiplyHeight) - offsetFromCenter;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();
        mesh.normals = newNormals;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.uv = uvs;
        mesh.name = name;

        return mesh;
    }

    private static void RecalculatePoint(List<Vector2> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] == points[i + 1])
            {
                points.Remove(points[i]);
                i--;
            }
        }
    }
}
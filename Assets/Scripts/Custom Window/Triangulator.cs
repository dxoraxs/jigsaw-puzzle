using System.Collections.Generic;
using UnityEngine;

public static class Triangulator
{
    public static List<Triangle> Generate(IReadOnlyList<Vector2> points)
    {
        if (points.Count < 3) return null;

        var path = new List<Vector2>();
        foreach (var vector3 in points)
            path.Add(vector3);

        if (points.Count == 3)
        {
            var triangles = new List<Triangle>()
                { new(path[0], path[1], path[2]) };
            return triangles;
        }

        var middleIndex = 1;
        var leftIndex = middleIndex - 1 < 0 ? path.Count - 1 : middleIndex - 1;
        var rightIndex = middleIndex + 1 >= path.Count ? 0 : middleIndex + 1;

        var middlePoint = path[middleIndex];

        var triangle = new Triangle(path[leftIndex], middlePoint, path[rightIndex]);
        
        path.RemoveAt(middleIndex);

        var recursionResult = Generate(path);
        recursionResult.Add(triangle);
        return recursionResult;
    }
}
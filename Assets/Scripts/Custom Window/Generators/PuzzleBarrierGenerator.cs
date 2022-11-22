using System.Collections.Generic;
using UnityEngine;

public static class PuzzleBarrierGenerator
{
    public static PuzzlesGridBarriersData Generate(SettingsBuilderDTO dto)
    {
        if (dto.CurvesData == null)
        {
            Debug.LogError("Curves are empty!");
            return null;
        }

        Random.InitState(dto.Seed);
        var result = new PuzzlesGridBarriersData(dto.CountHorizontal, dto.CountVertical);

        var horizontalOffset = 1f / dto.CountHorizontal;
        var verticalOffset = 1f / dto.CountVertical;
        for (var horizontal = 0; horizontal < dto.CountHorizontal; horizontal++)
        {
            var startHorizontal = horizontal * horizontalOffset;
            var endHorizontal = (horizontal + 1) * horizontalOffset;
            for (var vertical = 0; vertical < dto.CountVertical; vertical++)
            {
                var startVertical = vertical * verticalOffset;
                var endVertical = (vertical + 1) * verticalOffset;

                var startTop = new Vector2(startHorizontal, endVertical);
                var endPuzzle = new Vector2(endHorizontal, endVertical);
                var endRight = new Vector2(endHorizontal, startVertical);

                var topBarrier = vertical == dto.CountVertical - 1
                    ? GenerateLinearBarrier(startTop, endPuzzle)
                    : GenerateBarrier(startTop, endPuzzle, dto);

                var rightBarrier = horizontal == dto.CountHorizontal - 1
                    ? GenerateLinearBarrier(endPuzzle, endRight)
                    : GenerateBarrier(endPuzzle, endRight, dto);

                result.AddBarrierToPuzzle(horizontal, vertical, topBarrier, rightBarrier);
            }
        }

        return result;
    }
    
    public static Vector2[] GenerateLinearBarrier(Vector2 start, Vector2 end, float quality)
    {
        var border = new List<Vector2>();
        var direction = end - start;
        var distance = direction.magnitude;

        var countPoint = (int)(distance / quality);
        var distanceBetweenPoint = distance / countPoint;

        for (var counter = 0; counter < countPoint; counter++)
        {
            border.Add(start + direction.normalized * (counter * distanceBetweenPoint));
        }

        border.Add(end);
        return border.ToArray();
    }

    private static Vector2[] GenerateBarrier(Vector2 start, Vector2 end, SettingsBuilderDTO settings)
    {
        var border = new List<Vector2>();
        var randomCurve = settings.CurvesData.GetRandomCurve();

        var direction = end - start;
        var directionWeave = (Vector2)(Quaternion.Euler(Vector3.forward * 90) * direction.normalized);
        var distancePoint = direction.magnitude;
        var countPoint = (int)(distancePoint / settings.Quality);
        var distanceBetweenPoint = distancePoint / countPoint;

        for (var counter = 0; counter < countPoint; counter++)
        {
            border.Add(start + direction.normalized * (counter * distanceBetweenPoint) +
                       directionWeave * (randomCurve.Evaluate((float)counter / countPoint) * settings.Radius));
        }
        border.Add(end);
        return border.ToArray();
    }

    private static Vector2[] GenerateLinearBarrier(Vector2 start, Vector2 end)
    {
        return new[] { start, end };
    }
}
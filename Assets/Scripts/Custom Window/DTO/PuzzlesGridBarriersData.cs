using UnityEngine;

public class PuzzlesGridBarriersData
{
    private readonly PuzzleBarriersData[][] puzzles;

    public PuzzleBarriersData[][] GetArray => puzzles;
    
    public PuzzlesGridBarriersData(int countHorizontal, int countVertical)
    {
        puzzles = new PuzzleBarriersData[countHorizontal][];
        for (var horizontal = 0; horizontal < countHorizontal; horizontal++)
        {
            puzzles[horizontal] = new PuzzleBarriersData[countVertical];
        }
    }

    public void AddBarrierToPuzzle(int x, int y, Vector2[] topBarrier, Vector2[] rightBarrier)
    {
        var horizontalPosition = (1f / puzzles.Length) * (x + .5f);
        var verticalPosition = (1f / puzzles[0].Length) * (y + .5f);
        
        var left = x-1 >= 0 ? puzzles[x-1][y] : null;
        var bottom = y-1 >= 0 ? puzzles[x][y-1] : null;
        puzzles[x][y] = new PuzzleBarriersData(left, bottom, topBarrier, rightBarrier, new Vector2(horizontalPosition, verticalPosition));
    }
}
using DG.Tweening;
using UnityEngine;

public static class PuzzleAnimation
{
    private const float TIME_TO_CHANGE_SIZE = .25f;
    private const float SCALE_ON_TABLE = 1f;
    private const float MULTIPLY_DURATION_MOVE = 0.5F;
    
    public static void OnPuzzleStartDrag(Transform puzzle)
    {
        puzzle.DOScale(SCALE_ON_TABLE, TIME_TO_CHANGE_SIZE);
    }
    
    public static void PuzzleLocalMove(Transform puzzle, Vector3 localPosition)
    {
        var duration = (puzzle.localPosition - localPosition).magnitude * MULTIPLY_DURATION_MOVE;
        puzzle.DOLocalMove(localPosition, duration);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPuzzleManager : PuzzleStorage
{
    [SerializeField] protected Transform parentPuzzles;
    [SerializeField] private PuzzlePoolPositionCalculator positionCalculator;
    [SerializeField] private PoolDragController dragController;

    protected override void OnRemovePuzzle()
    {
        RefreshPositionPuzzle();
    }

    protected override void OnAddPuzzle()
    {
        RefreshPositionPuzzle();
    }

    private void RefreshPositionPuzzle()
    {
        if (puzzles.Count <= 0) return;
            
        var position = positionCalculator.GetLocalPositions(puzzles.Count);
        dragController.SetBarrierMove(position[0].z);
        for (var index = 0; index < puzzles.Count; index++)
        {
            puzzles[index].transform.SetParent(parentPuzzles);
            puzzles[index].transform.localPosition = position[index];
        }
    }
}
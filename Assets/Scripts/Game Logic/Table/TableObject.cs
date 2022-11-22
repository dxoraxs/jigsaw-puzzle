using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TableObject
{
    private const string PUZZLE_PART_NAME = "puzzlePart";
    private readonly Dictionary<Collider, ShardPuzzleData> puzzles;
    private readonly Transform parent;

    public TableObject(Transform table)
    {
        puzzles = new();
        parent = new GameObject().transform;
        parent.name = PUZZLE_PART_NAME;
        parent.SetParent(table);
    }

    public int Count => puzzles.Count;
    
    public Vector3 Position => parent.position;
    
    public bool IsContains(Collider collider)
    {
        return puzzles.ContainsKey(collider);
    }

    public bool TryToMergeObjects(TableObject anotherObject, float maxDistance)
    {
        foreach (var anotherPuzzle in anotherObject.puzzles)
        {
            var thisPuzzlePosition = anotherPuzzle.Key.transform.position;
            var newPuzzlePosition = parent.position + anotherPuzzle.Value.Position;
            if ((thisPuzzlePosition - newPuzzlePosition).magnitude < maxDistance)
            {
                foreach (var mainPuzzle in puzzles)
                {
                    if (IsPuzzleNeighbor(mainPuzzle.Value, anotherPuzzle.Value))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void MergeTwoObjects(TableObject anotherObject)
    {
        foreach (var anotherPuzzle in anotherObject.puzzles)
        {
            AddPuzzle(anotherPuzzle.Key, anotherPuzzle.Value);
        }
        anotherObject.puzzles.Clear();
    }
    
    public void RecalculateParentPosition()
    {
        if (puzzles.Count != 1) return;

        var firstPuzzlePair = puzzles.ElementAt(0);
        var firstPuzzleTransform = firstPuzzlePair.Key.transform;
        var offsetLocalPosition = firstPuzzlePair.Value.Position;
        parent.position = firstPuzzleTransform.position - offsetLocalPosition;
        firstPuzzleTransform.localPosition = offsetLocalPosition;
    }

    public void OnDestroyObject()
    {
        Object.Destroy(parent.GameObject());
    }

    public void DragParent(Vector3 position)
    {
        parent.position = position;
    }

    public void AddPuzzle(Collider collider, ShardPuzzleData data)
    {
        puzzles.Add(collider, data);
        AddObjectToParent(collider, data);
    }

    private void AddObjectToParent(Collider collider, ShardPuzzleData data)
    {
        collider.transform.SetParent(parent);
        PuzzleAnimation.PuzzleLocalMove(collider.transform, data.Position);
    }
    
    private static bool IsPuzzleNeighbor(ShardPuzzleData main, ShardPuzzleData another)
    {
        var differenceHorizontalIndex = Mathf.Abs(main.HorizontalIndex - another.HorizontalIndex);
        var differenceVerticalIndex = Mathf.Abs(main.VerticalIndex - another.VerticalIndex);

        if (differenceHorizontalIndex != 1 && differenceVerticalIndex != 1) return false;
        if (differenceHorizontalIndex == 1 && differenceVerticalIndex == 1) return false;

        if (differenceHorizontalIndex == 1 && differenceVerticalIndex > 1) return false;
        if (differenceVerticalIndex == 1 && differenceHorizontalIndex > 1) return false;

        return true;
    }
}
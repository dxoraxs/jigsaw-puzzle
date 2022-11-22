using System.Collections.Generic;
using UnityEngine;

public class PuzzleStorage : MonoBehaviour
{
    protected readonly List<Collider> puzzles = new();

    public int Count => puzzles.Count;
    
    public bool IsContains(Collider puzzle)
    {
        return puzzles.Contains(puzzle);
    }

    public void AddRange(params Collider[] puzzles)
    {
        this.puzzles.AddRange(puzzles);
        OnAddPuzzle();
    }

    protected virtual void OnAddPuzzle() {}

    public void Add(Collider puzzle)
    {
        AddRange(puzzle);
    }

    public void Remove(Collider puzzle)
    {
        puzzles.Remove(puzzle);
        OnRemovePuzzle();
    }
    
    protected virtual void OnRemovePuzzle() {}
}
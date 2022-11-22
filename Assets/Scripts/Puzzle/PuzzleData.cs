using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "puzzle_", menuName = "Puzzle/Create puzzle", order = 0)]
public class PuzzleData : ScriptableObject
{
    [SerializeField] private Material material;
    [SerializeField] private ShardPuzzleData[] shards;

    public void Initialize(Material material, ShardPuzzleData[] shards)
    {
        this.shards = new ShardPuzzleData[shards.Length];
        for (int i = 0; i < shards.Length; i++)
            this.shards[i] = shards[i];
        
        this.material = material;
    }

    public IEnumerable<ShardPuzzleData> GetShards => shards;
    public Material Material => material;
    public int CountPuzzles => shards.Length;
}
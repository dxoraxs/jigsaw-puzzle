using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const string ERROR_NOT_FIND_DATA = "Not find puzzles data!";
    [SerializeField] private MeshObjectCreator meshObjectCreator;
    [SerializeField] private DragController dragController;
    [SerializeField] private PoolPuzzleManager poolManager;
    [SerializeField] private PuzzleStorage tableManager;
    [SerializeField] private DragTableObjectController dragTableController;
    private readonly Dictionary<Collider, ShardPuzzleData> allPuzzles = new();

    [Button]
    private void RandomDebug()
    {
        Debug.Log(Random.Range(0,2));
    }
    
    public ShardPuzzleData GetPuzzleData(Collider puzzle) => allPuzzles[puzzle];
    
    private void Start()
    {
        if (!PuzzleAssetsController.LoadRandomScriptableObject(out var puzzles))
        {
            Debug.LogError(ERROR_NOT_FIND_DATA);
            return;
        }
        
        var meshes = meshObjectCreator.CreateMesh(puzzles.Material, puzzles.CountPuzzles);

        var listeners = InitializePuzzle(puzzles, meshes);
        poolManager.AddRange(listeners);

        dragController.SubscribeStartDragPuzzle(OnStartDragPuzzles);
        dragController.SubscribeEndDrag(OnEndDragPuzzle);
    }

    private void OnEndDragPuzzle()
    {
        if (dragTableController.GetCountInFirstTablePart() == allPuzzles.Count && poolManager.Count == 0)
        {
            dragController.StopDragInvoke();
        }
    }

    private void OnStartDragPuzzles(Collider puzzle, Vector3 point)
    {
        if (poolManager.IsContains(puzzle))
        {
            poolManager.Remove(puzzle);
            tableManager.Add(puzzle);
            PuzzleAnimation.OnPuzzleStartDrag(puzzle.transform);
        }
    }

    private Collider[] InitializePuzzle(PuzzleData puzzleData, List<MeshFilter> meshes)
    {
        var colliders = new List<Collider>();
        var shards = puzzleData.GetShards;
        foreach (var shardPuzzle in shards)
        {
            var meshFilter = meshes[0];
            meshes.RemoveAt(0);
            meshFilter.mesh = shardPuzzle.Mesh;
            var meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            colliders.Add(meshCollider);
            allPuzzles.Add(meshCollider, shardPuzzle);
        }

        return colliders.ToArray();
    }
}
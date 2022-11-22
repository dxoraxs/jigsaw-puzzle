using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class DragTableObjectController : MonoBehaviour
{
    private const float DISTANCE_TO_MERGE_TWO_OBJECT = 0.15f;
    [SerializeField] private DragController dragController;
    [SerializeField] private Transform parentParts;
    [SerializeField] private GameController gameController;
    private TableObject currentTableObject;
    private Vector3 offsetFromMousePosition;
    private readonly List<TableObject> dragTableObjects = new();

    public int GetCountInFirstTablePart()
    {
        if (dragTableObjects.Count == 0) return -1;

        return dragTableObjects[0].Count;
    }
    
    private void Start()
    {
        dragController.SubscribeStartDragPuzzle(OnStartDrag);
        dragController.SubscribeDragPuzzle(OnDragPuzzle);
        dragController.SubscribeEndDrag(OnEndDrag);
    }

    private void OnStartDrag(Collider puzzle, Vector3 startPointTouch)
    {
        TableObject currentTableObj = dragTableObjects.FirstOrDefault(tableObject => tableObject.IsContains(puzzle));

        if (currentTableObj == null)
        {
            currentTableObj = new TableObject(parentParts);
            currentTableObj.AddPuzzle(puzzle, gameController.GetPuzzleData(puzzle));
            currentTableObj.RecalculateParentPosition();
            dragTableObjects.Add(currentTableObj);
        }

        currentTableObject = currentTableObj;
        offsetFromMousePosition = currentTableObject.Position - startPointTouch;
        offsetFromMousePosition -= Vector3.up * offsetFromMousePosition.y;
    }

    private void OnDragPuzzle(Vector3 position)
    {
        currentTableObject.DragParent(position + offsetFromMousePosition);
    }

    private void OnEndDrag()
    {
        if (dragTableObjects.Count <= 1 || currentTableObject == null)
        {
            currentTableObject = null;
            return;
        }
        var listObjectDestroy = new List<TableObject>();

        foreach (var tableObject in dragTableObjects)
        {
            if (currentTableObject != tableObject &&
                currentTableObject.TryToMergeObjects(tableObject, DISTANCE_TO_MERGE_TWO_OBJECT))
            {
                currentTableObject.MergeTwoObjects(tableObject);
                listObjectDestroy.Add(tableObject);
            }
        }

        if (listObjectDestroy.Count > 0)
            foreach (var tableObject in listObjectDestroy)
            {
                tableObject.OnDestroyObject();
                dragTableObjects.Remove(tableObject);
            }

        currentTableObject = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolDragController : MonoBehaviour
{
    [SerializeField] private DragController dragController;
    [SerializeField] private Transform moveAnchorPuzzle;
    private float startOffsetFromRay;
    private Vector3 startAnchorPosition;
    private float maxOffset;

    public void SetBarrierMove(float offset)
    {
        maxOffset = Mathf.Abs(offset);
    }
    
    private void Start()
    {
        startAnchorPosition = Vector3.right * moveAnchorPuzzle.position.x;
        dragController.SubscribeDragPool(OnDrag, OnStartDrag);
    }

    private void OnStartDrag(float startZPosition)
    {
        startOffsetFromRay = moveAnchorPuzzle.position.z - startZPosition;
    }

    private void OnDrag(float zPosition)
    {
        MoveAnchor(zPosition);
    }

    private void MoveAnchor(float zPosition)
    {
        var newZPosition = zPosition + startOffsetFromRay;
        newZPosition = Mathf.Clamp(newZPosition, -maxOffset, maxOffset);
        moveAnchorPuzzle.position = startAnchorPosition + Vector3.forward * newZPosition;
    }
}
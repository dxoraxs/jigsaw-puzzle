using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask planeMask;
    private RaycastType typeDrag;
    private Action<float> onStartDragPool;
    private Action<float> onDragPool;
    private Action<Collider, Vector3> onStartDragPuzzle;
    private Action<Vector3> onDragPuzzle;
    private Action onEndDragTable;
    private RaycastCollider raycaster;

    public void StopDragInvoke()
    {
        onStartDragPool = null;
        onDragPool = null;
        onStartDragPuzzle = null;
        onDragPuzzle = null;
        onEndDragTable = null;
    }

    public void SubscribeEndDrag(Action endDrag)
    {
        onEndDragTable += endDrag;
    }

    public void SubscribeDragPool(Action<float> dragPool, Action<float> startDragPool)
    {
        onDragPool += dragPool;
        onStartDragPool += startDragPool;
    }

    public void SubscribeStartDragPuzzle(Action<Collider, Vector3> startDragPuzzle)
    {
        onStartDragPuzzle += startDragPuzzle;
    }

    public void SubscribeDragPuzzle(Action<Vector3> dragPuzzle)
    {
        onDragPuzzle += dragPuzzle;
    }

    private void Start()
    {
        raycaster = new RaycastCollider(Camera.main, planeMask);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnStartDrag();

        if (Input.GetMouseButton(0))
            DragPuzzle();

        if (Input.GetMouseButtonUp(0))
            OnEndDrag();
    }

    private void OnEndDrag()
    {
        switch (typeDrag)
        {
            case RaycastType.Pool:
                break;
            case RaycastType.Puzzle:
                onEndDragTable?.Invoke();
                break;
        }
    }

    private void DragPuzzle()
    {
        var pointRaycast = raycaster.GetRaycastPoint();
        switch (typeDrag)
        {
            case RaycastType.Pool:
                onDragPool?.Invoke(pointRaycast.z);
                break;
            case RaycastType.Puzzle:
                onDragPuzzle?.Invoke(pointRaycast);
                break;
        }
    }

    private void OnStartDrag()
    {
        var value = raycaster.GetRaycast(out typeDrag, out var startPoint);

        switch (typeDrag)
        {
            case RaycastType.Pool:
                onStartDragPool?.Invoke(startPoint.z);
                break;
            case RaycastType.Puzzle:
                onStartDragPuzzle?.Invoke(value, startPoint);
                break;
        }
    }
}

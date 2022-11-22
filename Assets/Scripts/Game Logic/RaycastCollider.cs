using UnityEngine;

public class RaycastCollider
{
    private const string POOL_TAG = "Pool";
    private const string TABLE_TAG = "Table";
    private readonly Camera camera;
    private readonly LayerMask planeMask;

    public RaycastCollider(Camera camera, LayerMask plane)
    {
        this.camera = camera;
        planeMask = plane;
    }

    public Vector3 GetRaycastPoint()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camera.farClipPlane, planeMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    
    public Collider GetRaycast(out RaycastType raycastType, out Vector3 point)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camera.farClipPlane))
        {
            if (hit.collider.CompareTag(POOL_TAG))
            {
                raycastType = RaycastType.Pool;
            }
            else if (hit.collider.CompareTag(TABLE_TAG))
            {
                raycastType = RaycastType.Table;
            }
            else
            {
                raycastType = RaycastType.Puzzle;
            }

            point = hit.point;
            return hit.collider;
        }

        point = ray.GetPoint(hit.distance);
        raycastType = RaycastType.None;
        return null;
    }
}
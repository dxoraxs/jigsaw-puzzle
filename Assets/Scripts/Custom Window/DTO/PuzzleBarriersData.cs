using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBarriersData
{
    private readonly PuzzleBarriersData leftBarriers;
    private readonly PuzzleBarriersData bottomBarriers;

    private readonly Vector2[] upBarrier;
    private readonly Vector2[] rightBarrier;

    private readonly Vector2 centerPosition;

    public PuzzleBarriersData(PuzzleBarriersData leftBarriers, PuzzleBarriersData bottomBarriers, Vector2[] upBarrier,
        Vector2[] rightBarrier, Vector2 centerPosition)
    {
        this.leftBarriers = leftBarriers;
        this.bottomBarriers = bottomBarriers;
        this.upBarrier = upBarrier;
        this.rightBarrier = rightBarrier;
        this.centerPosition = centerPosition;
    }

    public Vector2[] GetUpBarrier => upBarrier;

    public Vector2[] GetRightBarrier => rightBarrier;

    public Vector2 GetCenterPosition() => centerPosition;

    public IEnumerable<Vector2> GetLeftBarrier
    {
        get
        {
            if (leftBarriers != null)
            {
                var point = new List<Vector2>(leftBarriers.GetRightBarrier);
                point.Reverse();
                return point.ToArray();
            }

            var startPoint = new Vector2(upBarrier[0].x, rightBarrier[^1].y);
            var endPoint = new Vector2(upBarrier[0].x, rightBarrier[0].y);

            return new[] { startPoint, endPoint };
        }
    }

    public IEnumerable<Vector2> GetBottomBarrier
    {
        get
        {
            if (bottomBarriers != null)
            {
                var point = new List<Vector2>(bottomBarriers.GetUpBarrier);
                point.Reverse();
                return point.ToArray();
            }

            var startPoint = new Vector2(upBarrier[^1].x, rightBarrier[^1].y);
            var endPoint = new Vector2(upBarrier[0].x, rightBarrier[^1].y);


            return new[] { startPoint, endPoint };
        }
    }
}
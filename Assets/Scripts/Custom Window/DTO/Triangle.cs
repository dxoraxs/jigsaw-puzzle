using UnityEngine;

public class Triangle
{
    private readonly Vector2 pointA;
    private readonly Vector2 pointMiddle;
    private readonly Vector2 pointB;

    public Triangle(Vector2 pointA, Vector2 pointMiddle, Vector2 pointB)
    {
        this.pointA = pointA;
        this.pointMiddle = pointMiddle;
        this.pointB = pointB;
    }

    public Vector2 PointA => pointA;
    public Vector2 PointMiddle => pointMiddle;
    public Vector2 PointB => pointB;
}
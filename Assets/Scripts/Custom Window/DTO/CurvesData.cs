using UnityEngine;

[CreateAssetMenu(fileName = "curves_", menuName = "Puzzle/Create curve data", order = 0)]
public class CurvesData : ScriptableObject
{
    [SerializeField] private AnimationCurve[] curve;

    public AnimationCurve GetRandomCurve()
    {
        var randomIndex = Random.Range(0, curve.Length);
        return curve[randomIndex];
    }
}
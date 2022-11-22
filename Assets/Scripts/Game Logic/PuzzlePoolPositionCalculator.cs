using UnityEngine;

public class PuzzlePoolPositionCalculator : MonoBehaviour
{
    [SerializeField] private float offsetPositionZ;
    
    public Vector3[] GetLocalPositions(int count)
    {
        var result = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            result[i] = new Vector3(0, 0, offsetPositionZ * (i - count / 2f + 0.5f));
        }
        
        return result;
    }
}
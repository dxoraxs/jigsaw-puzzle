using UnityEngine;

public static class ExtensionsMethods
{
    public static Vector2 RoundCoordsToInt(this Vector2 vec)
    {
        return new Vector2(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }
}
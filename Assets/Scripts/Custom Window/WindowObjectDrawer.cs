using UnityEditor;
using UnityEngine;

public static class WindowObjectDrawer
{
    private const float CUBE_POINT_SCALE = 5f;
    private static PuzzleBuilderWindow currentWindow;

    public static PuzzleBuilderWindow CurrentWindow
    {
        set => currentWindow = value;
    }
    
    public static void DrawLine(Vector2 point1, Vector2 point2)
    {
        Handles.DrawLine(currentWindow.Grid.GridToGUI(point1), currentWindow.Grid.GridToGUI(point2));
    }

    public static void DrawTexture(Rect dimensions, Texture texture)
    {
        var rect = new Rect(currentWindow.Grid.GridToGUI(dimensions.position), dimensions.size / currentWindow.Grid.Zoom);
        GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, true);
    }
    
    public static void DrawCube(Vector2 position)
    {
        Handles.DrawWireCube(currentWindow.Grid.GridToGUI(position), Vector3.one * CUBE_POINT_SCALE);
    }
}
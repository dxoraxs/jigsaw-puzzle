using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleBuilderGrid
{
    const float MIN_ZOOM_VALUE = 0.1f;
    const float MAX_ZOOM_VALUE = 10;
    const int CELLS_IN_LINE_COUNT = 60;
    const float DEFAULT_CELL_SIZE = 10;
    
    private bool isDrawCenter;
    private readonly EditorWindow parentWindow;
    private Vector2 offsetFromCenter;
    private float zoom;
    
    public float Zoom
    {
        get { return zoom; }
        set { zoom = value > MAX_ZOOM_VALUE ? MAX_ZOOM_VALUE : value < MIN_ZOOM_VALUE ? MIN_ZOOM_VALUE : value; }
    }

    public Vector2 GridToGUI(Vector3 vec)
    {
        return (new Vector2(vec.x - offsetFromCenter.x, -vec.y - offsetFromCenter.y)) / zoom
               + new Vector2(parentWindow.position.width / 2, parentWindow.position.height / 2);
    }

    public void Draw()
    {
        DrawLines();
        DrawCenter();
    }
    

    public PuzzleBuilderGrid(EditorWindow parentWindow, bool isDrawCenterMark = true)
    {
        zoom = 5f;
        this.parentWindow = parentWindow;
        isDrawCenter = isDrawCenterMark;
        Recenter();
    }

    public void Move(Vector3 dv)
    {
        var x = offsetFromCenter.x + dv.x * zoom;
        var y = offsetFromCenter.y + dv.y * zoom;
        offsetFromCenter.x = x;
        offsetFromCenter.y = y;
    }

    private void Recenter()
    {
        offsetFromCenter = Vector3.zero;
    }

    private void DrawLines()
    {
        var lodLevel = (int)(Mathf.Log(zoom) / 1.5f);
        DrawLODLines(lodLevel > 0 ? lodLevel : 0);
    }

    private void DrawLODLines(int level)
    {
        var gridColor = Color.gray;
        var step0 = (int)Mathf.Pow(10, level);
        var halfCount = step0 * CELLS_IN_LINE_COUNT / 2 * 10;
        var length = halfCount * DEFAULT_CELL_SIZE;
        var offsetX = ((int)(offsetFromCenter.x / DEFAULT_CELL_SIZE)) / (step0 * step0) * step0;
        var offsetY = ((int)(offsetFromCenter.y / DEFAULT_CELL_SIZE)) / (step0 * step0) * step0;
        for (var i = -halfCount; i <= halfCount; i += step0)
        {
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, 0.3f);

            Handles.DrawLine(
                GridToGUI(new Vector2(-length + offsetX * DEFAULT_CELL_SIZE, (i + offsetY) * DEFAULT_CELL_SIZE)),
                GridToGUI(new Vector2(length + offsetX * DEFAULT_CELL_SIZE, (i + offsetY) * DEFAULT_CELL_SIZE))
            );
            Handles.DrawLine(
                GridToGUI(new Vector2((i + offsetX) * DEFAULT_CELL_SIZE, -length + offsetY * DEFAULT_CELL_SIZE)),
                GridToGUI(new Vector2((i + offsetX) * DEFAULT_CELL_SIZE, length + offsetY * DEFAULT_CELL_SIZE))
            );
        }

        offsetX = (offsetX / (10 * step0)) * 10 * step0;
        offsetY = (offsetY / (10 * step0)) * 10 * step0;
        
        for (var i = -halfCount; i <= halfCount; i += step0 * 10)
        {
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, 1);
            Handles.DrawLine(
                GridToGUI(new Vector2(-length + offsetX * DEFAULT_CELL_SIZE, (i + offsetY) * DEFAULT_CELL_SIZE)),
                GridToGUI(new Vector2(length + offsetX * DEFAULT_CELL_SIZE, (i + offsetY) * DEFAULT_CELL_SIZE))
            );
            Handles.DrawLine(
                GridToGUI(new Vector2((i + offsetX) * DEFAULT_CELL_SIZE, -length + offsetY * DEFAULT_CELL_SIZE)),
                GridToGUI(new Vector2((i + offsetX) * DEFAULT_CELL_SIZE, length + offsetY * DEFAULT_CELL_SIZE))
            );
        }
    }

    private void DrawCenter()
    {
        if (!isDrawCenter)
            return;

        Handles.color = Color.cyan;
        Handles.DrawLine(GridToGUI(Vector3.left * DEFAULT_CELL_SIZE * zoom),
            GridToGUI(Vector3.right * DEFAULT_CELL_SIZE * zoom));
        Handles.DrawLine(GridToGUI(Vector3.down * DEFAULT_CELL_SIZE * zoom),
            GridToGUI(Vector3.up * DEFAULT_CELL_SIZE * zoom));
    }
}
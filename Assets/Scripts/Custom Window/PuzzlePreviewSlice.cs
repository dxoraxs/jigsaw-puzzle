using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzlePreviewSlice
{
    private const int IMAGE_SIZE_WIDTH = 600;
    private readonly PuzzleBuilderWindow parentWindow;
    private Vector2 textureSize;
    
    public PuzzlePreviewSlice(PuzzleBuilderWindow parentWindow)
    {
        this.parentWindow = parentWindow;
    }

    public void DrawCurveBarrier()
    {
        var puzzles = parentWindow.PuzzlesGridBarriersData.GetArray;

        foreach (var horizontalRow in puzzles)
        {
            foreach (var currentVerticalPuzzle in horizontalRow)
            {
                var upBarrier = currentVerticalPuzzle.GetUpBarrier;
                DrawCurveBarrier(upBarrier);
                
                var rightBarrier = currentVerticalPuzzle.GetRightBarrier;
                DrawCurveBarrier(rightBarrier);
            }
        }
    }

    public void Draw(SettingsBuilderDTO settings)
    {
        DrawTexture(settings.Texture);
        
        DrawBorderLine(textureSize);
    }

    public void DrawLineSplice(SettingsBuilderDTO settings)
    {
        var horizontalOffsetXPosition = textureSize.x / 2f;
        var verticalOffsetYPosition = textureSize.y / 2f;
        
        var downBarrier = Vector2.down * verticalOffsetYPosition;
        var leftBarrier = Vector2.left * horizontalOffsetXPosition;
        
        for (var horizontalIndex = 1; horizontalIndex < settings.CountHorizontal; horizontalIndex++)
        {
            var offsetFromLeftBarrier = Vector2.left * (horizontalOffsetXPosition - textureSize.x * ((float)horizontalIndex / settings.CountHorizontal));
            WindowObjectDrawer.DrawLine(offsetFromLeftBarrier + downBarrier, offsetFromLeftBarrier - downBarrier);
        }
        
        for (var verticalIndex = 1; verticalIndex < settings.CountVertical; verticalIndex++)
        {
            var offsetFromDownBarrier = Vector2.down * (verticalOffsetYPosition - textureSize.y * ((float)verticalIndex / settings.CountVertical));
            WindowObjectDrawer.DrawLine(offsetFromDownBarrier + leftBarrier, offsetFromDownBarrier - leftBarrier);
        }
    }

    private void DrawCurveBarrier(Vector2[] barrier)
    {
        for (var index = 0; index < barrier.Length - 1; index++)
        {
            var point = barrier[index];
            var nextPoint = barrier[index + 1];
            var pointPositionOnGrid = new Vector2(point.x * textureSize.x, point.y * textureSize.y) - textureSize / 2;
            Handles.color = Color.blue;
            WindowObjectDrawer.DrawCube(pointPositionOnGrid);
            Handles.color = Color.red;
            WindowObjectDrawer.DrawLine(pointPositionOnGrid,
                new Vector2(nextPoint.x * textureSize.x, nextPoint.y * textureSize.y) - textureSize / 2);
        }
    }

    private static void DrawBorderLine(Vector2 scale)
    {
        Handles.color = Color.red;
        WindowObjectDrawer.DrawLine(Vector2.left * (scale.x / 2f) + Vector2.down * (scale.y / 2),
            Vector2.left * (scale.x / 2f) + Vector2.up * (scale.y / 2));
        WindowObjectDrawer.DrawLine(Vector2.left * (scale.x / 2f) + Vector2.up * (scale.y / 2),
            Vector2.right * (scale.x / 2f) + Vector2.up * (scale.y / 2));
        WindowObjectDrawer.DrawLine(Vector2.right * (scale.x / 2f) + Vector2.up * (scale.y / 2),
            Vector2.right * (scale.x / 2f) + Vector2.down * (scale.y / 2));
        WindowObjectDrawer.DrawLine(Vector2.right * (scale.x / 2f) + Vector2.down * (scale.y / 2),
            Vector2.left * (scale.x / 2f) + Vector2.down * (scale.y / 2));
    }

    private void DrawTexture(Texture texture)
    {
        var rescaleTextureHeght = texture.height * (IMAGE_SIZE_WIDTH / (float)texture.width);
        var offsetTextureCenter = Vector3.left * (IMAGE_SIZE_WIDTH / 2f) + Vector3.up * (rescaleTextureHeght / 2f);
        var rect = new Rect(offsetTextureCenter.x, offsetTextureCenter.y, IMAGE_SIZE_WIDTH, rescaleTextureHeght);
        textureSize = new Vector2(IMAGE_SIZE_WIDTH, rescaleTextureHeght);
        WindowObjectDrawer.DrawTexture(rect, texture);
    }
}
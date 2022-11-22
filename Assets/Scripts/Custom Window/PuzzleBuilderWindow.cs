using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleBuilderWindow : EditorWindow
{
    public PuzzleBuilderGrid Grid;
    public PuzzlesGridBarriersData PuzzlesGridBarriersData;
    private Vector3? lastMousePosition;
    private BuilderSettingsPanel settingsPanel;
    private PuzzlePreviewSlice puzzlePreviewSlice;
    
    [MenuItem("Window/Puzzle Builder/Open Editor")]
    public static void Create()
    {
        var window = GetWindow<PuzzleBuilderWindow>("PuzzleBuilder");
        window.position = new Rect(100, 100, 400, 400);
        window.Show();
    }

    public void GenerateMeshPuzzle()
    {
        GenerateMeshPuzzles.StartGenerateMesh(PuzzlesGridBarriersData, settingsPanel.PuzzleSettings);
    }
    
    private void OnEnable()
    {
        Grid = new PuzzleBuilderGrid(this);
        settingsPanel = new BuilderSettingsPanel(this);
        puzzlePreviewSlice = new PuzzlePreviewSlice(this);
        WindowObjectDrawer.CurrentWindow = this;
        
        wantsMouseMove = true;
    }
    
    private void OnGUI()
    {
        Grid.Draw();
        if (settingsPanel.PuzzleSettings.IsTextureNotNull)
        {
            puzzlePreviewSlice.Draw(settingsPanel.PuzzleSettings);
            
            if (settingsPanel.PuzzleSettings.IsSettingsIsReady)
                PuzzlesGridBarriersData = PuzzleBarrierGenerator.Generate(settingsPanel.PuzzleSettings);
            
            if (PuzzlesGridBarriersData != null)
                puzzlePreviewSlice.DrawCurveBarrier();
            else
                puzzlePreviewSlice.DrawLineSplice(settingsPanel.PuzzleSettings);
        }
        
        settingsPanel.Draw();
        KeysEvents();
    }
    
    private void KeysEvents()
    {
        var curEvent = Event.current;
        switch (curEvent.type)
        {
            case EventType.MouseDrag:
                if (curEvent.button == 1)
                    DragGrid();
                break;
            case EventType.MouseDown:
                if (curEvent.button == 1)
                    lastMousePosition = null;
                break;
            case EventType.ScrollWheel:
                OnScroll(curEvent.delta.y);
                break;
        }
    }
    
    private void OnScroll(float speed)
    {
        Grid.Zoom += speed * Grid.Zoom * 0.1f;
        Repaint();
    }
    
    private void DragGrid()
    {
        var curMousePosition = Event.current.mousePosition;
        if (lastMousePosition.HasValue)
        {
            var dv = GUIUtility.GUIToScreenPoint((Vector2)lastMousePosition)
                     - GUIUtility.GUIToScreenPoint(curMousePosition);
            Grid.Move(dv);
            Repaint();
        }
        lastMousePosition = curMousePosition;
    }
}
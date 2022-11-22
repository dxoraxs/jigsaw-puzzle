using UnityEditor;
using UnityEngine;

public class BuilderSettingsPanel
{
    private const float WIDTH_PANEL = 200;
    private const int WIDTH_LABEL = 75;
    private const string LABEL_TEXTURE = "Texture";
    private const string LABEL_SIZE = "Size: ";
    private const string LABEL_COUNT = "Count:";
    private const string LABEL_HORIZONTAL = "Horizontal";
    private const string LABEL_VERTICAL = "Vertical";
    private const string LABEL_QUALITY = "Quality";
    private const string LABEL_RADIUS = "Radius";
    private const string LABEL_SEED = "Seed";
    private const string LABEL_GENERATE = "Generate";
    
    private readonly PuzzleBuilderWindow parentWindow;
    private static SettingsBuilderDTO settingsBuilderDto;

    public BuilderSettingsPanel(PuzzleBuilderWindow window)
    {
        parentWindow = window;
    }

    public SettingsBuilderDTO PuzzleSettings => settingsBuilderDto;

    public void Draw()
    {
        var rect = EditorGUILayout.BeginVertical();
        
        rect.width = WIDTH_PANEL + 7; 
        EditorGUI.DrawRect(rect, Color.black);
        
        EditorGUIUtility.labelWidth = WIDTH_LABEL;
        CreateTextureField();
        
        if (settingsBuilderDto.IsTextureNotNull)
        {
            CreateCountInHorizontal();
            CreateSeedField();
            CreateBarriersSettings();
            CreateCurvesDataField();
            CreateButtonGenerateBarrierPoint();
        }
        
        rect.height = rect.height + 7;
        EditorGUILayout.EndVertical();
    }

    private void CreateTextureField()
    {
        settingsBuilderDto.Texture = (Texture)EditorGUILayout.ObjectField(LABEL_TEXTURE, settingsBuilderDto.Texture,
            typeof(Texture), false,
            GUILayout.MaxWidth(WIDTH_PANEL));
    }

    private void CreateCountInHorizontal()
    {
        EditorGUILayout.LabelField(LABEL_SIZE + $"{settingsBuilderDto.Texture.width}x{settingsBuilderDto.Texture.height}");
        EditorGUILayout.LabelField(LABEL_COUNT);
        WriteValueToField(ref settingsBuilderDto.CountHorizontal, LABEL_HORIZONTAL);
        WriteValueToField(ref settingsBuilderDto.CountVertical, LABEL_VERTICAL);
    }

    private void CreateBarriersSettings()
    {
        EditorGUILayout.Space(10);
        WriteValueToField(ref settingsBuilderDto.Quality, LABEL_QUALITY);
        WriteValueToField(ref settingsBuilderDto.Radius, LABEL_RADIUS);
    }

    private void CreateSeedField()
    {
        EditorGUILayout.Space(10);
        WriteValueToField(ref settingsBuilderDto.Seed, LABEL_SEED);
    }
    
    private void CreateCurvesDataField()
    {
        settingsBuilderDto.CurvesData = (CurvesData)EditorGUILayout.ObjectField("CurvesData", settingsBuilderDto.CurvesData,
            typeof(CurvesData), false, GUILayout.MaxWidth(WIDTH_PANEL));
    }

    private void CreateButtonGenerateBarrierPoint()
    {
        if (GUILayout.Button(LABEL_GENERATE,  GUILayout.Width(WIDTH_PANEL)))
        {
            parentWindow.GenerateMeshPuzzle();
        }
    }
    
    private void WriteValueToField(ref float value, string nameValue)
    {
        value = EditorGUILayout.FloatField(nameValue, value,
            EditorStyles.numberField,
            GUILayout.MaxWidth(WIDTH_PANEL));
        value = Mathf.Clamp(value, 0, float.MaxValue);
    }

    private void WriteValueToField(ref int value, string nameValue)
    {
        value = EditorGUILayout.IntField(nameValue, value,
            EditorStyles.numberField,
            GUILayout.MaxWidth(WIDTH_PANEL));
        value = Mathf.Clamp(value, 0, int.MaxValue);
    }
}
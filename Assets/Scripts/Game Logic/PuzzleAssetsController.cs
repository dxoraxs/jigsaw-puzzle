using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class PuzzleAssetsController
{
    private const string PATH_ASSETS = "Assets";
    private const string PATH_MODELS = "Models";
    private const string PATH_PUZZLE = "Puzzles";
    private const string PATH_MATERIALS = "Materials";
    private const string PATH_DATA = "Data";
    private const string PATH_SAVE_MESH = PATH_ASSETS + "/" + PATH_MODELS + "/" + PATH_PUZZLE + "/";
    private const string PATH_SAVE_MATERIAL = PATH_ASSETS + "/" + PATH_MATERIALS + "/" + PATH_PUZZLE + "/";
    private const string PATH_SAVE_SCRIPTABLE_OBJECT = PATH_ASSETS + "/" + PATH_DATA + "/" + PATH_PUZZLE + "/";
    private const string SHADER_NAME = "Standard";
    private const string TAG_SHADER_SMOOTHNESS = "_Glossiness";
    private const string FILTER_PUZZLE_FIND = "t:PuzzleData";

    public static bool LoadRandomScriptableObject(out PuzzleData puzzle)
    {
        puzzle = null;
        var puzzles = AssetDatabase.FindAssets(FILTER_PUZZLE_FIND, new[] { PATH_SAVE_SCRIPTABLE_OBJECT });

        if (puzzles.Length == 0) return false;
        
        var randomGuid = puzzles[Random.Range(0, puzzles.Length)];
        puzzle = (PuzzleData)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(randomGuid), typeof(PuzzleData));
        return true;
    }

    public static void SaveScriptableObject(ShardPuzzleData[] mesh, SettingsBuilderDTO settingsBuilder)
    {
        CheckPathToData();

        var objectName =
            $"{settingsBuilder.Texture.name}_{settingsBuilder.CountHorizontal}x{settingsBuilder.CountVertical}_data";

        var puzzleObject = (PuzzleData)ScriptableObject.CreateInstance(typeof(PuzzleData));
        puzzleObject.name = objectName;
        puzzleObject.Initialize(CreateAndSaveMaterial(settingsBuilder.Texture, objectName), mesh);

        AssetDatabase.CreateAsset(puzzleObject, PATH_SAVE_SCRIPTABLE_OBJECT + $"/{objectName}.asset");
        AssetDatabase.SaveAssets();
    }

    public static void SavePuzzleToFiles(Mesh[][] meshes, SettingsBuilderDTO settingsBuilder)
    {
        var folderName = $"{settingsBuilder.Texture.name}_{settingsBuilder.CountHorizontal}x{settingsBuilder.CountVertical}";

        CheckPathToModel(folderName);

        foreach (var horizontalRow in meshes)
        {
            foreach (var mesh in horizontalRow)
            {
                AssetDatabase.CreateAsset(mesh, PATH_SAVE_MESH + folderName + $"/{mesh.name}.asset");
            }
        }

        AssetDatabase.SaveAssets();
    }

    private static Material CreateAndSaveMaterial(Texture texture, string name)
    {
        CheckPathToMaterial();
        return CreateMaterial(texture, name);
    }

    private static Material CreateMaterial(Texture texture, string name)
    {
        var material = new Material(Shader.Find(SHADER_NAME));
        material.mainTexture = texture;
        material.SetFloat(TAG_SHADER_SMOOTHNESS, 0);
        material.color = Color.white;
        material.name = $"{name}_material";

        AssetDatabase.CreateAsset(material, PATH_SAVE_MATERIAL + $"/{name}_material.mat");
        return material;
    }

    private static void CheckPathToMaterial()
    {
        CheckFolder($"{PATH_ASSETS}", PATH_MATERIALS);
        CheckFolder($"{PATH_ASSETS}/{PATH_MATERIALS}", PATH_PUZZLE);
    }

    private static void CheckPathToData()
    {
        CheckFolder($"{PATH_ASSETS}", PATH_DATA);
        CheckFolder($"{PATH_ASSETS}/{PATH_DATA}", PATH_PUZZLE);
    }

    private static void CheckPathToModel(string folderName)
    {
        CheckFolder($"{PATH_ASSETS}", PATH_MODELS);
        CheckFolder($"{PATH_ASSETS}/{PATH_MODELS}", PATH_PUZZLE);
        CheckFolder($"{PATH_ASSETS}/{PATH_MODELS}/{PATH_PUZZLE}", folderName);
    }

    private static void CheckFolder(string path, string folderName)
    {
        if (!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        {
            AssetDatabase.CreateFolder(path, folderName);
        }
    }
}
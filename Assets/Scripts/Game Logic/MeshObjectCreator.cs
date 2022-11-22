using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshObjectCreator : MonoBehaviour
{
    [SerializeField] private float startScaleMesh;
    public List<MeshFilter> CreateMesh(Material material, int count)
    {
        var instancedPuzzles = new List<MeshFilter>();
        for (var i = 0; i < count; i++) 
            instancedPuzzles.Add(CreateMeshFilter(material));

        return instancedPuzzles;
    }
    
    private MeshFilter CreateMeshFilter(Material material)
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * startScaleMesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        
        return gameObject.AddComponent<MeshFilter>();
    }
}

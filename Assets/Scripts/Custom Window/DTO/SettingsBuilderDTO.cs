using UnityEngine;

public struct SettingsBuilderDTO
{
    public Texture Texture;
    public int CountHorizontal;
    public int CountVertical;
    public int Seed;
    public float Quality;
    public float Radius;
    public CurvesData CurvesData;

    public bool IsTextureNotNull => Texture != null;

    public bool IsSettingsIsReady => IsTextureNotNull && CurvesData != null && CountHorizontal > 0 &&
                                     CountVertical > 0 && Quality > 0;
}
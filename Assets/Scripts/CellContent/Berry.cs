using UnityEngine;

public class Berry : CellContent
{
    private int _colorValue;
    private Texture2D _texture;

    public int ColorValue => _colorValue;

    public void Initialize(int colorValue, Texture2D texture = null)
    {
        _colorValue = colorValue;
        _texture = texture;
        ApplyTexture(texture);
    }

    private void ApplyTexture(Texture2D texture)
    {
        var renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
    }
}

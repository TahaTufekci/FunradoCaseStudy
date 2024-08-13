using UnityEngine;

public class Frog : CellContent
{
    private int _colorValue;
    private Direction _direction;
    private Texture2D _texture;

    public int ColorValue => _colorValue;
    public Direction Direction => _direction;

    public void Initialize(int colorValue, Direction direction, Texture2D texture = null)
    {
        _colorValue = colorValue;
        _texture = texture;
        _direction = direction;
        ApplyTexture(texture);
    }

    private void ApplyTexture(Texture2D texture)
    {
        var renderer = GetComponentInChildren<Renderer>();
        renderer.material.mainTexture = texture;
    }
}

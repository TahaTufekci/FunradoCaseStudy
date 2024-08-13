using DG.Tweening;
using UnityEngine;

public class Arrow : CellContent
{
    private int _colorValue;
    private Material _material;
    private Direction _direction;

    public int ColorValue => _colorValue;
    public Direction Direction => _direction;

    public void Initialize(int colorValue, Material material, Direction direction)
    {
        _colorValue = colorValue;
        _direction = direction;
        _material = material;
        ApplyMaterial(material);
    }

    private void ApplyMaterial(Material material)
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = material;
    }
    public override void Interact()
    {
        var scale = transform.localScale;
        transform.DOScale(scale * 1.25f, 0.15f).OnComplete(() =>
        {
            transform.DOScale(scale, 0.15f);
        });
    }
    public override void Activate()
    {
        var scale = transform.localScale;
        // Reset scale to zero to start the animation from the center
        transform.localScale = Vector3.zero;

        // Scale the berry from zero to its full size
        float animationDuration = 0.3f; // Duration of the animation
        transform.DOScale(scale, animationDuration).SetEase(Ease.OutBack);

        gameObject.SetActive(true);
    }
    public Direction GetDirection()
    {
        return _direction;
    }
}

using DG.Tweening;
using UnityEngine;

public abstract class CellContent : MonoBehaviour, ICellContent, IGridItem
{
    protected int posX, posY, posZ;

    public int PosX => posX;
    public int PosY => posY;
    public int PosZ => posZ;

    public virtual void Activate()
    {
        // Reset scale to zero to start the animation from the center
        transform.localScale = Vector3.zero;

        // Scale the berry from zero to its full size
        float animationDuration = 0.3f; // Duration of the animation
        transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);

        gameObject.SetActive(true);
    }
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public virtual void Interact()
    {
        var scale = transform.lossyScale;
        transform.DOScale(scale * 1.25f, 0.15f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.15f);
        });
    }
    public virtual void SetCoordinates(int x, int y, int cellFloor)
    {
        posX = x;
        posY = y;
        posZ = cellFloor;
        gameObject.name = $"{GetType().Name}: ({x}) ({y}) ({cellFloor})";
    }
}

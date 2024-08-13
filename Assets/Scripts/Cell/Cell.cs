using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellContent _cellContent;
    private int posX, posY,posZ;
    private int colorValue;
    public Texture2D texture; // Texture for the cell

    public int PosY { get => posY; }
    public int PosX { get => posX; }
    public int PosZ { get => posZ; }

    public void Initialize(int colorValue, Texture2D texture = null)
    {
        this.colorValue = colorValue;
        this.texture = texture;
        ApplyTexture(texture);
    }
    private void ApplyTexture(Texture2D texture)
    {
        // Apply the texture to the block
        var renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
    }
    public void SetCoordinates(int x, int y, int cellFloor)
    {
        posX = x;
        posY = y;
        posZ = cellFloor;
        gameObject.name = "Cell: (" + x + ") (" + y + ") (" + cellFloor + ")";
    }
    public void SetActiveContent(CellContent content)
    {
        _cellContent = content;
        Activate();
    }
    public void SetPassiveContent(CellContent content)
    {
        _cellContent = content;
        Deactivate();
    }
    public bool HasBerry()
    {
        return _cellContent is Berry;
    }

    public bool HasArrow()
    {
        return _cellContent is Arrow;
    }
    public bool HasFrog()
    {
        return _cellContent is Frog;
    }
    
    public CellContent GetContent()
    {
        return _cellContent;
    }

    public void Activate()
    {
        _cellContent?.Activate();
    }

    public void Deactivate()
    {
        _cellContent?.Deactivate();
    }
}

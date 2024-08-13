using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public enum CellSituation
{
    Empty, // Empty tile
    HasCell // Cell
}
public class BaseCell : MonoBehaviour
{
    private CellSituation cellSituation;
    [SerializeField] private Stack<Cell> cells = new Stack<Cell>();
    private int posX, posY;

    public int PosY { get => posY; }
    public int PosX { get => posX; }

    public void SetCoordinates(int x, int y)
    {
        posX = x;
        posY = y;
        gameObject.name = "BaseCell: (" + x + ") (" + y + ")";
    }
    public void SetCellSituation(CellSituation cellSituation)
    {
        this.cellSituation = cellSituation;
    }

    public Stack<Cell> GetCells()
    {
        return cells;
    }
    public void AddCell(Cell cell)
    {
        cells.Push(cell);
    }
    public void RemoveCell()
    {
        if (cells.Count > 0)
        {
            var removedCell = cells.Pop();

            // Animate the scaling of the cell to zero with a squeezing effect
            removedCell.transform.DOScale(Vector3.zero, 0.7f)
                .SetEase(Ease.OutExpo) // Adjust the ease for a squeezing effect
                .OnComplete(() =>
                {
                   Destroy(removedCell);
                    
                });
            if (cells.Count > 0)
            {
                var topCell = GetTopCell();
                if (topCell != null)
                {
                    topCell.Activate(); // Activate the new top cell
                }
            }
        }
    }


    public Cell GetTopCell()
    {
        if (cells.Count > 0)
        {
            return cells.Peek();
        }
        return null;
    }

}



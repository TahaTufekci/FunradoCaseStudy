using Builders;
using Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        [SerializeField] private GridBuilder _gridBuilder;
        private List<BaseCell> _baseCellList = new List<BaseCell>();
        private List<Cell> _cellList = new List<Cell>();
        private List<Frog> _frogList = new List<Frog>();
        public List<BaseCell> BaseCellList => _baseCellList;
        public List<Cell> CellList => _cellList;
        public List<Frog> FrogList => _frogList;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _cellList = _gridBuilder.CellList;
            _baseCellList = _gridBuilder.BaseCellList;
            _frogList = _gridBuilder.FrogList;
        }

        public Cell GetNextCell(BaseCell currentBaseCell, Direction direction)
        {
            int newY = currentBaseCell.PosY;
            int newX = currentBaseCell.PosX;
            var arrowDirection = DirectionMapper.GetDirection(direction);

            if (direction == Direction.Up || direction == Direction.Down)
            {
                newY = currentBaseCell.PosY + arrowDirection;
            }
            else
            {
                newX = currentBaseCell.PosX + arrowDirection;
            }

            BaseCell nextBaseCell = GetBaseCell(newX, newY);
            if (nextBaseCell != null)
            {
                Cell topCell = nextBaseCell.GetTopCell();
                if (topCell != null)
                {
                    return topCell;
                }
            }
            return null;
        }

        public void DestroyBerryCell(GameObject berry)
        {
            var berryComponent = berry.GetComponent<Berry>();
            var berryCell = GetCell(berryComponent.PosX, berryComponent.PosY, berryComponent.PosZ);
            var baseCell = GetBaseCell(berryCell.PosX, berryCell.PosY);
            baseCell.RemoveCell();
        }

        public void DestroyArrowCell(GameObject arrow)
        {
            var arrowComponent = arrow.GetComponent<Arrow>();
            var arrowCell = GetCell(arrowComponent.PosX, arrowComponent.PosY, arrowComponent.PosZ);
            var baseCell = GetBaseCell(arrowCell.PosX, arrowCell.PosY);
            baseCell.RemoveCell();
        }
        public Cell GetCell(int col, int row, int cellFloor)
        {
            return _cellList.Find(cell => cell.PosX == col && cell.PosY == row && cell.PosZ == cellFloor);
        }
        public BaseCell GetBaseCell(int col, int row)
        {
            return _baseCellList.Find(cell => cell.PosX == col && cell.PosY == row);
        }
    }
}

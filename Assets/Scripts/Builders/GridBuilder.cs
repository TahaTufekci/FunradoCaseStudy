using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
    public class GridBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject baseCellPrefab;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject frogPrefab;
        [SerializeField] private GameObject berryPrefab;
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private GameObject backgroundPlane;
        [SerializeField] private List<CellSO> cellScriptableObjects;
        [SerializeField] private List<FrogSO> frogScriptableObjects;
        [SerializeField] private List<BerrySO> berryScriptableObjects;
        [SerializeField] private List<ArrowSO> arrowScriptableObjects;

        private Renderer _cellRenderer;
        private float _cellHeight;
        private float _cellWidth;
        private float _spawnPointXOffset;
        private float _spawnPointZOffset;

        private List<BaseCell> _baseCellList = new List<BaseCell>();
        private List<Cell> _cellList = new List<Cell>();
        private List<Frog> _frogList = new List<Frog>();

        public List<BaseCell> BaseCellList => _baseCellList;
        public List<Cell> CellList => _cellList;
        public List<Frog> FrogList => _frogList;

        private LevelData _currentLevel;

        private void Start()
        {
            InitializeLevelData();
            InitializeDimensions();
            PlaceBaseCells();
            PlaceCells();
            PlaceCellContents();
        }

        private void InitializeLevelData()
        {
            _currentLevel = LevelManager.Instance.CurrentLevel;
        }
        private void InitializeDimensions()
        {
            _cellRenderer = baseCellPrefab.GetComponent<Renderer>();
            _cellHeight = _cellRenderer.bounds.size.z;
            _cellWidth = _cellRenderer.bounds.size.x;
        }

        private void PlaceBaseCells()
        {
            _spawnPointXOffset = ((float)_currentLevel.columns / 2 * _cellWidth) - (_cellWidth / 2);
            _spawnPointZOffset = ((float)_currentLevel.rows / 2 * _cellHeight) - (_cellHeight / 2);

            for (int i = 0; i < _currentLevel.columns; i++)
            {
                for (int j = 0; j < _currentLevel.rows; j++)
                {
                    var cellPosition = CalculateBaseCellPosition(i, j);
                    var cellGameObject = Instantiate(baseCellPrefab, cellPosition, backgroundPlane.transform.rotation, backgroundPlane.transform);
                    var baseCell = cellGameObject.GetComponent<BaseCell>();
                    baseCell.SetCoordinates(i, j);
                    _baseCellList.Add(baseCell);
                }
            }
        }

        private Vector3 CalculateBaseCellPosition(int col, int row)
        {
            var xPosition = (col * _cellWidth) - _spawnPointXOffset;
            var zPosition = -(row * _cellHeight) + _spawnPointZOffset;
            return new Vector3(xPosition, 0.05f, zPosition);
        }

        private void PlaceCells()
        {
            // Sorted contents by floor level (z) in ascending order in order to prevent unintended instantiation
            _currentLevel.contents.Sort((a, b) => a.position.z.CompareTo(b.position.z));
            foreach (var content in _currentLevel.contents)
            {
                var cell = CreateCell(content.colorValue);
                cell.transform.position = CalculateCellPosition(content.position.x, content.position.y, content.position.z);
                _cellList.Add(cell);
                cell.SetCoordinates(content.position.x, content.position.y,content.position.z);
                GridManager.Instance.GetBaseCell(content.position.x, content.position.y).AddCell(cell);
            }
        }

        private Cell CreateCell(int colorValue)
        {
            return new CellBuilder()
                .SetColorValue(colorValue)
                .SetPrefab(cellPrefab)
                .SetScriptableObjects(cellScriptableObjects)
                .Build(backgroundPlane.transform);
        }

        private Vector3 CalculateCellPosition(int col, int row, int cellFloor)
        {
            var xPosition = (col * _cellWidth) - _spawnPointXOffset;
            var zPosition = -(row * _cellHeight) + _spawnPointZOffset;
            return new Vector3(xPosition, 0.05f + (0.085f * cellFloor), zPosition);
        }

        private void PlaceCellContents()
        {
            foreach (var content in _currentLevel.contents)
            {
                var cell = GridManager.Instance.GetCell(content.position.x, content.position.y, content.position.z);
                if (cell == null) continue;

                if (content.type == ContentType.Frog)
                {
                    PlaceFrog(cell, content.colorValue,content.direction);
                }
                else if (content.type == ContentType.Berry)
                {
                    PlaceBerry(cell, content.colorValue);
                }
                else if (content.type == ContentType.Arrow)
                {
                    PlaceArrow(cell, content.direction, content.colorValue);
                }
            }
        }

        private void PlaceFrog(Cell cell, int colorValue, Direction direction)
        {
            FrogBuilder frogBuilder = new FrogBuilder();
            frogBuilder.SetColorValue(colorValue);
            frogBuilder.SetScriptableObjects(frogScriptableObjects);
            frogBuilder.SetPrefab(frogPrefab);
            frogBuilder.SetDirection(direction);
    
            CellContent frog = frogBuilder.Build(cell);
 
            SetCellContent(cell, frog);
            _frogList.Add((Frog)frog);
        }

        private void PlaceBerry(Cell cell, int colorValue)
        {
            BerryBuilder berryBuilder = new BerryBuilder();
            berryBuilder.SetColorValue(colorValue);
            berryBuilder.SetScriptableObjects(berryScriptableObjects);
            berryBuilder.SetPrefab(berryPrefab);

            CellContent berry = berryBuilder.Build(cell);

            SetCellContent(cell, berry);
        }

        private void PlaceArrow(Cell cell, Direction direction, int colorValue)
        {
            ArrowBuilder arrowBuilder = new ArrowBuilder();
            arrowBuilder.SetColorValue(colorValue);
            arrowBuilder.SetScriptableObjects(arrowScriptableObjects);
            arrowBuilder.SetPrefab(arrowPrefab);
            arrowBuilder.SetDirection(direction);

            CellContent arrow = arrowBuilder.Build(cell);

            SetCellContent(cell, arrow);
        }
        private void SetCellContent(Cell cell, CellContent content)
        {
            var baseCell = GridManager.Instance.GetBaseCell(cell.PosX, cell.PosY);
            var topCell = baseCell.GetTopCell();

            if (topCell != null && topCell.Equals(cell))
            {
                cell.SetActiveContent(content);
            }
            else
            {
                cell.SetPassiveContent(content);
            }
        }

    }
}

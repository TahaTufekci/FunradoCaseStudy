using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
    public class CellBuilder
    {
        private int colorValue;
        private GameObject prefab;
        private List<CellSO> cellScriptableObjects;

        public CellBuilder SetColorValue(int colorValue)
        {
            this.colorValue = colorValue;
            return this;
        }


        public CellBuilder SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            return this;
        }

        public CellBuilder SetScriptableObjects(List<CellSO> blockScriptableObjects)
        {
            this.cellScriptableObjects = blockScriptableObjects;
            return this;
        }

        public Cell Build(Transform parent)
        {
            var matchingCellSO = cellScriptableObjects.Find(cellSO =>cellSO.colorValue == colorValue);

            if (matchingCellSO != null)
            {
                var cellObj = Object.Instantiate(prefab, parent);
                var cell = cellObj.GetComponent<Cell>();
                cell.Initialize(matchingCellSO.colorValue, matchingCellSO.texture);
                return cell;
            }

            Debug.LogError("No matching CellSO found for the color value.");
            return null;
        }
    }
}